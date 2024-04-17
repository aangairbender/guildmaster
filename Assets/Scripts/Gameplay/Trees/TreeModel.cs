using UnityEngine;

namespace Gameplay.Trees
{
    public class TreeModel
    {
        public const float MaxDurability = 5;
        public const int DropWoodAmount = 5;

        public Vector3 Position { get; set; }
        public float Durability { get; set; }

        // returns how many wood to drop if wood would be emmited
        public int Hit(float damage)
        {
            if (Destroyed()) return 0;

            if (damage >= Durability)
            {
                Durability = 0.0f;
                return DropWoodAmount;
            } else
            {
                Durability -= damage;
                return 0;
            }
        }

        public bool Destroyed() => Durability == 0.0f;
    }
}
