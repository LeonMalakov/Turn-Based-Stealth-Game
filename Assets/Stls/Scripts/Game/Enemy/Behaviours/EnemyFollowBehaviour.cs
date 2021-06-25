namespace Stls
{
    public class EnemyFollowBehaviour : IEnemyStateBehaviour
    {
        private EnemyBehaviour _behaviour;
        private Cell[] _path;
        private int _currentPathIndex;

        public EnemyFollowBehaviour(EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Start()
        {
        }

        public void Perform()
        {
            if (_behaviour.Enemy.IsPlayerInAttackRange)
            {
                _behaviour.Enemy.Attack(_behaviour.Enemy.KnownPlayer.Location);
            }
            else
            {
                GridPathfinder.CalculatePath(_behaviour.Enemy.Location, _behaviour.Enemy.KnownPlayer.Location, out _path);
                _currentPathIndex = 0;

                MoveNextFollowState();
            }
        }

        private void MoveNextFollowState()
        {
            Cell next = _path[_currentPathIndex + 1];

            if (next.Pawns.HasAlive == false
                && _behaviour.Enemy.CanMove(next)
                && _currentPathIndex < _path.Length - 2)
            {
                _currentPathIndex++;
                _behaviour.Enemy.Move(next, GridCoordinates.GetDirectionFromCoordinates(_path[_currentPathIndex + 1].Position - next.Position));
            }
        }
    }
}