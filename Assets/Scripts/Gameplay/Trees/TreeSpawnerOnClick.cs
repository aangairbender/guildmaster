using Gameplay.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Gameplay.Trees
{
    public class TreeSpawnerOnClick : MonoBehaviour
    {
        [Inject] GameController gameController;
        [Inject] CameraService cameraService;

        [SerializeField] LayerMask groundLayerMask;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (cameraService.RaycastMousePosition(groundLayerMask, out var hit))
                {
                    gameController.SpawnTreeAt(hit.point);
                }
            }   
        }
    }
}
