using Gameplay.Time;
using UnityEngine;
using VContainer;

namespace Gameplay.Cameras
{
    public class CameraController : MonoBehaviour
    {
        public Transform cameraTransform;

        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public Vector3 zoomAmount;

        public Vector3 newPosition;
        public Quaternion newRotation;
        public Vector3 newZoom;

        void Start()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovementInput();
        }

        void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, UnityEngine.Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, UnityEngine.Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, UnityEngine.Time.deltaTime * movementTime);
        }

        void HandleMovementInput()
        {
            if (UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += transform.forward * movementSpeed;
            }
            if (UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += transform.forward * -movementSpeed;
            }
            if (UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += transform.right * movementSpeed;
            }
            if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += transform.right * -movementSpeed;
            }
            if (UnityEngine.Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (UnityEngine.Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }
            newZoom += zoomAmount * UnityEngine.Input.mouseScrollDelta.y;
        }
    }
}