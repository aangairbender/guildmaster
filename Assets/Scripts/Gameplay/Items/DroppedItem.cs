using UnityEngine;

namespace Gameplay.Items
{
    public class DroppedItem
    {
        public Vector3 Position { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
