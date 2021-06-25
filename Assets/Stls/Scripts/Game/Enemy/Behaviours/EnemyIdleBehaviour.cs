using System.Linq;

namespace Stls
{
    public class EnemyIdleBehaviour : IEnemyStateBehaviour
    {
        private EnemyBehaviour _behaviour;
        private Cell[] _patrolWaypoints;
        private int _currentPatrolWaypointIndex;
        private Cell[] _path;
        private int _currentPathIndex;

        public EnemyIdleBehaviour(EnemyBehaviour behaviour, Cell[] patrolWaypoints)
        {
            _behaviour = behaviour;
            _patrolWaypoints = patrolWaypoints;
        }

        public void Start()
        {
            if (_patrolWaypoints != null && _patrolWaypoints.Length >= 2)
                FindNextWaypointPath(_behaviour.Enemy.Location);
        }

        public void Perform()
        {
            if (_patrolWaypoints.Length >= 2)
            {
                MoveNextIdleState();
            }
        }

        private void FindNextWaypointPath(Cell from)
        {
            if (_currentPatrolWaypointIndex >= _patrolWaypoints.Length - 1)
                _currentPatrolWaypointIndex = -1;

            GridPathfinder.CalculatePath(from, _patrolWaypoints[++_currentPatrolWaypointIndex], out _path);
            _currentPathIndex = 0;
        }

        private void MoveNextIdleState()
        {
            Cell next = _path[_currentPathIndex + 1];

            if (next.Pawns.HasAlive == false)
            {
                if (_behaviour.Enemy.CanMove(next))
                {
                    if (_currentPathIndex < _path.Length - 2)
                        _currentPathIndex++;
                    else
                        FindNextWaypointPath(next);

                    _behaviour.Enemy.Move(next, GridCoordinates.GetDirectionFromCoordinates(_path[_currentPathIndex + 1].Position - next.Position));
                }
            }
        }
    }
}