using System;
using UnityEngine;

namespace Stls
{
    public class Pawn : MonoBehaviour
    {
        public delegate void DiedEventHandler(GridDirection attackedFrom);
        public delegate void AttackingEventHandler(Cell targetLocation);
        public delegate void TakedownMovingEventHandler(Cell to, GridDirection newDirection, GridDirection takedownDirection);
        public delegate void MovingEventHandler(Cell to, GridDirection newDirection);
        public delegate void InstantMovingEventHandler(Cell to, GridDirection newDirection);

        private Cell _location;

        public Cell Location
        {
            get => _location;
            protected set
            {
                if (_location != null)
                {
                    _location.PawnExit(this);
                }

                _location = value;

                if (_location != null)
                    _location.PawnEnter(this);
            }
        }

        public GridDirection Direction { get; protected set; }
        public bool IsAlive { get; private set; } = true;
        public Pawn GrabbedPawn { get; private set; }

        public event DiedEventHandler Died;
        public event AttackingEventHandler Attacking;
        public event TakedownMovingEventHandler TakedownMoving;
        public event MovingEventHandler Moving;
        public event InstantMovingEventHandler InstantMoving;

        public void Init(Cell location, GridDirection direction)
        {
            MoveInstant(location, direction);
        }

        public void Die(GridDirection attackedFrom)
        {
            IsAlive = false;
            OnDied();
            Died?.Invoke(attackedFrom);
        }

        public void Attack(Cell targetLocation)
        {
            targetLocation.Pawns.Alive.Die(GridDirection.None);
            Attacking?.Invoke(targetLocation);
        }

        public bool CanMove(Cell to) => CanMoveInternal(to);

        public bool CanMoveInstant(Cell to) => CanMoveInternal(to, ignoreLocation: true);

        public bool CanTakedown(Cell to) => to.Pawns.HasAlive
                                              && CanMoveInternal(to, ignoreLocation: false, ignorePawns: true);

        public void TakedownMove(Cell to, GridDirection direction)
        {
            if (!CanTakedown(to))
                throw new InvalidOperationException();

            Pawn target = to.Pawns.Alive;
            var moveDir = GridCoordinates.GetDirectionFromCoordinates(Location.Position - to.Position);
            var takedownDirection = target.Direction.Relative(moveDir);
            target.Die(takedownDirection);

            TakedownMoving?.Invoke(to, direction, takedownDirection);
            
            Location = to;
            Direction = direction;
        }

        public void Move(Cell to, GridDirection direction)
        {
            if (!CanMove(to))
                throw new InvalidOperationException();

            Moving?.Invoke(to, direction);

            Location = to;
            Direction = direction;
        }

        public void MoveInstant(Cell to, GridDirection direction)
        {
            if (!CanMoveInstant(to))
                throw new InvalidOperationException();

            InstantMoving?.Invoke(to, direction);
         
            Location = to;
            Direction = direction;
        }

        private bool CanMoveInternal(Cell to, bool ignoreLocation = false, bool ignorePawns = false)
        {
            if (to == null)
                throw new NullReferenceException();

            if (!to.IsWalkable)
                return false;

            if (!ignoreLocation)
            {
                if (!Location.IsNeighbor(to: to))
                    return false;
            }

            if (!ignorePawns)
            {
                if (to.Pawns.HasAlive)
                    return false;
            }

            return true;
        }

        protected virtual void OnDied() { }
    }
}