using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BusShift.Bus;

namespace BusShift.UI
{
    /// <summary>
    /// Interactive route map toggled by the TAB key.
    ///
    /// When open:
    ///   • A semi-opaque overlay blocks ~40 % of the driver's view (immersive tension)
    ///   • A red dot tracks the bus world-position mapped to UI coordinates
    ///   • Up to 5 stop waypoints are marked; the next upcoming stop pulses
    ///
    /// World → UI mapping uses a configurable <see cref="worldBounds"/> rect
    /// and the UI map's RectTransform as the canvas space reference.
    ///
    /// Requires <see cref="RouteManager"/> in the scene (assign via Inspector).
    /// </summary>
    public class MapUI : MonoBehaviour
    {
        // ── Inspector ─────────────────────────────────────────────────────────

        [Header("Canvas Groups")]
        [Tooltip("CanvasGroup on the map panel itself.")]
        [SerializeField] private CanvasGroup mapCanvasGroup;

        [Tooltip("Full-screen semi-opaque overlay that sits behind the map panel, " +
                 "blocking ~40 % of the player's view.")]
        [SerializeField] private CanvasGroup overlayCanvasGroup;

        [Header("Map Elements")]
        [Tooltip("RectTransform of the red dot that tracks the bus.")]
        [SerializeField] private RectTransform busDot;

        [Tooltip("Exactly 5 RectTransform markers, one per route stop. " +
                 "Extra markers are hidden if the route has fewer stops.")]
        [SerializeField] private RectTransform[] stopMarkers = new RectTransform[5];

        [Tooltip("RectTransform of the map image itself. " +
                 "Its rect defines the UI coordinate space for position mapping.")]
        [SerializeField] private RectTransform mapRect;

        [Header("Route")]
        [Tooltip("RouteManager in the scene. Provides waypoint list and bus transform.")]
        [SerializeField] private RouteManager routeManager;

        [Header("Fade")]
        [Tooltip("Alpha of the full-screen overlay when the map is open (0 = none, 1 = fully opaque).")]
        [SerializeField] [Range(0f, 1f)] private float overlayTargetAlpha = 0.4f;

        [Tooltip("Speed of fade transitions.")]
        [SerializeField] [Range(1f, 20f)] private float fadeSpeed = 8f;

        [Header("World Bounds")]
        [Tooltip("World-space XZ rectangle that maps to the full map image. " +
                 "Set x/y to the minimum corner (world X,Z) and width/height to the extents.")]
        [SerializeField] private Rect worldBounds = new Rect(-200f, -100f, 400f, 200f);

        [Header("Pulse (next stop)")]
        [Tooltip("Pulse animation speed in cycles per second.")]
        [SerializeField] [Range(0.5f, 5f)] private float pulseSpeed = 2f;

        [Tooltip("Minimum scale of the pulsing next-stop marker.")]
        [SerializeField] [Range(0.5f, 1f)]  private float pulseMinScale = 0.8f;

        [Tooltip("Maximum scale of the pulsing next-stop marker.")]
        [SerializeField] [Range(1f, 2f)]    private float pulseMaxScale = 1.4f;

        [Header("Marker Colors")]
        [SerializeField] private Color nextStopColor   = Color.yellow;
        [SerializeField] private Color visitedColor    = new Color(0.5f, 0.5f, 0.5f);
        [SerializeField] private Color pendingColor    = Color.white;

        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>True while the map panel is visible.</summary>
        public bool IsOpen { get; private set; }

        // ── Private state ─────────────────────────────────────────────────────

        private float _targetMapAlpha;
        private float _targetOverlayAlpha;
        private float _pulseTime;

        // Cache of stop waypoint indices built once from RouteManager.Waypoints
        private readonly List<int> _stopIndices = new List<int>(5);

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void Awake()
        {
            // Start hidden
            if (mapCanvasGroup     != null) mapCanvasGroup.alpha     = 0f;
            if (overlayCanvasGroup != null) overlayCanvasGroup.alpha = 0f;

            SetInteractable(false);
            BuildStopIndex();
            InitialiseMarkers();
        }

        private void Update()
        {
            HandleInput();
            UpdateFades();

            if (IsOpen)
            {
                UpdateBusDot();
                UpdateStopMarkers();
            }
        }

        // ── Private logic ─────────────────────────────────────────────────────

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                ToggleMap();
        }

        private void ToggleMap()
        {
            IsOpen = !IsOpen;

            _targetMapAlpha     = IsOpen ? 1f : 0f;
            _targetOverlayAlpha = IsOpen ? overlayTargetAlpha : 0f;

            SetInteractable(IsOpen);
        }

        private void UpdateFades()
        {
            float dt = Time.deltaTime * fadeSpeed;

            if (mapCanvasGroup != null)
                mapCanvasGroup.alpha = Mathf.Lerp(mapCanvasGroup.alpha, _targetMapAlpha, dt);

            if (overlayCanvasGroup != null)
                overlayCanvasGroup.alpha = Mathf.Lerp(overlayCanvasGroup.alpha, _targetOverlayAlpha, dt);
        }

        /// <summary>
        /// Converts the bus world position to a UI position relative to the map image rect.
        /// Uses the XZ plane (Y is height in world space, irrelevant for a top-down map).
        /// </summary>
        private void UpdateBusDot()
        {
            if (busDot == null || routeManager == null || mapRect == null) return;
            if (routeManager.BusTransform == null) return;

            Vector3 worldPos = routeManager.BusTransform.position;

            // Normalise world XZ inside the configured worldBounds rect (0–1)
            float nx = Mathf.InverseLerp(worldBounds.xMin, worldBounds.xMax, worldPos.x);
            float ny = Mathf.InverseLerp(worldBounds.yMin, worldBounds.yMax, worldPos.z);

            // Map to UI rect coordinates (anchored at centre of mapRect)
            Rect r = mapRect.rect;
            busDot.anchoredPosition = new Vector2(
                Mathf.Lerp(r.xMin, r.xMax, nx),
                Mathf.Lerp(r.yMin, r.yMax, ny)
            );
        }

        /// <summary>
        /// Positions stop markers on the map and animates the pulse on the next upcoming stop.
        /// </summary>
        private void UpdateStopMarkers()
        {
            if (routeManager == null || mapRect == null) return;

            _pulseTime += Time.deltaTime * pulseSpeed;

            // Smooth sine pulse: maps sin output (-1..1) to (pulseMinScale..pulseMaxScale)
            float pulseFactor = Mathf.Lerp(pulseMinScale, pulseMaxScale,
                (Mathf.Sin(_pulseTime * Mathf.PI * 2f) + 1f) * 0.5f);

            int nextStopAbsoluteIndex = FindNextStopWaypointIndex();

            for (int i = 0; i < stopMarkers.Length; i++)
            {
                if (stopMarkers[i] == null) continue;

                bool hasData = i < _stopIndices.Count;
                stopMarkers[i].gameObject.SetActive(hasData);

                if (!hasData) continue;

                int waypointIdx = _stopIndices[i];
                Vector3 wp      = routeManager.Waypoints[waypointIdx].Position;

                // Position the marker on the map
                float nx = Mathf.InverseLerp(worldBounds.xMin, worldBounds.xMax, wp.x);
                float ny = Mathf.InverseLerp(worldBounds.yMin, worldBounds.yMax, wp.z);
                Rect  r  = mapRect.rect;

                stopMarkers[i].anchoredPosition = new Vector2(
                    Mathf.Lerp(r.xMin, r.xMax, nx),
                    Mathf.Lerp(r.yMin, r.yMax, ny)
                );

                bool isNext    = waypointIdx == nextStopAbsoluteIndex;
                bool isVisited = waypointIdx <  routeManager.CurrentWaypointIndex;

                // Scale — only next stop pulses
                stopMarkers[i].localScale = isNext ? Vector3.one * pulseFactor : Vector3.one;

                // Colour tint via the marker's own Image component
                Image img = stopMarkers[i].GetComponent<Image>();
                if (img != null)
                {
                    img.color = isNext    ? nextStopColor
                              : isVisited ? visitedColor
                              :             pendingColor;
                }
            }
        }

        // ── Initialisation helpers ────────────────────────────────────────────

        /// <summary>
        /// Scans RouteManager.Waypoints once and stores indices of stop waypoints (IsStop == true).
        /// We cache a maximum of 5 because the UI provides exactly 5 markers.
        /// </summary>
        private void BuildStopIndex()
        {
            _stopIndices.Clear();

            if (routeManager == null || routeManager.Waypoints == null) return;

            foreach (var wp in routeManager.Waypoints)
            {
                // Safety: do not exceed the number of UI markers available
                if (_stopIndices.Count >= stopMarkers.Length) break;

                // Find the absolute index for later comparison with CurrentWaypointIndex
                int idx = routeManager.Waypoints.IndexOf(wp);
                if (wp.IsStop)
                    _stopIndices.Add(idx);
            }
        }

        /// <summary>
        /// Hides any stop markers that don't have a corresponding waypoint.
        /// </summary>
        private void InitialiseMarkers()
        {
            for (int i = 0; i < stopMarkers.Length; i++)
            {
                if (stopMarkers[i] != null)
                    stopMarkers[i].gameObject.SetActive(i < _stopIndices.Count);
            }
        }

        /// <summary>
        /// Returns the absolute waypoint index of the next upcoming stop,
        /// or -1 if no stop remains on the route.
        /// </summary>
        private int FindNextStopWaypointIndex()
        {
            if (routeManager == null || routeManager.Waypoints == null) return -1;

            for (int i = routeManager.CurrentWaypointIndex; i < routeManager.Waypoints.Count; i++)
            {
                if (routeManager.Waypoints[i].IsStop) return i;
            }
            return -1;
        }

        private void SetInteractable(bool value)
        {
            if (mapCanvasGroup == null) return;
            mapCanvasGroup.interactable   = value;
            mapCanvasGroup.blocksRaycasts = value;
        }
    }
}
