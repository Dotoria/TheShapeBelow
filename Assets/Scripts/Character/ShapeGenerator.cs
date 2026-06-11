using UnityEngine;

namespace Character
{
    public class ShapeGenerator
    {
        private const float MIN_SIZE = 0.2f;
        private const float MAX_SIZE = 0.5f;

        public static void GenerateShape(MeshFilter meshFilter)
        {
            if (null == meshFilter)
                return;
            
            Vector3 north = new Vector3(0, 0.5f, 0);
            Vector3 east = new Vector3(Random.Range(MIN_SIZE, MAX_SIZE), Random.Range(-MIN_SIZE, MIN_SIZE), 0);
            Vector3 south = new Vector3(0, Random.Range(-MAX_SIZE, -MIN_SIZE), 0);
            Vector3 west = new Vector3(Random.Range(-MAX_SIZE, -MIN_SIZE), Random.Range(-MIN_SIZE, MIN_SIZE), 0);

            Vector3[] vertices = new Vector3[] { north, east, south, west, north };
            int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

            Vector2[] uv = new Vector2[]
            {
                new Vector2(0.5f, 1f),
                new Vector2(1f, 0.5f),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(0f, 0.5f)
            };

            Mesh mesh = new Mesh();
            mesh.Clear();
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }
    }
}