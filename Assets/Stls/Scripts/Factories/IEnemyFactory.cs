using System.Threading.Tasks;
using UnityEngine;

namespace Stls
{
    public interface IEnemyFactory
    {
        Enemy Create(Transform parent, Cell location, GridDirection direction, Cell[] patrolWaypoints);
        Task LoadAsync();
        void Reclaim(Enemy enemy);
    }
}