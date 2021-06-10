using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stls
{
    public class Cell : MonoBehaviour
    {
        public delegate void VisibilityStateChangedEventHandler(bool isVisible, bool isInAttackRange);

        private Cell[] _neighbors = new Cell[6];
        private CellPawnsCollection _pawns = new CellPawnsCollection();

        public GridCoordinates Position { get; private set; }
        public bool IsWalkable { get; private set; } = true;
        public bool IsVisible { get; private set; }
        public bool IsInAttackRange { get; private set; }

        public IReadOnlyList<Cell> Neighbors => _neighbors;
        public IReadOnlyCellPawnsCollection Pawns => _pawns;

        public event VisibilityStateChangedEventHandler VisibilityStateChanged;

        public void Init(GridCoordinates pos)
        {
            Position = pos;
            transform.position = pos.ToWorldPosition();
        }

        public Cell GetNeighbor(GridDirection dir) => _neighbors[(int)dir];

        public bool IsNeighbor(Cell to) => _neighbors.Contains(to);

        public void SetNeighbor(GridDirection dir, Cell cell)
        {
            _neighbors[(int)dir] = cell;
            cell._neighbors[(int)dir.Opposite()] = this;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
        }

        public void SetVisibilityState(bool isVisible, bool isInAttackRange)
        {
            if (IsVisible != isVisible || IsInAttackRange != isInAttackRange)
            {
                IsVisible = isVisible;
                IsInAttackRange = isInAttackRange;
                VisibilityStateChanged?.Invoke(isVisible, isInAttackRange);
            }
        }

        public void PawnEnter(Pawn pawn)
        {
            _pawns.AddPawn(pawn);
        }

        public void PawnExit(Pawn pawn)
        {
            _pawns.RemovePawn(pawn);
        }
    }
}