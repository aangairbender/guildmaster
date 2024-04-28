using Gameplay.Input;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Characters
{
    public class CharacterController : IStartable, IDisposable
    {
        [Inject] IInputService inputService;
        [Inject] ICharacterModel characterModel;

        public void Start()
        {
            inputService.OnAction1 += InputService_OnAction1;
        }

        public void Dispose()
        {
            inputService.OnAction1 -= InputService_OnAction1;
        }

        private void InputService_OnAction1()
        {
            var character = new Character
            {
                Position = Vector3.zero
            };
            characterModel.AddCharacter(character);
        }
    }
}
