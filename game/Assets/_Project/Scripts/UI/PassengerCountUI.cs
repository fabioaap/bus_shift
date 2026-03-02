using UnityEngine;
using TMPro;
using BusShift.Bus;

namespace BusShift.UI
{
    // ─────────────────────────────────────────────────────────────────────────
    //  PassengerCountUI
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Displays the live passenger count as a <see cref="TextMeshProUGUI"/> label
    /// in the format <c>"Passageiros: X/Y"</c>, where <b>X</b> is the number of
    /// living children currently on board and <b>Y</b> is the total expected for
    /// this route.
    ///
    /// <para>
    /// <b>Colour states:</b>
    /// <list type="bullet">
    ///   <item><b>Green</b> — all passengers have been safely delivered
    ///         (<see cref="PassengerManager.OnAllPassengersDelivered"/> fired).</item>
    ///   <item><b>Yellow</b> — route in progress; at least one passenger is on
    ///         board or yet to board.</item>
    ///   <item><b>Red</b> — no passenger is on board, delivery is not yet
    ///         complete, and at least one passenger was expected — indicates a
    ///         possible missing child.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Subscribes to <see cref="PassengerManager"/> events via
    /// <see cref="OnEnable"/> / <see cref="OnDisable"/>; no direct reference
    /// to <see cref="PassengerManager"/> is required in the Inspector.
    /// </para>
    /// </summary>
    public class PassengerCountUI : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — references
        // ─────────────────────────────────────────────────────────────────────

        [Header("Label")]
        [Tooltip("TextMeshProUGUI component that will display the passenger count.")]
        [SerializeField] private TextMeshProUGUI _passengerCountText;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — colours
        // ─────────────────────────────────────────────────────────────────────

        [Header("Status Colours")]
        [Tooltip("Label colour when all passengers have been safely delivered.")]
        [SerializeField] private Color _colorAllDelivered = new Color(0.18f, 0.80f, 0.44f, 1f); // emerald green

        [Tooltip("Label colour while the route is in progress.")]
        [SerializeField] private Color _colorInRoute = new Color(1.00f, 0.80f, 0.00f, 1f);      // amber yellow

        [Tooltip("Label colour when passengers are expected but none are on board (possible missing child).")]
        [SerializeField] private Color _colorMissing = new Color(0.90f, 0.20f, 0.20f, 1f);      // danger red

        // ─────────────────────────────────────────────────────────────────────
        //  Private state
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Current count of live passengers on board (not yet alighted).</summary>
        private int _onBoard;

        /// <summary>Total live passengers expected for this route.</summary>
        private int _totalExpected;

        /// <summary>Whether all passengers have been delivered this route.</summary>
        private bool _allDelivered;

        // ─────────────────────────────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void OnEnable()
        {
            PassengerManager.OnRouteInitialized       += HandleRouteInitialized;
            PassengerManager.OnPassengerBoarded       += HandleBoarded;
            PassengerManager.OnPassengerAlighted      += HandleAlighted;
            PassengerManager.OnAllPassengersDelivered += HandleAllDelivered;
        }

        private void OnDisable()
        {
            PassengerManager.OnRouteInitialized       -= HandleRouteInitialized;
            PassengerManager.OnPassengerBoarded       -= HandleBoarded;
            PassengerManager.OnPassengerAlighted      -= HandleAlighted;
            PassengerManager.OnAllPassengersDelivered -= HandleAllDelivered;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Event handlers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Resets counters when a new route manifest is loaded.
        /// </summary>
        /// <param name="totalLive">Total number of live passengers for the route.</param>
        private void HandleRouteInitialized(int totalLive)
        {
            _totalExpected = totalLive;
            _onBoard       = 0;
            _allDelivered  = false;
            Refresh();
        }

        /// <summary>
        /// Increments the on-board counter when a live passenger boards.
        /// </summary>
        /// <param name="passenger">The passenger who boarded.</param>
        private void HandleBoarded(PassengerData passenger)
        {
            // Ghost passengers do not count toward the live display.
            if (!passenger.IsGhost) _onBoard++;
            Refresh();
        }

        /// <summary>
        /// Decrements the on-board counter when a live passenger alights.
        /// </summary>
        /// <param name="passenger">The passenger who alighted.</param>
        private void HandleAlighted(PassengerData passenger)
        {
            if (!passenger.IsGhost)
                _onBoard = Mathf.Max(0, _onBoard - 1);

            Refresh();
        }

        /// <summary>
        /// Sets the delivered flag and refreshes the label colour to green.
        /// </summary>
        private void HandleAllDelivered()
        {
            _allDelivered = true;
            Refresh();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Updates the label text and colour to reflect the current state.
        /// </summary>
        private void Refresh()
        {
            if (_passengerCountText == null) return;

            _passengerCountText.text = $"Passageiros: {_onBoard}/{_totalExpected}";

            if (_allDelivered)
            {
                // All children safely delivered.
                _passengerCountText.color = _colorAllDelivered;
            }
            else if (_totalExpected > 0 && _onBoard == 0)
            {
                // Passengers are expected but none are on board and route is not
                // complete — possible missing child, warn the driver.
                _passengerCountText.color = _colorMissing;
            }
            else
            {
                // Normal route-in-progress state.
                _passengerCountText.color = _colorInRoute;
            }
        }
    }
}
