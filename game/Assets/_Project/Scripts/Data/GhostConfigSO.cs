using UnityEngine;

namespace BusShift.Data
{
    [CreateAssetMenu(fileName = "GhostConfig", menuName = "BusShift/Ghost Config")]
    public class GhostConfigSO : ScriptableObject
    {
        public Ghosts.GhostType GhostType;
        public float BaseAttackInterval = 30f;
        public float AttackWindow = 3f;
        [Tooltip("Attack interval reduction per day (seconds)")]
        public float DifficultyScaling = 3f;
    }
}
