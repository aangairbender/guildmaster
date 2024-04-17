using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Gameplay.Cameras
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;

        // TODO: cache results per layerMask, clean cache in the beginning of each update
        public bool RaycastMousePosition(LayerMask layerMask, out RaycastHit hit)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = mainCamera.nearClipPlane;
            var ray = mainCamera.ScreenPointToRay(mousePos);
            return Physics.Raycast(ray, out hit, mainCamera.farClipPlane, layerMask);
        }
    }
}
