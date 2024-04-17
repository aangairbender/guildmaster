using Gameplay.Characters;
using Gameplay.Furniture;
using Gameplay.Items;
using System;
using UnityEngine;

namespace Gameplay.Tasks
{
    public class CarryTask : Task
    {
        public DroppedItem DroppedItem { get; }
        public Chest Target { get; }

        public override Vector3? LocationHint => DroppedItem.Position;

        public CarryTask(DroppedItem droppedItem, Chest target)
        {
            DroppedItem = droppedItem;
            Target = target;
        }

        public override bool Apply(Character agent, float deltaTime)
        {
            if (DroppedItem.Quantity > 0)
            {
                if (Vector3.Distance(agent.Position, DroppedItem.Position) > 0.5)
                {
                    agent.MoveTo(DroppedItem.Position, deltaTime);
                    return false;
                }

                agent.Pickup(DroppedItem.Item, DroppedItem.Quantity);
                DroppedItem.Quantity = 0;
            }


            if (Vector3.Distance(agent.Position, Target.Position) > 0.5)
            {
                agent.State = "carrying";
                agent.MoveTo(Target.Position, deltaTime);
                return false;
            }

            agent.TransferCarriage(Target);
            return true;
        }
    }
}
