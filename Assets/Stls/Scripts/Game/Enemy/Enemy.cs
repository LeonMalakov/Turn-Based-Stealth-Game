using System;
using UnityEngine;

namespace Stls
{
    public class Enemy : Pawn
    {
        [SerializeField] private EnemyConfig _config;

        private EnemyBehaviour _behaviour;
        public Player KnownPlayer { get; private set; }
        public bool IsPlayerInAttackRange { get; private set; }

        public EnemyConfig Config
        {
            get => _config;
            set => _config = value;
        }

        public event Action Alarmed;

        public void Init(Cell location, GridDirection direction, Cell[] patrolWaypoints)
        {
            _behaviour = new EnemyBehaviour(this, patrolWaypoints);

            Init(location, direction);

            _behaviour.StartState(EnemyBehaviour.State.Idle);
        }

        public void PlayTurn()
        {
            if (KnownPlayer != null)
            {
                Alarmed?.Invoke();
            }

            _behaviour.PerformState();

            ResetTurnStates();
        }

        public void SeeHero(Player player, bool isInAttackRange)
        {
            KnownPlayer = player;
            IsPlayerInAttackRange = isInAttackRange;

            _behaviour.StartState(EnemyBehaviour.State.Follow);
        }

        public void SeeDeadBody(Enemy body)
        {
            Debug.Log($"Dead body founded: '{body.gameObject.name}'.");
        }

        private void ResetTurnStates()
        {
            IsPlayerInAttackRange = false;
        }

        protected override void OnDied()
        {
            _behaviour.StartState(EnemyBehaviour.State.Died);
        }
    }
}