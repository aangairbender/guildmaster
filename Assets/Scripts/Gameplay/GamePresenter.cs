using Gameplay.Characters;
using Gameplay.Furniture;
using Gameplay.Items;
using Gameplay.Trees;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using VContainer;

namespace Gameplay
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] CharacterView characterViewPrefab;
        [SerializeField] GameObject treePrefab;
        [SerializeField] DroppedItemView droppedItemViewPrefab;
        [SerializeField] ChestView chestPrefab;

        [Inject] GameWorld world;

        Dictionary<TreeModel, GameObject> trees = new();
        Dictionary<DroppedItem, DroppedItemView> drop = new();

        private void Awake()
        {
            world.Trees.CollectionChanged += Trees_CollectionChanged;
            world.Characters.CollectionChanged += Characters_CollectionChanged;
            world.DroppedItems.CollectionChanged += DroppedItems_CollectionChanged;
            world.Chests.CollectionChanged += Chests_CollectionChanged;
        }

        private void Chests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Chest item in e.NewItems)
                {
                    var view = Instantiate(chestPrefab);
                    view.SetData(item);
                }
            }

            if (e.OldItems != null)
            {
                Debug.LogWarning("deleting droppeditems not supported by presenter");
            }
        }

        private void OnDestroy()
        {
            world.Trees.CollectionChanged -= Trees_CollectionChanged;
            world.Characters.CollectionChanged -= Characters_CollectionChanged;
            world.DroppedItems.CollectionChanged -= DroppedItems_CollectionChanged;
            world.Chests.CollectionChanged -= Chests_CollectionChanged;
        }

        private void DroppedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DroppedItem item in e.NewItems)
                {
                    var view = Instantiate(droppedItemViewPrefab);
                    drop[item] = view;
                    view.SetItem(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (DroppedItem item in e.OldItems)
                {
                    if (drop.TryGetValue(item, out var go))
                    {
                        Destroy(go.gameObject);
                        drop.Remove(item);
                    }
                }
            }
        }

        private void Characters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Character item in e.NewItems)
                {
                    var characterView = Instantiate(characterViewPrefab);
                    characterView.SetCharacter(item);
                }
            }

            if (e.OldItems != null)
            {
                Debug.LogWarning("deleting characters not supported by presenter");
            }
        }


        private void Trees_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TreeModel tree in e.NewItems)
                {
                    trees[tree] = Instantiate(treePrefab, tree.Position, Quaternion.identity);
                }
            }

            if (e.OldItems != null)
            {
                foreach (TreeModel tree in e.OldItems)
                {
                    if (trees.TryGetValue(tree, out GameObject go))
                    {
                        Destroy(go);
                        trees.Remove(tree);
                    }
                }
            }
        }
    }
}
