using Gameplay.Gathering.Data;
using UnityEngine;

namespace Gameplay.Gathering.Components
{
    public class GatheringNode : MonoBehaviour
    {
        [field: SerializeField]
        public GatheringNodeConfig Config { get; private set; }

        [field: SerializeField]
        public int AmountLeft { get; private set; }

        void Start()
        {
            Debug.Assert(Config != null);
            AmountLeft = Config.InitialAmount;
        }
    }
}
    