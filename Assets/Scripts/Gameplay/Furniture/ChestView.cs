using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Furniture
{
    public class ChestView : MonoBehaviour
    {
        Chest chest;

        public void SetData(Chest chest)
        {
            this.chest = chest;
            Invalidate();
        }

        private void Update()
        {
            Invalidate();
        }

        private void Invalidate()
        {
            transform.SetPositionAndRotation(chest.Position, chest.Rotation);
        }
    }
}
