using UnityEngine;

namespace Gameplay.Cameras
{
    public interface ICameraService
    {
        bool RaycastMousePosition(LayerMask layerMask, out RaycastHit hit);
    }

    public class CameraService : MonoBehaviour, ICameraService
    {
        [SerializeField] Camera mainCamera;

        // TODO: cache results per layerMask, clean cache in the beginning of each update
        public bool RaycastMousePosition(LayerMask layerMask, out RaycastHit hit)
        {
            var mousePos = UnityEngine.Input.mousePosition;
            mousePos.z = mainCamera.nearClipPlane;
            var ray = mainCamera.ScreenPointToRay(mousePos);
            return Physics.Raycast(ray, out hit, mainCamera.farClipPlane, layerMask);
        }
    }
}
