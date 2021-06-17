using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Stls
{
    public class Game : MonoBehaviour
    {
        private Level _level;

        private Coroutine _waitForEndOfTurnCoroutine;

        [Inject]
        public void Construct(Level level)
        {
            _level = level;
        }

        public async Task InitAsync(LevelData levelData)
        {
            await _level.CreateAsync(levelData);

            _level.Player.TurnEnded += OnPlayerTurnEnded;

            EnemiesSightChecker.CheckCellsVisibility(_level.EnemyGroup, _level.Grid);
        }

        private void OnDestroy()
        {
            if(_level.Player != null)
                _level.Player.TurnEnded -= OnPlayerTurnEnded;
        }


        private void PlayTurn()
        {
            // Player won.
            if (_level.Player.Location.Position == _level.LevelData.ExitPosition)
            {
                Debug.Log("GAME: WIN");
                return;
            }

            EnemiesSightChecker.CheckTargetsVisibility(_level.Player, _level.EnemyGroup, _level.Grid);

            // Move all enemies.
            _level.EnemyGroup.PlayTurn();


            EnemiesSightChecker.CheckCellsVisibility(_level.EnemyGroup, _level.Grid);

            // Player loose.
            if (!_level.Player.IsAlive)
            {
                Debug.Log("GAME: LOOSE.");
                return;
            }

            _level.Player.StartTurn();
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
    }
}