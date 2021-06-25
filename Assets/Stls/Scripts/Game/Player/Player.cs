using System;

namespace Stls
{
    public class Player : Pawn
    {
        public bool IsTurnStarted { get; private set; } = true;
        public event Action TurnEnded;

        public void StartTurn()
        {
            IsTurnStarted = true;
        }

        public void EndTurn()
        {
            IsTurnStarted = false;
            TurnEnded?.Invoke();
        }
    }
}