using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Stls
{
    public class Game : MonoBehaviour
    {
        public Level Level { get; private set; }

        private Coroutine _waitForEndOfTurnCoroutine;

        public event Action PlayerWon;
        public event Action PlayerLost;

        [Inject]
        public void Construct(Level level)
        {
            Level = level;
        }

        public async Task InitAsync(LevelData levelData)
        {
            await Level.CreateAsync(levelData);

            Level.Player.TurnEnded += OnPlayerTurnEnded;

            EnemiesSightChecker.CheckCellsVisibility(Level.EnemyGroup, Level.Grid);
        }

        private void OnDestroy()
        {
            if(Level.Player != null)
                Level.Player.TurnEnded -= OnPlayerTurnEnded;
        }


        private void PlayTurn()
        {
            if (IsPlayerWon())
            {
                PlayerWon?.Invoke();
                return;
            }

            EnemiesSightChecker.CheckTargetsVisibility(Level.Player, Level.EnemyGroup, Level.Grid);

            Level.EnemyGroup.PlayTurn();

            EnemiesSightChecker.CheckCellsVisibility(Level.EnemyGroup, Level.Grid);

            if (IsPlayerLost())
            {
                PlayerLost?.Invoke();
                return;
            }

            Level.Player.StartTurn();
        }


        private void WaitForEndOfTurn(Action action)
        {
            _waitForEndOfTurnCoroutine = StartCoroutine(WaitForEndOfTurnCoroutine(action));
        }

        private IEnumerator WaitForEndOfTurnCoroutine(Action action)
        {
            yield return new WaitForSeconds(Constants.Game.TurnTime);
            action.Invoke();
        }

        private void OnPlayerTurnEnded()
        {
            WaitForEndOfTurn(PlayTurn);
        }

        private bool IsPlayerLost() => !Level.Player.IsAlive;

        private bool IsPlayerWon() => Level.Player.Location.Position == Level.LevelData.ExitPosition;
    }
}