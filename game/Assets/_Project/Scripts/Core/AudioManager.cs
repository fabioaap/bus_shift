using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using BusShift.Core;

namespace BusShift.Core
{
    /// <summary>
    /// Singleton SFX pool manager for Bus Shift.
    ///
    /// <para>
    /// Maintains a warm pool of <see cref="_poolSize"/> <see cref="AudioSource"/>
    /// children, all configured for 2-D audio.  When a one-shot effect is requested,
    /// the first idle (non-playing) source is claimed; if all sources are busy the
    /// oldest one is recycled.
    /// </para>
    ///
    /// <para>
    /// <b>Dedicated sources</b> (not part of the pool):
    /// <list type="bullet">
    ///   <item><see cref="_sanityLoopSource"/> – low-sanity ambient loop, automatically
    ///   activated when <see cref="SanitySystem.CurrentSanity"/> &lt; 20 %.</item>
    ///   <item><see cref="_hornSource"/> – bus horn triggered by the <b>H</b> key.
    ///   Uses the new Unity Input System.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Uses <c>DontDestroyOnLoad</c> so the manager persists across scene loads.
    /// Only one instance is allowed; duplicates self-destruct on <c>Awake</c>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires the <b>Unity Input System</b> package.
    /// In Project Settings → Player → Active Input Handling, select
    /// <i>Both</i> or <i>Input System Package (New)</i>.
    /// </remarks>
    public class AudioManager : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Singleton
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Singleton instance.  Null until the first scene that contains the prefab is loaded.</summary>
        public static AudioManager Instance { get; private set; }

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — SFX Clips
        // ─────────────────────────────────────────────────────────────────────

        [Header("SFX Pool")]
        [Tooltip("Number of AudioSource slots in the pool.  Increase if SFX are cut off.")]
        [SerializeField] [Range(4, 32)] private int _poolSize = 10;

        [Header("Named SFX Clips")]
        [Tooltip("Played when a ghost appears.  Use PlayGhostAppearance() or index directly.")]
        [SerializeField] private AudioClip[] ghostAppearanceSFX;

        [Tooltip("Door open effect.")]
        [SerializeField] private AudioClip[] doorOpenSFX;

        [Tooltip("Door close effect.")]
        [SerializeField] private AudioClip[] doorCloseSFX;

        [Tooltip("Bus engine start / rev.")]
        [SerializeField] private AudioClip[] busEngineSFX;

        [Tooltip("Ambient loop played when sanity drops below 20 %.  Assign to a looping clip.")]
        [SerializeField] private AudioClip sanityLowLoopSFX;

        [Tooltip("Sound played when the player presses the bus horn (H key).")]
        [SerializeField] private AudioClip hornSFX;

        [Header("Sanity Warning")]
        [Tooltip("Sanity threshold (0–1) below which the sanity-low loop activates.")]
        [SerializeField] [Range(0f, 1f)] private float _sanityLowThreshold = 0.20f;

        [Header("Horn")]
        [Tooltip("Master volume for the horn AudioSource.")]
        [SerializeField] [Range(0f, 1f)] private float _hornVolume = 1f;

        [Tooltip("Cooldown in seconds between horn presses.")]
        [SerializeField] [Range(0.1f, 5f)] private float _hornCooldown = 0.5f;

        // ─────────────────────────────────────────────────────────────────────
        //  Private — pool
        // ─────────────────────────────────────────────────────────────────────

        private AudioSource[] _pool;

        /// <summary>
        /// Ring-buffer cursor used when all pool sources are busy.
        /// We recycle the "oldest-started" slot by round-robining.
        /// </summary>
        private int _recycleIndex;

        // ─────────────────────────────────────────────────────────────────────
        //  Private — dedicated sources
        // ─────────────────────────────────────────────────────────────────────

        private AudioSource _sanityLoopSource;
        private AudioSource _hornSource;
        private float       _hornCooldownRemaining;

        // ─────────────────────────────────────────────────────────────────────
        //  Unity lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            // Singleton guard
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            BuildPool();
            BuildDedicatedSources();
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
            HandleHornInput();
            TickHornCooldown();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API — SFX Pool
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Plays a one-shot audio clip from the pool at the AudioListener's position (2-D).
        /// </summary>
        /// <param name="clip">The clip to play.  Ignored if null.</param>
        /// <param name="volume">Playback volume (0–1).</param>
        /// <param name="pitch">Playback pitch multiplier.</param>
        public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null) return;

            var src = GetAvailableSource();
            src.spatialBlend = 0f;           // 2-D
            src.transform.localPosition = Vector3.zero;
            src.pitch  = pitch;
            src.volume = Mathf.Clamp01(volume);
            src.PlayOneShot(clip);
        }

        /// <summary>
        /// Plays a one-shot audio clip from the pool at a specific world position (3-D).
        /// </summary>
        /// <param name="clip">The clip to play.  Ignored if null.</param>
        /// <param name="worldPosition">World-space position from which the sound emanates.</param>
        /// <param name="volume">Playback volume (0–1).</param>
        public void PlaySFX(AudioClip clip, Vector3 worldPosition, float volume = 1f)
        {
            if (clip == null) return;

            var src = GetAvailableSource();
            src.transform.position = worldPosition;
            src.spatialBlend = 1f;           // full 3-D
            src.volume = Mathf.Clamp01(volume);
            src.pitch  = 1f;
            src.PlayOneShot(clip);
        }

        /// <summary>
        /// Plays a one-shot audio clip after a delay (2-D).
        /// </summary>
        /// <param name="clip">The clip to play.  Ignored if null.</param>
        /// <param name="delay">Delay in seconds before playback begins.</param>
        /// <param name="volume">Playback volume (0–1).</param>
        public void PlaySFXWithDelay(AudioClip clip, float delay, float volume = 1f)
        {
            if (clip == null) return;
            StartCoroutine(DelayedPlayCoroutine(clip, delay, volume));
        }

        /// <summary>Stops all pooled SFX AudioSources immediately.</summary>
        public void StopAllSFX()
        {
            if (_pool == null) return;
            foreach (var src in _pool)
            {
                if (src != null && src.isPlaying)
                    src.Stop();
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API — Named SFX helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Plays a random clip from <see cref="ghostAppearanceSFX"/> at the given position.
        /// </summary>
        /// <param name="worldPosition">World position of the ghost.</param>
        public void PlayGhostAppearance(Vector3 worldPosition)
        {
            PlayRandom(ghostAppearanceSFX, worldPosition);
        }

        /// <summary>Plays a random clip from <see cref="doorOpenSFX"/> (2-D).</summary>
        public void PlayDoorOpen()   => PlayRandom(doorOpenSFX);

        /// <summary>Plays a random clip from <see cref="doorCloseSFX"/> (2-D).</summary>
        public void PlayDoorClose()  => PlayRandom(doorCloseSFX);

        /// <summary>Plays a random clip from <see cref="busEngineSFX"/> (2-D).</summary>
        public void PlayBusEngine()  => PlayRandom(busEngineSFX);

        /// <summary>
        /// Immediately plays the bus horn from <see cref="_hornSource"/>,
        /// bypassing the Input System (useful for programmatic triggers).
        /// Respects <see cref="_hornCooldown"/>.
        /// </summary>
        public void PlayBusHorn()
        {
            if (_hornCooldownRemaining > 0f) return;
            if (hornSFX == null || _hornSource == null) return;

            _hornSource.volume = _hornVolume;
            _hornSource.PlayOneShot(hornSFX);
            _hornCooldownRemaining = _hornCooldown;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("[AudioManager] Bus horn.");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Internal — pool management
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns the next idle <see cref="AudioSource"/> from the pool.
        /// If all sources are busy, the oldest one (round-robin) is recycled.
        /// </summary>
        /// <returns>A ready-to-use <see cref="AudioSource"/>.</returns>
        private AudioSource GetAvailableSource()
        {
            // First pass: find a source that is not currently playing
            foreach (var src in _pool)
            {
                if (src != null && !src.isPlaying)
                    return src;
            }

            // All busy: recycle in round-robin order
            var recycled = _pool[_recycleIndex % _pool.Length];
            _recycleIndex = (_recycleIndex + 1) % _pool.Length;

            if (recycled != null)
                recycled.Stop();

            return recycled;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Creates the pool of AudioSource children at startup.</summary>
        private void BuildPool()
        {
            _pool = new AudioSource[_poolSize];

            for (int i = 0; i < _poolSize; i++)
            {
                var child = new GameObject($"SFX_Pool_{i:00}");
                child.transform.SetParent(transform, false);

                var src = child.AddComponent<AudioSource>();
                src.playOnAwake  = false;
                src.spatialBlend = 0f;    // default: 2-D
                src.loop         = false;

                _pool[i] = src;
            }
        }

        /// <summary>Creates the dedicated <see cref="_sanityLoopSource"/> and <see cref="_hornSource"/>.</summary>
        private void BuildDedicatedSources()
        {
            // ── Sanity loop ────────────────────────────────────────────────
            var sanityChild = new GameObject("SFX_SanityLoop");
            sanityChild.transform.SetParent(transform, false);

            _sanityLoopSource             = sanityChild.AddComponent<AudioSource>();
            _sanityLoopSource.clip        = sanityLowLoopSFX;
            _sanityLoopSource.loop        = true;
            _sanityLoopSource.playOnAwake = false;
            _sanityLoopSource.spatialBlend = 0f;
            _sanityLoopSource.volume      = 0f;

            // ── Horn ───────────────────────────────────────────────────────
            var hornChild = new GameObject("SFX_Horn");
            hornChild.transform.SetParent(transform, false);

            _hornSource             = hornChild.AddComponent<AudioSource>();
            _hornSource.playOnAwake = false;
            _hornSource.spatialBlend = 0f;
            _hornSource.loop        = false;
        }

        /// <summary>
        /// Picks a random <see cref="AudioClip"/> from <paramref name="clips"/> and
        /// plays it as a 2-D one-shot.
        /// </summary>
        private void PlayRandom(AudioClip[] clips, float volume = 1f)
        {
            if (clips == null || clips.Length == 0) return;
            var clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            PlaySFX(clip, volume);
        }

        /// <summary>Plays a random clip from <paramref name="clips"/> at a world position (3-D).</summary>
        private void PlayRandom(AudioClip[] clips, Vector3 worldPos, float volume = 1f)
        {
            if (clips == null || clips.Length == 0) return;
            var clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            PlaySFX(clip, worldPos, volume);
        }

        /// <summary>Coroutine backing <see cref="PlaySFXWithDelay"/>.</summary>
        private IEnumerator DelayedPlayCoroutine(AudioClip clip, float delay, float volume)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            PlaySFX(clip, volume);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Input — horn
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Polls the H key via the new Input System and fires the horn.</summary>
        private void HandleHornInput()
        {
            var kb = Keyboard.current;
            if (kb == null) return;

            if (kb.hKey.wasPressedThisFrame)
                PlayBusHorn();
        }

        /// <summary>Counts down the horn cooldown each frame.</summary>
        private void TickHornCooldown()
        {
            if (_hornCooldownRemaining > 0f)
                _hornCooldownRemaining -= Time.deltaTime;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Event handlers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Activates or deactivates the sanity-low ambient loop based on the
        /// current sanity value reported by <see cref="SanitySystem"/>.
        /// </summary>
        /// <param name="sanity">Current sanity in [0, 1].</param>
        private void HandleSanityChanged(float sanity)
        {
            if (_sanityLoopSource == null || sanityLowLoopSFX == null) return;

            bool shouldPlay = sanity < _sanityLowThreshold;

            if (shouldPlay && !_sanityLoopSource.isPlaying)
            {
                _sanityLoopSource.volume = 1f;
                _sanityLoopSource.Play();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[AudioManager] Sanity low ({sanity:P0}) — loop started.");
#endif
            }
            else if (!shouldPlay && _sanityLoopSource.isPlaying)
            {
                _sanityLoopSource.Stop();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[AudioManager] Sanity restored ({sanity:P0}) — loop stopped.");
#endif
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Editor helpers
        // ─────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            _poolSize            = Mathf.Max(1, _poolSize);
            _sanityLowThreshold  = Mathf.Clamp01(_sanityLowThreshold);
            _hornVolume          = Mathf.Clamp01(_hornVolume);
            _hornCooldown        = Mathf.Max(0.1f, _hornCooldown);
        }
#endif
    }
}
