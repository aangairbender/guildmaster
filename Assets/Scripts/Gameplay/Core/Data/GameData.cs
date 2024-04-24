using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Core.Data
{
    public abstract class GameData : ScriptableObject
    {
        [SerializeField]
        public GameDataId Id;

        public string IdStr => Id.ToString();

        protected GameData()
        {
            Id = GameDataId.Next();
        }
    }
    
    [Serializable]
    public readonly struct GameDataId
    {
        // two 64-bit numbers, can represent guid/uuid etc
        [SerializeField]
        private readonly ulong raw;

        private GameDataId(ulong raw) { this.raw = raw; }

        public static GameDataId Next()
        {
            var currentTime = DateTime.UtcNow.ToBinary();
            return new GameDataId((ulong)currentTime);
        }
    }
}
