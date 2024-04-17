using Gameplay.Characters;
using Gameplay.Furniture;
using Gameplay.Items;
using Gameplay.Tasks;
using Gameplay.Time;
using Gameplay.Trees;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace Gameplay
{
    public class GameController : MonoBehaviour
    {
        [Inject] ITimeService timeService;
        [Inject] GameWorld world;

        void Start()
        {
            for (int i = 0; i < 10; ++i)
            {
                var position = new Vector3(
                    UnityEngine.Random.value * 20 - 10,
                    0.0f,
                    UnityEngine.Random.value * 20 - 10
                );
                SpawnTreeAt(position);
            }
            for (int i = 0; i < 3; ++i)
            {
                SpawnCharacterAt(Vector3.zero);
            }

            var chest = new Chest { Position = new Vector3(3, 0, 3), Rotation = Quaternion.identity };
            world.Chests.Add(chest);
        }

        public void SpawnTreeAt(Vector3 position)
        {
            var tree = new TreeModel { Position = position, Durability = TreeModel.MaxDurability };
            world.Trees.Add(tree);
        }

        public void SpawnCharacterAt(Vector3 position)
        {
            var character = new Character
            {
                Task = null,
                Position = position,
                Rotation = Quaternion.identity,
            };
            world.Characters.Add(character);
        }

        public void SpawnDroppedItemAt(Vector3 position, Item item, int quantity)
        {
            var droppedItem = new DroppedItem
            {
                Position = position,
                Item = item,
                Quantity = quantity
            };
            world.DroppedItems.Add(droppedItem);
        }

        void Update()
        {
            AssignTasks();
            PerformTasks();
            RemoveTrees();
            RemoveDroppedOfZeroQuantity();
        }

        void AssignTasks()
        {
            var allTasks = new List<Task>();
            foreach (var t in world.Trees)
            {
                allTasks.Add(new ChopTask(t));
            }
            foreach (var d in world.DroppedItems)
            {
                // hack
                var closestChest = world.Chests[0];
                allTasks.Add(new CarryTask(d, closestChest));
            }

            foreach (var c in world.Characters)
            {
                if (c.Task != null)
                {
                    allTasks.Remove(c.Task);
                }
            }

            var pairs = new List<Tuple<Character, Task>>();
            foreach (var c in world.Characters)
            {
                if (c.Task != null) continue;
                foreach (var t in allTasks)
                {
                    pairs.Add(new Tuple<Character, Task>(c, t));
                }
            }

            pairs.Sort((a, b) => Vector3.SqrMagnitude(a.Item1.Position - a.Item2.LocationHint.GetValueOrDefault(Vector3.zero)).CompareTo(Vector3.SqrMagnitude(b.Item1.Position - b.Item2.LocationHint.GetValueOrDefault(Vector3.zero))));

            var takenTasks = new HashSet<Task>();
            var takenCharacters = new HashSet<Character>();
            foreach (var pair in pairs)
            {
                if (takenCharacters.Contains(pair.Item1)) continue;
                if (takenTasks.Contains(pair.Item2)) continue;

                takenCharacters.Add(pair.Item1);
                takenTasks.Add(pair.Item2);
                pair.Item1.Task = pair.Item2;
            }

            foreach (var c in world.Characters)
            {
                c.Task ??= new TravelTask(Vector3.zero);
            }
        }

        void PerformTasks()
        {
            foreach (var c  in world.Characters)
            {
                var completed = c.Task?.Apply(c, timeService.DeltaTime);
                if (completed == true) c.Task = null;
            }
        }

        void RemoveTrees()
        {
            var destroyed = world.Trees.Where(t => t.Durability == 0.0f).ToList();
            foreach (var t in destroyed) world.Trees.Remove(t);
        }

        void RemoveDroppedOfZeroQuantity()
        {
            var destroyed = world.DroppedItems.Where(d => d.Quantity == 0).ToList();
            foreach (var d in destroyed) world.DroppedItems.Remove(d);
        }
    }
}
