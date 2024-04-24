using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Core.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Core/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        // here can attach various configs from all gameplay mechanics

        [Header("Prefabs")]
        public GameObject TreePrefab;
        public GameObject CharacterPrefab;
        public GameObject ChestPrefab;
        public GameObject DroppedItemPrefab;
    }
}
