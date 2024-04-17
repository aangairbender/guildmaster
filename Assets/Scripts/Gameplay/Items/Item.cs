using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Items
{
    //[CreateAssetMenu(fileName = "NewItem", menuName = "Gameplay/Item")]
    //public class Item : ScriptableObject
    //{
    //    public string Name;
    //}

    public class Item
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Item other && Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
