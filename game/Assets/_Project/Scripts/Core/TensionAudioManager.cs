using System.Collections;
using UnityEngine;
using BusShift.Core;

namespace BusShift.Core
{
    /// <summary>
    /// Reactive audio manager that crossfades four tension tracks based on the
    /// driver's current sanity level, as reported by <see cref="SanitySystem.OnSanityChanged"/>.
    ///
    /// <para>
    /// <b>Track mapping (sanity → track index):</b>
    /// <list type="bullet">
    ///   <item><b>Track 0</b> (0 – 25 %)  — Calm ambience: wind, soft engine.</item>
    ///   <item><b>Track 1</b> (25 – 50 %) — Low heartbeat.</item>
    ///   <item><b>Track 2</b> (50 – 75 %) — Whispers + rapid heartbeat.</item>
    ///   <item><b>Track 3</b> (75 – 100%) — Muffled screams + static.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Each track gets its own <see cref="AudioSource"/> child created at runtime.
    /// Crossfades are driven every frame via <c>Mathf.MoveTowards</c> over
    /// <see cref="CrossfadeDuration"/> seconds — no coroutines needed for the
    /// normal flow, keeping it lightweight.
    /// </para>
    ///
    /// <para>
    /// Call <see cref="TriggerJumpscare"/> to temporarily amplify audio and
    /// request a camera shake via the static event <see cref="OnCameraShakeRequested"/>.
    /// Note: Unity <see cref="AudioSource.volume"/> is clamped to [0, 1]; the
    /// 1.5× multiplier is applied by boosting <see cref="AudioListener.volume"/>
    /// for the duration, then restoring it.
    /// </para>
    /// </summary>
    public class TensionAudioManager : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Static Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired by <see cref="TriggerJumpscare"/> to request a camera shake.
        /// Argument is the shake duration in seconds.
        /// Subscribe from a camera shake component or <c>CameraSystem</c>.
        /// </summary>
        public static event System.Action<float> OnCameraShakeRequested;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector
        // ─────────────────────────────────────────────────────────────────────

        [Header("Tension Tracks (4 clips in order)")]
        [Tooltip("Four audio clips indexed 0–3 matching the tension stages.\n" +
                 "0: calm  1: low-heartbeat  2: whispers  3: screams+static")]
        [SerializeField] private AudioClip[] _tensionTracks = new AudioClip[4];

        [Header("Crossfade")]
        [Tooltip("Time in seconds to fully transition from one track to another.")]
        [SerializeField] private float _crossfadeDuration = 2f;

        [Tooltip("Master volume applied to all tension tracks (not the jumpscare multiplier).")]
        [SerializeField] [Range(0f, 1f)] private float _masterVolume = 1f;

        // ─────────────────────────────────────────────────────────────────────
        //  Private state
        // ─────────────────────────────────────────────────────────────────────

        private AudioSource[] _sources;          // one per track
        private float[]       _targetVolumes;    // desired volume for each source
        private int           _activeTrackIndex = 0;
        private bool          _jumpscareActive  = false;

        // We store the original AudioListener volume so we can restore it after a jumpscare.
        private float _listenerVolumeBeforeJumpscare = 1f;

        // ─────────────────────────────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            BuildAudioSources();
        }

        private void OnEnable()
        {
            SanitySystem.OnSanityChanged += HandleSanityChanged;
        }

        private void OnDisable()
        {
            SanitySystem.OnSanityChanged -= HandleSanityChanged;
        }

        private void Update()
        {
            if (_sources == null) return;
            if (_jumpscareActive) return;   // coroutine controls volumes during jumpscare

            ApplyCrossfade();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Triggers a jumpscare effect: raises <see cref="AudioListener.volume"/> to
        /// 1.5× its current value (clamped to the Unity maximum of 1.0) for
        /// <paramref name="duration"/> seconds, then restores it.
        /// Also fires <see cref="OnCameraShakeRequested"/> with the same duration.
        /// </summary>
        /// <param name="duration">Duration of the jumpscare amplification in seconds.</param>
        public void TriggerJumpscare(float duration)
        {
            if (_jumpscareActive)
            {
                StopAllCoroutines();
                RestoreListenerVolume();
            }

            StartCoroutine(JumpscareRoutine(duration));
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Event handlers
        // ─────────────────────────────────────────────────────────────────────

        private void HandleSanityChanged(float sanity)
        {
            // Map 0-1 sanity to track index 0-3.
            // Clamp ensures sanity == 1 maps to track 3, not index 4.
            int newIndex = Mathf.Clamp(Mathf.FloorToInt(sanity * 4f), 0, 3);

            if (newIndex == _activeTrackIndex) return;

            _activeTrackIndex = newIndex;
            RefreshTargetVolumes();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[TensionAudioManager] Sanity {sanity:P0} → track {_activeTrackIndex}.");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Creates one AudioSource child per tension track and starts playback
        /// at zero volume so crossfades work from the first frame.
        /// </summary>
        private void BuildAudioSources()
        {
            const int TrackCount = 4;

            _sources       = new AudioSource[TrackCount];
            _targetVolumes = new float[TrackCount];

            for (int i = 0; i < TrackCount; i++)
            {
                var child  = new GameObject($"TensionTrack_{i}");
                child.transform.SetParent(transform, false);

                var src = child.AddComponent<AudioSource>();
                src.clip        = (i < _tensionTracks.Length) ? _tensionTracks[i] : null;
                src.loop        = true;
                src.volume      = 0f;
                src.playOnAwake = false;
                src.spatialBlend = 0f;  // 2-D, no positional audio

                if (src.clip != null)
                    src.Play();

                _sources[i]       = src;
                _targetVolumes[i] = 0f;
            }

            // Track 0 is active by default (calm ambience at game start).
            _targetVolumes[0] = _masterVolume;
        }

        /// <summary>
        /// Sets the desired volume for all tracks: 1 for the active track, 0 for the rest.
        /// The actual volume change happens gradually in <see cref="ApplyCrossfade"/>.
        /// </summary>
        private void RefreshTargetVolumes()
        {
            for (int i = 0; i < _targetVolumes.Length; i++)
                _targetVolumes[i] = (i == _activeTrackIndex) ? _masterVolume : 0f;
        }

        /// <summary>
        /// Moves each AudioSource volume toward its target each frame.
        /// Step size is calculated from <see cref="_crossfadeDuration"/>.
        /// </summary>
        private void ApplyCrossfade()
        {
            // Avoid division by zero: if crossfade duration is tiny, snap immediately.
            float step = _crossfadeDuration > 0f
                ? (_masterVolume / _crossfadeDuration) * Time.deltaTime
                : _masterVolume;

            for (int i = 0; i < _sources.Length; i++)
            {
                if (_sources[i] == null) continue;
                _sources[i].volume = Mathf.MoveTowards(_sources[i].volume, _targetVolumes[i], step);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Jumpscare coroutine
        // ─────────────────────────────────────────────────────────────────────

        private IEnumerator JumpscareRoutine(float duration)
        {
            _jumpscareActive = true;

            // Boost AudioListener volume by 1.5× (Unity hard-clamps at 1.0).
            _listenerVolumeBeforeJumpscare = AudioListener.volume;
            AudioListener.volume           = Mathf.Clamp01(_listenerVolumeBeforeJumpscare * 1.5f);

            // Force active track to full volume instantly for the stab effect.
            if (_sources != null && _activeTrackIndex < _sources.Length
                                 && _sources[_activeTrackIndex] != null)
            {
                _sources[_activeTrackIndex].volume = _masterVolume;
            }

            // Notify CameraSystem (or any listener) to apply a light shake.
            OnCameraShakeRequested?.Invoke(duration);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[TensionAudioManager] Jumpscare started ({duration}s).");
#endif

            yield return new WaitForSeconds(duration);

            RestoreListenerVolume();
            _jumpscareActive = false;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("[TensionAudioManager] Jumpscare ended — audio restored.");
#endif
        }

        private void RestoreListenerVolume()
        {
            AudioListener.volume = _listenerVolumeBeforeJumpscare;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Editor helpers
        // ─────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            _crossfadeDuration = Mathf.Max(0.01f, _crossfadeDuration);
            _masterVolume      = Mathf.Clamp01(_masterVolume);
        }
#endif
    }
}
