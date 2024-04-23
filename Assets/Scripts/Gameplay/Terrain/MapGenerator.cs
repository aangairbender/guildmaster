using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Terrain
{
    public class MapGenerator : MonoBehaviour
    {
        public enum DrawMode { NoiseMap, ColorMap, Mesh };
        const float MinScale = 0.0001f;
        const int mapChankSize = 241;

        public DrawMode drawMode;
        public int mapWidth;
        public int mapHeight;
        public float noiseScale;
        [Min(1f)]
        public float cellSize;

        public int octaves;
        [Range(0, 1)]
        public float persistance;
        public float lacunarity;

        public int seed;
        public Vector2 offset;

        public bool useFalloff;

        public float meshHeightMultiplier;
        public AnimationCurve meshHeightCurve;

        public bool autoUpdate;

        public TerrainType[] regions;

        float[,] falloffMap;

        public LayerMask GroundMask;
        public GameObject[] trees;
        public GameObject[] rocks;
        public GameObject detailsContainer;
        public bool generateDetails;
        public float treeLine;

        private void Awake()
        {
            falloffMap = FalloffGenerator.GeneratorFalloffMap(mapWidth, mapHeight);
        }

        private void Start()
        {
            GenerateMap(true);
        }

        public void GenerateMap(bool generateTrees = false)
        {
            var noiseMap = Noise.GenerateNoiseMap(
                mapWidth,
                mapHeight,
                seed,
                noiseScale,
                octaves,
                persistance,
                lacunarity,
                offset
            );

            if (useFalloff)
            {
                for (int y = 0; y < mapHeight; ++y)
                {
                    for (int x = 0; x < mapWidth; ++x)
                    {
                        noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                    }
                }
            }

            var colorMap = new Color[mapWidth * mapHeight];
            for (int x = 0; x < mapWidth; ++x)
                for (int y = 0; y < mapHeight; ++y)
                {
                    var currentHeight = noiseMap[x, y];
                    colorMap[y * mapWidth + x] = regions.First(r => currentHeight <= r.height).color;
                }

            var display = FindObjectOfType<MapDisplay>();
            if (drawMode == DrawMode.NoiseMap)
            {
                var texture = TextureGenerator.TextureFromHeightMap(noiseMap);
                display.DrawTexture(texture);
            }
            else if (drawMode == DrawMode.ColorMap)
            {
                var texture = TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight);
                display.DrawTexture(texture);
            }
            else if (drawMode == DrawMode.Mesh)
            {
                var mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, cellSize);
                var texture = TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight);
                display.DrawMesh(mesh, texture);
                if (generateDetails)
                {
                    ClearDetails();
                    GenerateTrees();
                    GenerateRocks();
                }
            }
        }

        private void ClearDetails()
        {
            var childrenCount = detailsContainer.transform.childCount;
            for (int i = childrenCount - 1; i >= 0; --i)
            {
                DestroyImmediate(detailsContainer.transform.GetChild(i).gameObject);
            }
        }

        private void GenerateTrees()
        {
            var center = new Vector2(mapWidth, mapHeight) * cellSize * 0.5f;
            var points = MySampling.GeneratePoints(10, new Vector2(mapWidth, mapHeight) * cellSize);
            foreach (var point2 in points)
            {
                var point = point2 - center;
                var ray = new Ray(new Vector3(point.x, 1000f, point.y), -Vector3.up);
                if (Physics.Raycast(ray, out var hit, 2000f, GroundMask))
                {
                    if (hit.point.y < treeLine) continue;
                    var angle = Random.Range(0, 360);
                    var treeIndex = Random.Range(0, trees.Length);
                    var tree = Instantiate(trees[treeIndex], hit.point, Quaternion.AngleAxis(angle, Vector3.up));
                    tree.transform.SetParent(detailsContainer.transform);
                }
            }
        }

        private void GenerateRocks()
        {
            var offset = new Vector2(cellSize, cellSize) * 0.5f;
            var center = new Vector2(mapWidth, mapHeight) * cellSize * 0.5f;
            var points = MySampling.GeneratePoints(20, new Vector2(mapWidth, mapHeight) * cellSize);
            foreach (var point2 in points)
            {
                var point = offset + point2 - center;
                var ray = new Ray(new Vector3(point.x, 1000f, point.y), -Vector3.up);
                if (Physics.Raycast(ray, out var hit, 2000f, GroundMask))
                {
                    if (hit.point.y < treeLine) continue;
                    var angle = Random.Range(0, 360);
                    var rockIndex = Random.Range(0, rocks.Length);
                    var tree = Instantiate(rocks[rockIndex], hit.point, Quaternion.AngleAxis(angle, Vector3.up));
                    tree.transform.SetParent(detailsContainer.transform);
                }
            }
        }

        public void OnValidate()
        {
            if (mapWidth < 1) mapWidth = 1;
            if (mapHeight < 1) mapHeight = 1;
            noiseScale = Mathf.Max(noiseScale, MinScale);
            if (lacunarity < 1) lacunarity = 1;
            if (octaves < 0) octaves = 0;

            falloffMap = FalloffGenerator.GeneratorFalloffMap(mapWidth, mapHeight);
        }
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}