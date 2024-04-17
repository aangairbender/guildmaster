using Gameplay.Characters;
using Gameplay.Items;
using Gameplay.Trees;
using UnityEngine;

namespace Gameplay.Tasks
{
    public class ChopTask : Task
    {
        public TreeModel Tree { get; }

        public override Vector3? LocationHint => Tree.Position;

        public ChopTask(TreeModel tree)
        {
            Tree = tree;
        }

        public override bool Apply(Character agent, float deltaTime)
        {
            if (Tree.Durability <= 0) return true;

            if (Vector3.Distance(agent.Position, Tree.Position) > 0.5)
            {
                agent.MoveTo(Tree.Position, deltaTime);
                return false;
            }

            agent.State = "chopping";
            agent.Rotation *= Quaternion.AngleAxis(180.0f * deltaTime, Vector3.up);
            var emitWoodCnt = Tree.Hit(1.0f * deltaTime);
            if (emitWoodCnt > 0)
            {
                var dropMinRange = 0.5f;
                var dropMaxRange = 1.0f;
                var dropPos = Tree.Position + Quaternion.AngleAxis(Random.value * 360.0f, Vector3.up) * Vector3.forward * Random.Range(dropMinRange, dropMaxRange);
                var droppedItem = new DroppedItem { Item = new Item { Name = "Wood" }, Position = dropPos, Quantity = emitWoodCnt };
                GameWorld.Default.DroppedItems.Add(droppedItem);
            }
            return false;
        }
    }
}
