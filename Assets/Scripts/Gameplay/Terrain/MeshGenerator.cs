using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gameplay.Terrain
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, float cellSize)
        {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);
            var topLeft = new Vector3((width - 1) / -2f, 0f, (height - 1) / 2f) * cellSize;

            var meshData = new MeshData(width, height);
            var vertexIndex = 0;

            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                {
                    var vertexHeight = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
                    meshData.vertices[vertexIndex] = topLeft + new Vector3(x, vertexHeight, -y) * cellSize;
                    meshData.uvs[vertexIndex] = new Vector2(1f * x / width, 1f * y / height);

                    if (x < width - 1 && y < height - 1)
                    {
                        meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                        meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex += 1;
                }

            return meshData;
        }
    }

    public class MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] uvs;

        int triangleIndex = 0;

        public MeshData(int meshWidth, int meshHeight)
        {
            vertices = new Vector3[meshWidth * meshHeight];
            uvs = new Vector2[meshWidth * meshHeight];
            triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}