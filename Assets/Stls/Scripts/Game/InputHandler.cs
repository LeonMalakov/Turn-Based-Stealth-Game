using System;
using UnityEngine;
using Zenject;

namespace Stls
{
    public class InputHandler : MonoBehaviour
    {
        private GridClickHandler _gridClickHandler;
        public event Action<GridCoordinates> GridClicked;

        [Inject]
        public void Construct(GridClickHandler gridClickHandler)
        {
            _gridClickHandler = gridClickHandler;
        }

        private void OnEnable()
        {
            _gridClickHandler.Clicked += OnGridClicked;
        }

        private void OnDisable()
        {
            _gridClickHandler.Clicked -= OnGridClicked;
        }

        private void OnGridClicked(GridCoordinates coords)
        {
            GridClicked?.Invoke(coords);
        }
    }
}