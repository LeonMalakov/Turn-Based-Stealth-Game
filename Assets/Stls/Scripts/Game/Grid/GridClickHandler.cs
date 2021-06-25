using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stls
{
    public class GridClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public event Action<GridCoordinates> Clicked;

        private void ClickAt(Vector3 pos)
        {
            GridCoordinates cellCoord = GridCoordinates.FromWorldPosition(pos);

            Clicked?.Invoke(cellCoord);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickAt(eventData.pointerCurrentRaycast.worldPosition);
        }
    }
}