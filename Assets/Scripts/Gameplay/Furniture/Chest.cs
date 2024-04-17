using Gameplay.Items;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Gameplay.Furniture
{
    public class Chest
    {
        public Dictionary<Item, int> Content { get; } = new();
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public void Put(Item item, int quantity)
        {
            if (Content.TryGetValue(item, out int current))
            {
                Content[item] = current + quantity;
            }
            else
            {
                Content.Add(item, quantity);
            }
        }
    }
}
