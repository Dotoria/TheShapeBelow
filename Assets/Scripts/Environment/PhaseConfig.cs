using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    [CreateAssetMenu(fileName = "PhaseConfig", menuName = "Configs/Phase Config")]
    public class PhaseConfig : ScriptableObject
    {
        [Serializable]
        public class PhaseData
        {
            public int phaseIndex;

            [Header("Enemies")]
            public List<EnemySpawnData> enemies;

            [Header("Friends")]
            public List<FriendSpawnData> friends;
        }
        [SerializeField] private List<PhaseData> _phases;

        public PhaseData GetPhase(int phaseIndex)
        {
            foreach (PhaseData phase in _phases)
            {
                if (phase.phaseIndex == phaseIndex)
                    return phase;
            }

            return null;
        }
    }
}