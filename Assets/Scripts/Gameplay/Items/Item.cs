using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Gameplay/Item")]
    public class Item : ScriptableObject
    {
        public string Name;
    }
}
