using Gameplay.Trees.Components;
using Unity.Entities;
using UnityEngine;

namespace Gameplay.Trees.Authoring
{
    public class TreeAuthoring : MonoBehaviour
    {
        class Baker : Baker<TreeAuthoring>
        {
            public override void Bake(TreeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new TreeData
                {
                    Durability = TreeData.MaxDurability,
                });
            }
        }
    }
}
