using Gameplay.Furniture;
using Gameplay.Items;
using Gameplay.Tasks;
using System;
using UnityEngine;

namespace Gameplay.Characters
{
    public class Character
    {
        public const float Speed = 2.0f;

        public Task Task { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public Item CarriedItem { get; private set; }
        public int CarriedQuantity { get; private set; }

        // temporary
        public string State { get; set; }

        public void MoveTo(Vector3 target, float deltaTime)
        {
            var direction = target - Position;
            Rotation = Quaternion.LookRotation(direction);
            var travelDistance = Speed * deltaTime;
            if (travelDistance * travelDistance >= direction.sqrMagnitude)
            {
                Position = target;
            }
            else
            {
                Position += direction.normalized * travelDistance;
            }
        }

        public void Pickup(Item item, int quantity)
        {
            if (CarriedItem == item)
            {
                CarriedQuantity += quantity;
            } else
            {
                if (CarriedQuantity > 0)
                {
                    GameWorld.Default.DroppedItems.Add(new DroppedItem { Item = item, Quantity = quantity, Position = Position });
                }
                CarriedItem = item;
                CarriedQuantity = quantity;
            }
        }

        public void TransferCarriage(Chest chest)
        {
            chest.Put(CarriedItem, CarriedQuantity);
            CarriedQuantity = 0;
        }
    }
}
