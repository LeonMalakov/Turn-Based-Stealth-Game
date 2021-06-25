using System.Collections.Generic;
using UnityEngine;

namespace Stls
{
    public static class GridPathfinder
    {
        private class Node
        {
            public Cell Cell;
            public Node Parent;
            public float CostFromStart;
            public float SqrDistToEnd;
            public float TotalCost => CostFromStart + SqrDistToEnd;
        }

        /// <summary>
        /// Calculates path from one cell to another.<br />
        /// Returns TRUE, if path was founded.<br />
        /// Returns FALSE, if path wasn't founded.
        /// </summary>
        public static bool CalculatePath(Cell from, Cell to, out Cell[] path)
        {
            // A* algorithm.
            
            // Closed - list of alredy processed cells.
            // Open - list of cells that can be proccesed in next iteration.
            List<Cell> closed = new List<Cell>();
            List<Node> open = new List<Node>();

            // Create start node, add to open list.
            open.Add(new Node()
            {
                Cell = from,
                CostFromStart = 0,
                SqrDistToEnd = GetSqrDistance(from.Position, to.Position)
            });

            while (open.Count > 0)
            {
                // Find node with minimum cost at open.
                Node minCostNode = open[0];
                open.ForEach((n) =>
                {
                    if (n.TotalCost < minCostNode.TotalCost)
                        minCostNode = n;
                });

                open.Remove(minCostNode);
                closed.Add(minCostNode.Cell);

                // If target reached.
                if (minCostNode.Cell == to) 
                {
                    // Path founded!
                    path = BuildPath(minCostNode);
                    return true; 
                }

                // For each neighbor cell.
                foreach(Cell cell in minCostNode.Cell.Neighbors)
                {

                    // If neigbor not exists, skip.
                    if (cell == null) continue;

                    // If cell in closed list, skip.
                    if (closed.Contains(cell)) continue;


                    // If cell is notWalkable, add to closed list, skip.
                    if (!cell.IsWalkable)
                    {
                        closed.Add(cell);
                        continue;
                    }

                    // Calculate this cell cost from start.
                    // 1 - this cell cost.
                    float costFromStart = minCostNode.CostFromStart + 1;

                    // Find this cell node in open list.
                    Node currentNode = open.Find((n) => n.Cell == cell);

                    // If cell is not in open, add.
                    if (currentNode == null)
                        open.Add(new Node()
                        {
                            Cell = cell,
                            CostFromStart = costFromStart,
                            SqrDistToEnd = GetSqrDistance(cell.Position, to.Position),
                            Parent = minCostNode,
                        });
                    // Else if new cost < old cost, update it.
                    else if (costFromStart < currentNode.CostFromStart)
                    {
                        currentNode.CostFromStart = costFromStart;
                        currentNode.Parent = minCostNode;
                    }
                }
            }

            // Path not founded.
            path = null;
            return false;
        }

        // Builds GridDirection array from start cell to end.
        private static Cell[] BuildPath(Node endNode)
        {
            List<Cell> path = new List<Cell>();

            Node node = endNode;
            path.Add(node.Cell);
            while (node.Parent != null)
            {
                path.Add(node.Parent.Cell);
                node = node.Parent;
            }

            path.Reverse();
            return path.ToArray();
        }

        private static float GetSqrDistance(GridCoordinates l, GridCoordinates r)
        {
            Vector3 delta = r.ToWorldPosition() - l.ToWorldPosition();

            return delta.x * delta.x + delta.z * delta.z;
        }
    }
}