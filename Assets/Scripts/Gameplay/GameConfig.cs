using Gameplay.Characters;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        // here can attach various configs from all gameplay mechanics

        [Header("Prefabs")]
        public GameObject TreePrefab;
        public CharacterView CharacterPrefab;
        public GameObject ChestPrefab;
        public GameObject DroppedItemPrefab;
    }
}
