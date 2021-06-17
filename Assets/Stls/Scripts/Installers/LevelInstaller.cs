using UnityEngine;
using Zenject;

namespace Stls
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private Level _level;
        [SerializeField] private Grid _grid;
        [SerializeField] private Game _game;

        public Game Game => _game;

        public override void InstallBindings()
        {
            BindCellFactory();
            BindPlayerFactory();
            BindEnemyFactory();

            BindInputHandler();
            BindLevel();
            BindGrid();
        }

        private void BindInputHandler()
        {
            Container
                .Bind<InputHandler>()
                .FromInstance(_inputHandler)
                .AsSingle();
        }

        private void BindLevel()
        {
            Container
                .Bind<Level>()
                .FromInstance(_level)
                .AsSingle();
        }

        private void BindGrid()
        {
            Container
                .Bind<Grid>()
                .FromInstance(_grid)
                .AsSingle();

            Container
                .Bind<GridClickHandler>()
                .FromInstance(_grid.GetComponent<GridClickHandler>())
                .AsSingle();
        }

        private void BindCellFactory()
        {
            Container
                .Bind<ICellFactory>()
                .To<CellFactory>()
                .AsSingle();
        }

        private void BindPlayerFactory()
        {
            Container
                .Bind<IPlayerFactory>()
                .To<PlayerFactory>()
                .AsSingle();
        }

        private void BindEnemyFactory()
        {
            Container
                .Bind<IEnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle();
        }
    }
}
