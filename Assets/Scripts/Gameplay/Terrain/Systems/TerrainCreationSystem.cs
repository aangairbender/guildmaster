using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using TerrainData = Gameplay.Terrain.Components.TerrainData;

namespace Gameplay.Terrain.Systems
{
    [BurstCompile]
    public partial struct TerrainCreationSystem : ISystem
    {
        // [BurstCompile]
        void OnCreate(ref SystemState state)
        {
            int width = 100;
            int height = 100;
            var heightMap = new float[width, height];
            var terrainData = new TerrainData(width, height, heightMap);

            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.SetName(entity, "ground");
            state.EntityManager.AddComponentObject(entity, terrainData);

            var desc = new RenderMeshDescription(
                shadowCastingMode: ShadowCastingMode.Off,
                receiveShadows: true,
                renderingLayerMask: 1);

            var material = Resources.Load<Material>("Ground");
            var mesh = CreateCube();// MeshGenerator.GenerateTerrainMesh(heightMap, 1.0f, AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f), 1.0f).CreateMesh();

            var renderMeshArray = new RenderMeshArray(new Material[] { material }, new Mesh[] { mesh });

            //state.EntityManager.AddComponent<Static>(entity);
            RenderMeshUtility.AddComponents(
                entity,
                state.EntityManager,
                desc,
                renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
        }

        private Mesh CreateCube()
        {
            Vector3[] vertices = {
                new Vector3 (0, 0, 0),
                new Vector3 (1, 0, 0),
                new Vector3 (1, 1, 0),
                new Vector3 (0, 1, 0),
                new Vector3 (0, 1, 1),
                new Vector3 (1, 1, 1),
                new Vector3 (1, 0, 1),
                new Vector3 (0, 0, 1),
            };

                int[] triangles = {
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

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.Optimize();
            mesh.RecalculateNormals();
            return mesh;
        }

        void OnUpdate(ref SystemState state) {}

        void OnDestroy(ref SystemState state) { }
    }
}
