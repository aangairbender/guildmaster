using Gameplay.Gathering.Data;
using Unity.Entities;

namespace Gameplay.Gathering.Components
{
    public struct GatheringNode : IComponentData
    {
        public ResourceData Resource;
        public int Amount;

        public GatheringNode(ResourceData resource, int amount)
        {
            Resource = resource;
            Amount = amount;
        }
    }
}
