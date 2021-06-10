using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stls
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GridPresenter : MonoBehaviour
    {
        private Mesh _mesh;

        public void Triangulate(IEnumerable<Cell> cells)
        {
            if (_mesh == null)
                CreateMesh();

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            foreach (Cell c in cells)
            {
                Triangulate(c, vertices, triangles);
            }

            _mesh.vertices = vertices.ToArray();
            _mesh.triangles = triangles.ToArray();
            _mesh.RecalculateNormals();
        }


        private void Triangulate(Cell cell, List<Vector3> vertices, List<int> triangles)
        {
            Vector3 center = cell.transform.localPosition;

            AddTriangle(
                center + GridMetrics.Corners[0],
                center + GridMetrics.Corners[1],
                center + GridMetrics.Corners[2],
                vertices, triangles);

            AddTriangle(
                center + GridMetrics.Corners[5],
                center + GridMetrics.Corners[0],
                center + GridMetrics.Corners[2],
                vertices, triangles);

            AddTriangle(
                center + GridMetrics.Corners[5],
                center + GridMetrics.Corners[2],
                center + GridMetrics.Corners[3],
                vertices, triangles);

            AddTriangle(
                center + GridMetrics.Corners[5],
                center + GridMetrics.Corners[3],
                center + GridMetrics.Corners[4],
                vertices, triangles);
        }

        private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, List<Vector3> vertices, List<int> triangles)
        {
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
        }

        private void CreateMesh()
        {
            GetComponent<MeshFilter>().sharedMesh = _mesh = new Mesh();
            _mesh.name = "HEX";
        }
    }
}