using System;
using UnityEngine;

namespace Gameplay.Input
{
    // TODO: make this service more game aware (e.g. instead of keys it would operate on "actions/commands")
    // So not key B is pressed, but command "Build" is executed
    // in game settings player can map command to a different physical key
    public interface IInputService
    {
        // temporary
        bool IsKeyDown(KeyCode key);

        event Action OnAction1;
    }

    public class InputService : MonoBehaviour, IInputService
    {
        public event Action OnAction1;

        public bool IsKeyDown(KeyCode key) => UnityEngine.Input.GetKey(key);

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnAction1?.Invoke();
            }
        }
    }
}
