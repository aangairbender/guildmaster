using Gameplay.Core;
using UnityEngine;

namespace Gameplay.Gathering.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Gathering/Resource")]
    public class Resource : GameData
    {
        public string Name;
        public Sprite Icon;
    }
}
