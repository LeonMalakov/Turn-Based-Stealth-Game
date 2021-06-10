using System.Threading.Tasks;
using UnityEngine;

namespace Stls
{
    public interface IPlayerFactory
    {
        Player Create(Transform parent, Cell location, GridDirection direction);
        Task LoadAsync();
        void Reclaim(Player player);
    }
}