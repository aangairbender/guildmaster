using System;
using UnityEngine;

namespace Gameplay.Building
{
    public struct Wall : IEquatable<Wall>
    {
        public Vector2Int a;
        public Vector2Int b;

        public Wall(Vector2Int a, Vector2Int b)
        {
            this.a = a;
            this.b = b;
        }

        public bool Equals(Wall other) =>
            (a == other.a && b == other.b) || (a == other.b && b == other.a);
    }
}
