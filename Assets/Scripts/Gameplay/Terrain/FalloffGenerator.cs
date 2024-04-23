using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Terrain
{
    public static class FalloffGenerator
    {
        public static float[,] GeneratorFalloffMap(int width, int height)
        {
            var map = new float[width, height];

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    var x = 1f * i / width * 2 - 1;
                    var y = 1f * j / height * 2 - 1;

                    var value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                    map[i, j] = Evaluate(value);
                }
            }

            return map;
        }

        static float Evaluate(float value)
        {
            var a = 3f;
            var b = 2.2f;

            return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
        }
    }
}
