using System.Threading.Tasks;
using UnityEngine;

namespace Stls
{
    public interface ICellFactory
    {
        Cell Create(Transform parent, GridCoordinates coords);
        Task LoadAsync();
        void Reclaim(Cell cell);
    }
}