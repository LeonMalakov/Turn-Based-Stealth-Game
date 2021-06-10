namespace Stls
{
    public enum GridDirection
    {
        None = -1,
        ForwardRight = 0, 
        Right = 1, 
        BackRight = 2, 
        BackLeft = 3, 
        Left = 4, 
        ForwardLeft = 5
    }

    public static class GridDirectionExtensions
    {
        public static GridDirection Opposite(this GridDirection dir) =>
            dir > GridDirection.BackRight ? (dir - 3) : (dir + 3);


        /// <summary>
        /// Relative Direction, where from - direction to cell where attack from, dir - direction of attacked pawn.
        /// </summary>
        public static GridDirection Relative(this GridDirection dir, GridDirection from) =>
            (GridDirection)(from < dir ? ((int)from + 6 - (int)dir) : (from - dir));
    }
}