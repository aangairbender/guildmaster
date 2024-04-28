using System;
using UnityEngine;

namespace Gameplay.Characters
{
    public class Character
    {
        public Vector3 Position { get; set; }
        // TODO: maybe remove, we dont care about rotation on this level, can just define rotation at view level
        public Quaternion Rotation { get; set; }

        // temporary
        public string State { get; set; }
    }
}
