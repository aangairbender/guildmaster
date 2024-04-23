using Unity.Entities;

namespace Gameplay.Trees.Components
{
    public struct TreeData : IComponentData
    {
        public const float MaxDurability = 5;
        public const int DropWoodAmount = 5;

        public float Durability;
    }
}
