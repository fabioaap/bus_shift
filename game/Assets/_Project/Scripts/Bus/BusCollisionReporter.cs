using UnityEngine;

namespace BusShift.Bus
{
    /// <summary>
    /// Companion MonoBehaviour placed on the bus Rigidbody root.
    /// Forwards Unity physics collision callbacks to C# events so that
    /// other systems (e.g. GhostIntegrationBridge → GraceGhost) can react
    /// without modifying BusController.
    /// </summary>
    public class BusCollisionReporter : MonoBehaviour
    {
        /// <summary>
        /// Fired every time the bus's Rigidbody enters a new physics collision.
        /// </summary>
        public event System.Action OnCollision;

        private void OnCollisionEnter(Collision collision)
        {
            OnCollision?.Invoke();
        }
    }
}
