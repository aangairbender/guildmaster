using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gameplay.Terrain
{
    public static class Noise
    {
        public static float[,] GenerateNoiseMap(
            int mapWidth,
            int mapHeight,
            int seed,
            float scale,
            int octaves,
            float persistance,
            float lacunarity,
            Vector2 offset
        )
        {
            var noiseMap = new float[mapWidth, mapHeight];

            var prng = new System.Random(seed);
            var octaveOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; ++i)
            {
                var offsetX = prng.Next(-100000, 100000) + offset.x;
                var offsetY = prng.Next(-100000, 100000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            var maxNoiseHeight = float.MinValue;
            var minNoiseHeight = float.MaxValue;

            var halfWidth = mapWidth / 2f;
            var halfHeight = mapHeight / 2f;

            for (int x = 0; x < mapWidth; ++x)
                for (int y = 0; y < mapHeight; ++y)
                {
                    var amplitude = 1f;
                    var frequency = 1f;
                    var noiseHeight = 0f;

                    for (int i = 0; i < octaves; ++i)
                    {
                        var sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                        var sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                        var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    maxNoiseHeight = Mathf.Max(maxNoiseHeight, noiseHeight);
                    minNoiseHeight = Mathf.Min(minNoiseHeight, noiseHeight);

                    noiseMap[x, y] = noiseHeight;
                }

            for (int x = 0; x < mapWidth; ++x)
                for (int y = 0; y < mapHeight; ++y)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }

            return noiseMap;
        }
    }
}