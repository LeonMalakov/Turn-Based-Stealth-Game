namespace Stls
{
    public class EnemyBehaviour
    {
        public enum State
        {
            None = -1,
            Idle,
            Follow,
            Died
        }

        public Enemy Enemy { get; private set; }
        private EnemyIdleBehaviour _idle;
        private EnemyFollowBehaviour _follow;

        public State CurrentState { get; private set; } = State.None;


        public EnemyBehaviour(Enemy enemy, Cell[] patrolWaypoints)
        {
            Enemy = enemy;

            _idle = new EnemyIdleBehaviour(this, patrolWaypoints);
            _follow = new EnemyFollowBehaviour(this);
        }

        private IEnemyStateBehaviour CurrentBehaviour
        {
            get
            {
                switch (CurrentState)
                {
                    case State.Idle: return _idle;
                    case State.Follow: return _follow;

                    default: return null;
                }
            }
        }

        public void StartState(State state)
        {
            if (CurrentState == state)
                return;

            CurrentState = state;
            CurrentBehaviour?.Start();
        }

        public void PerformState()
        {
            CurrentBehaviour?.Perform();
        }
    }
}