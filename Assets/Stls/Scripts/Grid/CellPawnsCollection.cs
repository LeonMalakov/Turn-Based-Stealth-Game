using System;
using System.Collections.Generic;
using System.Linq;

namespace Stls
{
    public interface IReadOnlyCellPawnsCollection
    {
        IReadOnlyCollection<Pawn> Pawns { get; }
        Pawn Alive { get; }
        bool HasAlive { get; }
    }

    public class CellPawnsCollection : IReadOnlyCellPawnsCollection
    {
        private HashSet<Pawn> _pawns = new HashSet<Pawn>();

        public IReadOnlyCollection<Pawn> Pawns => _pawns;
        public Pawn Alive => _pawns.FirstOrDefault(pawn => pawn.IsAlive);
        public bool HasAlive => Alive != null;

        public void AddPawn(Pawn pawn)
        {
            if (_pawns.Contains(pawn))
                return;

            if (pawn.IsAlive && Alive != null)
                throw new InvalidOperationException("Cell can contains only one alive pawn.");

            _pawns.Add(pawn);
        }

        public void RemovePawn(Pawn pawn)
        {
            if (_pawns.Contains(pawn))
            {
                _pawns.Remove(pawn);
            }
        }
    }
}