using UnityEngine;
using Zenject;

namespace Stls
{
    public class PlayerController : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private Level _level;

        [Inject]
        public void Construct(Level level, InputHandler inputHandler)
        {
            _level = level;
            _inputHandler = inputHandler;
        }

        private void OnEnable()
        {
            _inputHandler.GridClicked += OnGridClicked;
        }

        private void OnDisable()
        {
            _inputHandler.GridClicked -= OnGridClicked;
        }

        private void OnGridClicked(GridCoordinates coords)
        {
            if (!_level.Player.IsTurnStarted)
                return;

            Cell selectedCell = _level.Grid.GetCell(coords);

            if (selectedCell == null)
                return;

            if (selectedCell == _level.Player.Location)
            {
                GrabDropBody(selectedCell);
            }
            else
            {
                MoveOrTakedown(selectedCell);
            }
        }

        private void GrabDropBody(Cell cell)
        {
            /*
            if (_level.Player.CanGrabBody())
            {
                _level.Player.GrabBody();
            }
            else if (_level.Player.GrabbedBody != null)
            {
                _level.Player.DropBody();
            }
            */
        }

        private void MoveOrTakedown(Cell cell)
        {
            GridDirection moveDirection = GridCoordinates.GetDirectionFromCoordinates(
              cell.Position - _level.Player.Location.Position);

            if ((int)moveDirection == -1)
                return;


            if (_level.Player.CanTakedown(cell))
            {
                _level.Player.TakedownMove(cell, moveDirection);
                _level.Player.EndTurn();
            }
            else if (_level.Player.CanMove(cell))
            {
                _level.Player.Move(cell, moveDirection);
                _level.Player.EndTurn();
            }
        }
    }
}