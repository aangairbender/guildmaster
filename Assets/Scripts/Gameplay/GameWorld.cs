using Gameplay.Characters;
using Gameplay.Furniture;
using Gameplay.Items;
using Gameplay.Terrain;
using Gameplay.Trees;
using System.Collections.ObjectModel;

namespace Gameplay
{
    public class GameWorld
    {
        public static GameWorld Default;

        public ObservableCollection<Character> Characters { get; } = new();
        public ObservableCollection<TreeModel> Trees { get; } = new();
        public ObservableCollection<DroppedItem> DroppedItems { get; } = new();
        public ObservableCollection<Chest> Chests { get; } = new();
    }
}
