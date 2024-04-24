using UnityEngine;

namespace Gameplay.Gathering.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Gathering/GatheringNode")]
    public class GatheringNodeData : ScriptableObject
    {
        public ResourceData Resource;
        public int Amount;
    }
}
