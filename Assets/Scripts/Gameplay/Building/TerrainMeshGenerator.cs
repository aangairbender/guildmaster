using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Building
{

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class TerrainMeshGenerator : MonoBehaviour
    {
        [SerializeField] public int Width = 100;
        [SerializeField] public int Height = 100;

        [SerializeField] float CellSize = 1.0f;
        [SerializeField] float CellHeight = 0.1f;

        private Tile[,] tiles;

        // Start is called before the first frame update
        void Start()
        {
            tiles = new Tile[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tiles[x, y] = new Tile();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            bool rebuildRequired = false;

            if (UnityEngine.Input.GetMouseButtonDown(2))
            {
                rebuildRequired = true;
            }

            if (rebuildRequired)
            {
                RebuildTerrain();
            }
        }

        private void RebuildTerrain()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!tiles[x, y].Road)
                    {
                        BuildGrassTile(x, y, vertices, triangles);
                    }
                }
            }

            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.Optimize();
            mesh.RecalculateNormals();
        }

        private void BuildGrassTile(int x, int y, List<Vector3> vertices, List<int> triangles)
        {
            Vector3[] v = {
                new Vector3 (0, 0, 0),
                new Vector3 (1, 0, 0),
                new Vector3 (1, 1, 0),
                new Vector3 (0, 1, 0),
                new Vector3 (0, 1, 1),
                new Vector3 (1, 1, 1),
                new Vector3 (1, 0, 1),
                new Vector3 (0, 0, 1),
            };

            int[] t = {
                0, 2, 1, //face front
			    0, 3, 2,
                2, 3, 4, //face top
			    2, 4, 5,
                1, 2, 5, //face right
			    1, 5, 6,
                0, 7, 4, //face left
			    0, 4, 3,
                5, 4, 7, //face back
			    5, 7, 6,
                0, 6, 7, //face bottom
			    0, 1, 6
            };

            vertices.AddRange(v.Select(w => UnitToTile(w) + new Vector3((-Width * 0.5f + x - 0.5f) * CellSize, 0.0f, (-Height * 0.5f + y - 0.5f) * CellSize)));
            triangles.AddRange(t);
        }

        private Vector3 UnitToTile(Vector3 v)
        {
            return new Vector3(v.x * CellSize, v.y * CellHeight, v.z * CellSize);
        }
    }
}
