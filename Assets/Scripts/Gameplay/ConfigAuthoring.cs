using Unity.Entities;
using UnityEngine;

namespace Gameplay
{
    public class ConfigAuthoring : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject TreePrefab;

        class Baker : Baker<ConfigAuthoring>
        {
            public override void Bake(ConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Config
                {
                    TreePrefab = GetEntity(authoring.TreePrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }

    public struct Config : IComponentData
    {
        public Entity TreePrefab;
    }
}
