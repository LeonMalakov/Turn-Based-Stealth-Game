using UnityEngine;

namespace Stls
{
    public static class GridVisibilityChecker
    {
        public static bool IsCellVisible(GridCoordinates viewer, GridDirection viewDirection, int viewRange, GridCoordinates target, Grid grid)
        {
            if (!IsCellInRange(viewer, viewRange, target))
                return false;

            if (!IsTargetInsideFieldOfView(viewer, viewDirection, viewRange, target))
                return false;

            if (IsTargetOverlapped(viewer, target, grid))
                return false;

            return true;
        }

        public static bool IsCellInRange(GridCoordinates viewer, int range, GridCoordinates target)
        {
            float rangeWorld = range * GridMetrics.InnerRadius * 2 + GridMetrics.RadiusError;
            if ((target - viewer).ToWorldPosition().sqrMagnitude > rangeWorld * rangeWorld)
                return false;

            return true;
        }

        private static bool IsTargetInsideFieldOfView(GridCoordinates viewer, GridDirection viewDirection, int viewRange, GridCoordinates target)
        {
            switch (viewDirection)
            {
                case GridDirection.ForwardRight:
                    if (target.Y <= viewer.Y && target.Z >= viewer.Z) return true;
                    break;

                case GridDirection.Right:
                    if (target.X >= viewer.X && target.Y <= viewer.Y) return true;
                    break;

                case GridDirection.BackRight:
                    if (target.X >= viewer.X && target.Z <= viewer.Z) return true;
                    break;


                case GridDirection.BackLeft:
                    if (target.Y >= viewer.Y && target.Z <= viewer.Z) return true;
                    break;

                case GridDirection.Left:
                    if (target.X <= viewer.X && target.Y >= viewer.Y) return true;
                    break;

                case GridDirection.ForwardLeft:
                    if (target.X <= viewer.X && target.Z >= viewer.Z) return true;
                    break;
            }

            return false;
        }

        private static bool IsTargetOverlapped(GridCoordinates viewer, GridCoordinates target, Grid _grid)
        {
            // World positions.
            Vector3 viewerPos = viewer.ToWorldPosition();
            Vector3 targetPos = target.ToWorldPosition();

            // Line from viewer to target coefficients.
            float a = targetPos.z - viewerPos.z;
            float b = viewerPos.x - targetPos.x;
            float c = (viewerPos.z * targetPos.x) - (targetPos.z * viewerPos.x);

            // Radius value for comparsion, it is not equals for radius itself.
            float compRadius = GridMetrics.OuterRadius * Mathf.Sqrt(a * a + b * b);

            // Viewer-Target area bounds.
            int fromX = Mathf.Min(viewer.X, target.X);
            int toX = Mathf.Max(viewer.X, target.X);
            int fromZ = Mathf.Min(viewer.Z, target.Z);
            int toZ = Mathf.Max(viewer.Z, target.Z);

            for (int x = fromX; x <= toX; x++)
                for (int z = fromZ; z <= toZ; z++)
                {
                    // Current iteration coordinates.
                    GridCoordinates currentCoords = new GridCoordinates(x, z);
                    Vector3 currentPos = currentCoords.ToWorldPosition();

                    // Comparsion values.
                    float compDist = Mathf.Abs(a * currentPos.x + b * currentPos.z + c);

                    // If dist less that radius, cell is on view line.
                    if (compDist < compRadius)
                    {
                        Cell cell = _grid.GetCell(currentCoords);

                        // If cell exists and not walkable, then target invisible.
                        if (cell != null && !cell.IsWalkable)
                        {
                            return true;
                        }
                    }
                }
            return false;
        }
    
    }
}