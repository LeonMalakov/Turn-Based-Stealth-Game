using UnityEngine;

namespace Stls
{
    [System.Serializable]
    public class EnemyConfig
    {
        [SerializeField] private int _viewRange;
        [SerializeField] private int _attackRange;

        public int ViewRange
        {
            get => _viewRange;
            set => _viewRange = value;
        }
        public int AttackRange
        {
            get => _attackRange;
            set => _attackRange = value;
        }
    }
}
