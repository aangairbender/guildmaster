using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Building
{
    public static class Utils
    {
        public static float Angle(Vector2Int vector2)
        {
            var angle = Mathf.Atan2(vector2.y, vector2.x);
            if (angle < 0) angle += 2f * Mathf.PI;
            return angle;
        }

        /// <summary>
        /// Rotates the vector around origin in counterclockwise direction.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="angle">Angle in radians</param>
        /// <returns>Rotated vector</returns>
        public static Vector2 Rotate(Vector2 v, float angle)
        {
            return new Vector2(
                v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle),
                v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle)
            );
        }

        public static List<Wall> ExtractOuterWalls(List<Wall> walls)
        {
            throw new NotImplementedException();
        }
    }
}
