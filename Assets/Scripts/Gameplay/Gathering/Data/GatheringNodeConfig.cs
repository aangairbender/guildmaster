using UnityEngine;

namespace Gameplay.Gathering.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Gathering/GatheringNodeConfig")]
    public class GatheringNodeConfig : ScriptableObject
    {
        public Resource Resource;
        [Min(1)]
        public int InitialAmount;
    }
}
