using UnityEngine;

namespace Gameplay.Items
{
    public class DroppedItemView : MonoBehaviour
    {
        DroppedItem item;
        private float angle;

        public void SetItem(DroppedItem item)
        {
            this.item = item;
            angle = Random.value * 360.0f;
            Invalidate();
        }

        private void Update()
        {
            Invalidate();
        }

        void Invalidate()
        {
            transform.SetPositionAndRotation(item.Position, Quaternion.AngleAxis(angle, Vector3.up));
        }
    }
}
