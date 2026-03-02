using System.Collections;
using UnityEngine;
using BusShift.Bus;

namespace BusShift.Ghosts
{
    /// <summary>
    /// Scene-level manager that determines which active ghosts are visible to the player
    /// through the rearview mirror or the security (CCTV) camera.
    /// <para>
    /// Visibility is evaluated every <see cref="PollInterval"/> seconds (default 0.1 s)
    /// using Unity's <see cref="GeometryUtility.TestPlanesAABB"/> frustum test.
    /// When a ghost enters a camera frustum, <see cref="GhostBase.SetObserved"/> is set
    /// to <c>true</c>; when it leaves or the camera is inactive it is set to <c>false</c>.
    /// </para>
    /// <para>
    /// Mirror visibility is only counted while the player is actively focusing the mirror
    /// (<see cref="CameraSystem.IsMirrorFocused"/> == <c>true</c>).
    /// Security camera visibility is only counted while the CCTV feed is clean
    /// (<see cref="CameraSystem.IsCameraClean"/> == <c>true</c>).
    /// </para>
    /// <para>
    /// <b>Setup:</b> Place this component on any persistent scene GameObject (e.g. the
    /// GhostManager or GameManager). Assign <see cref="MirrorCamera"/> and
    /// <see cref="SecurityCamera"/> in the Inspector — both can reference the same
    /// <c>RearCamera</c> if the bus uses a single rear-facing camera for both purposes.
    /// <see cref="CameraSystem"/> is resolved automatically via
    /// <see cref="Object.FindAnyObjectByType{T}"/> if left unassigned.
    /// </para>
    /// </summary>
    public class GhostVisibilityChecker : MonoBehaviour
    {
        // ── Inspector ─────────────────────────────────────────────────────────

        [Header("Observation Cameras")]
        [Tooltip("Camera used for the rearview mirror. Visibility is counted only while IsMirrorFocused is true.")]
        [SerializeField] private Camera _mirrorCamera;

        [Tooltip("Camera used for the security / CCTV monitor. Visibility is counted only while IsCameraClean is true.")]
        [SerializeField] private Camera _securityCamera;

        [Header("Poll Settings")]
        [Tooltip("Seconds between visibility checks. Lower values are more responsive but more expensive.")]
        [SerializeField] private float _pollInterval = 0.1f;

        // ── Runtime ───────────────────────────────────────────────────────────

        /// <summary>Resolved in <see cref="Start"/>; used to query mirror / camera state.</summary>
        private CameraSystem _cameraSystem;

        // ── Unity lifecycle ───────────────────────────────────────────────────

        private void Start()
        {
            _cameraSystem = FindAnyObjectByType<CameraSystem>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (_cameraSystem == null)
                Debug.LogWarning("[GhostVisibilityChecker] CameraSystem not found in scene. " +
                                 "Mirror/security-camera state checks will be skipped — ghosts will " +
                                 "be considered observed whenever they enter the frustum.");

            if (_mirrorCamera == null && _securityCamera == null)
                Debug.LogWarning("[GhostVisibilityChecker] Neither MirrorCamera nor SecurityCamera " +
                                 "is assigned. No ghost visibility will ever be detected.");
#endif

            StartCoroutine(PollVisibilityLoop());
        }

        // ── Poll coroutine ────────────────────────────────────────────────────

        /// <summary>
        /// Coroutine that evaluates ghost visibility every <see cref="_pollInterval"/> seconds.
        /// Using a coroutine avoids the overhead of a per-frame Update call.
        /// </summary>
        private IEnumerator PollVisibilityLoop()
        {
            var wait = new WaitForSeconds(_pollInterval);

            while (true)
            {
                CheckAllGhosts();
                yield return wait;
            }
        }

        // ── Visibility logic ──────────────────────────────────────────────────

        /// <summary>
        /// Evaluates all active <see cref="GhostBase"/> instances and updates their
        /// observed state based on whether they fall inside an active camera's frustum.
        /// </summary>
        private void CheckAllGhosts()
        {
            // Determine which cameras are currently usable.
            bool mirrorActive   = IsMirrorActive();
            bool securityActive = IsSecurityActive();

            // Pre-compute frustum planes only for active cameras (avoids redundant work).
            Plane[] mirrorPlanes   = mirrorActive   ? GeometryUtility.CalculateFrustumPlanes(_mirrorCamera)   : null;
            Plane[] securityPlanes = securityActive ? GeometryUtility.CalculateFrustumPlanes(_securityCamera) : null;

            // No active camera → mark every ghost as unobserved and bail out early.
            if (mirrorPlanes == null && securityPlanes == null)
            {
                foreach (var ghost in FindObjectsByType<GhostBase>(FindObjectsSortMode.None))
                    ghost?.SetObserved(false);
                return;
            }

            // Test each active ghost against the frustum planes.
            foreach (var ghost in FindObjectsByType<GhostBase>(FindObjectsSortMode.None))
            {
                if (ghost == null || !ghost.isActiveAndEnabled) continue;

                Bounds bounds  = GetGhostBounds(ghost);
                bool observed  = false;

                if (mirrorPlanes != null)
                    observed |= GeometryUtility.TestPlanesAABB(mirrorPlanes, bounds);

                if (!observed && securityPlanes != null)
                    observed |= GeometryUtility.TestPlanesAABB(securityPlanes, bounds);

                ghost.SetObserved(observed);
            }
        }

        // ── Camera state helpers ──────────────────────────────────────────────

        /// <summary>
        /// Returns <c>true</c> when the mirror camera is assigned AND the player is
        /// actively looking through it.
        /// Falls back to <c>true</c> when no <see cref="CameraSystem"/> is available
        /// (editor/test scenarios without a full scene setup).
        /// </summary>
        private bool IsMirrorActive()
        {
            if (_mirrorCamera == null) return false;
            if (_cameraSystem == null) return true;   // graceful fallback
            return _cameraSystem.IsMirrorFocused;
        }

        /// <summary>
        /// Returns <c>true</c> when the security camera is assigned AND the CCTV feed
        /// is currently free of static noise.
        /// Falls back to <c>true</c> when no <see cref="CameraSystem"/> is available.
        /// </summary>
        private bool IsSecurityActive()
        {
            if (_securityCamera == null) return false;
            if (_cameraSystem == null) return true;   // graceful fallback
            return _cameraSystem.IsCameraClean;
        }

        // ── Bounds resolution ─────────────────────────────────────────────────

        /// <summary>
        /// Resolves the world-space AABB for a ghost.
        /// Priority: <see cref="Collider"/> bounds → <see cref="Renderer"/> bounds (children included) →
        /// unit cube centred on the ghost's transform position as a last resort.
        /// </summary>
        /// <param name="ghost">The ghost whose bounds are needed.</param>
        /// <returns>A world-space <see cref="Bounds"/> suitable for frustum testing.</returns>
        private static Bounds GetGhostBounds(GhostBase ghost)
        {
            var col = ghost.GetComponent<Collider>();
            if (col != null) return col.bounds;

            var rend = ghost.GetComponentInChildren<Renderer>();
            if (rend != null) return rend.bounds;

            // Fallback: 1 m³ cube centred on the ghost (safe minimum for frustum test).
            return new Bounds(ghost.transform.position, Vector3.one);
        }

        // ── Inspector-exposed references ──────────────────────────────────────

        /// <summary>
        /// The camera assigned as the rearview mirror source.
        /// Assign in the Inspector or set at runtime before the component starts.
        /// </summary>
        public Camera MirrorCamera
        {
            get => _mirrorCamera;
            set => _mirrorCamera = value;
        }

        /// <summary>
        /// The camera assigned as the security / CCTV monitor source.
        /// Assign in the Inspector or set at runtime before the component starts.
        /// </summary>
        public Camera SecurityCamera
        {
            get => _securityCamera;
            set => _securityCamera = value;
        }
    }
}
