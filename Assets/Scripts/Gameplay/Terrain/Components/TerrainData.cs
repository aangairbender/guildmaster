using Unity.Entities;

namespace Gameplay.Terrain.Components
{
    public class TerrainData : IComponentData
    {
        public int Width;
        public int Height;
        public float[,] HeightMap;

        public TerrainData() { }

        public TerrainData(int width, int height, float[,] heightMap)
        {
            Width = width;
            Height = height;
            HeightMap = heightMap;
        }
    }
}
