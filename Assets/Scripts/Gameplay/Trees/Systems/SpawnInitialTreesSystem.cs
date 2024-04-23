using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Gameplay.Trees.Systems
{
    public partial struct SpawnInitialTreesSystem : ISystem
    {
        uint updateCounter;

        [BurstCompile]
        void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Config>();
        }

        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            state.Enabled = false;
            var config = SystemAPI.GetSingleton<Config>();

            var instances = state.EntityManager.Instantiate(config.TreePrefab, 10, Allocator.Temp);

            var random = Random.CreateFromIndex(updateCounter++);

            foreach (var instance in instances)
            {
                var transform = SystemAPI.GetComponentRW<LocalTransform>(instance);
                var pos = random.NextFloat3();
                pos.y = 0;
                transform.ValueRW.Position = (pos - new float3(0.5f, 0, 0.5f)) * 20;
            }
        }
    }
}
