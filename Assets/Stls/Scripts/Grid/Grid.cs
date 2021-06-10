using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Stls
{
    public class Grid : MonoBehaviour
    {
        private ICellFactory _cellFactory;
        private int _width;
        private int _height;
        private Cell[] _cells;

        public IReadOnlyList<Cell> Cells => _cells;

        [Inject]
        public void Construct(ICellFactory cellFactory)
        {
            _cellFactory = cellFactory;
        }

        public async Task CreateAsync(int width, int height)
        {
            _width = width;
            _height = height;

            await CreateGridAsync();          
        }

        public Cell GetCell(GridCoordinates coords)
        {
            int index = coords.X + coords.Z * _width + coords.Z / 2;

            if(index >= 0 && index < _cells.Length && _cells[index].Position == coords)
                return _cells[index];
            else 
                return null;
        }


        private async Task CreateGridAsync()
        {
            await _cellFactory.LoadAsync();

            _cells = new Cell[_width * _height];

            for (int z = 0; z < _height; z++)
                for (int x = 0; x < _width; x++)
                {
                    CreateCell(x, z);                
                }
        }

        private void CreateCell(int x, int z)
        {
            int currentIndex = x + z * _width;

            // Setup cell.
            Cell cell = _cells[currentIndex] = _cellFactory.Create(transform, GridCoordinates.FromOffset(x, z));

            // Setting neighbors.
            if (x > 0)
                cell.SetNeighbor(GridDirection.Left, _cells[currentIndex - 1]);

            if (z % 2 != 0)
            {
                if (z > 0)
                    cell.SetNeighbor(GridDirection.BackLeft, _cells[currentIndex - _width]);

                if (z > 0 && x < _width - 1)
                    cell.SetNeighbor(GridDirection.BackRight, _cells[currentIndex - _width + 1]);
            }
            else
            {
                if (z > 0 && x > 0)
                    cell.SetNeighbor(GridDirection.BackLeft, _cells[currentIndex - _width - 1]);

                if (z > 0)
                    cell.SetNeighbor(GridDirection.BackRight, _cells[currentIndex - _width]);
            }
        }
    }
}