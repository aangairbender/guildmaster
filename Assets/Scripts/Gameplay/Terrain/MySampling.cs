using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gameplay.Terrain
{
    public static class MySampling
    {
        public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize)
        {
            var cntX = sampleRegionSize.x / radius;
            var cntY = sampleRegionSize.y / radius;
            var points = new List<Vector2>();
            for (int x = 0; x < cntX; ++x)
                for (int y = 0; y < cntY; ++y)
                {
                    points.Add(new Vector2(radius * x, radius * y));
                }
            return points;
        }
    }
}