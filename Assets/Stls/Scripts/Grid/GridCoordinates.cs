using System;
using UnityEngine;

namespace Stls
{
    [System.Serializable]
    public struct GridCoordinates
    {
        [SerializeField] private int _x;
        [SerializeField] private int _z;

        public int X => _x;
        public int Z => _z;
        public int Y => -X - Z;


        #region Constructors
        public GridCoordinates(int x, int z)
        {
            _x = x;
            _z = z;
        }

        public static GridCoordinates FromOffset(int x, int z) => new GridCoordinates(x - z / 2, z);

        public static GridCoordinates FromWorldPosition(Vector3 position)
        {
            float x = position.x / (GridMetrics.InnerRadius * 2f);
            float y = -x;

            float offset = position.z / (GridMetrics.OuterRadius * 3f);
            x -= offset;
            y -= offset;

            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x - y);

            if (iX + iY + iZ != 0)
            {
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x - y - iZ);

                if (dX > dY && dX > dZ)
                {
                    iX = -iY - iZ;
                }
                else if (dZ > dY)
                {
                    iZ = -iX - iY;
                }
            }

            return new GridCoordinates(iX, iZ);
        }
        #endregion

        public Vector3 ToWorldPosition()
        {
            return new Vector3(
                (X + Z * 0.5f) * (GridMetrics.InnerRadius * 2f),
                0,
                Z * (GridMetrics.OuterRadius * 1.5f)
                );
        }


        #region GridDirections
        public static GridCoordinates ForwardRight => new GridCoordinates(0, 1);
        public static GridCoordinates Right => new GridCoordinates(1, 0);
        public static GridCoordinates BackRight => new GridCoordinates(1, -1);
        public static GridCoordinates BackLeft => -ForwardRight;
        public static GridCoordinates Left => -Right;
        public static GridCoordinates ForwardLeft => -BackRight;

        private static readonly GridCoordinates[] directions = new GridCoordinates[] {
            ForwardRight, Right, BackRight, BackLeft, Left, ForwardLeft
        };

        public static GridCoordinates GetCoordinatesFromDirection(GridDirection dir) => directions[(int)dir];

        public static GridDirection GetDirectionFromCoordinates(GridCoordinates coords) => (GridDirection)Array.IndexOf(directions, coords);
        #endregion


        #region Operators
        public static bool operator==(GridCoordinates l, GridCoordinates r) => (l.X == r.X) && (l.Z == r.Z);

        public static bool operator !=(GridCoordinates l, GridCoordinates r) => (l.X != r.X) || (l.Z != r.Z);

        public static GridCoordinates operator -(GridCoordinates l) => new GridCoordinates(-l.X, -l.Z);

        public static GridCoordinates operator +(GridCoordinates l, GridCoordinates r) => 
            new GridCoordinates(l.X + r.X, l.Z + r.Z);

        public static GridCoordinates operator -(GridCoordinates l, GridCoordinates r) => l + (-r);
        #endregion


        #region Overrides
        public override string ToString() => $"({X}, {Y}, {Z})";
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}