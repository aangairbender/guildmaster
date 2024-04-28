using Gameplay.Characters;
using System;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Shipment
{
    public class CargoController : IStartable, IDisposable
    {
        // TODO: consider replacing with event bus to avoid direct access to other system's model
        [Inject] ICharacterModel characterModel;
        [Inject] ICargoModel cargoModel;

        public void Start()
        {
            characterModel.CharacterAdded += CharacterModel_CharacterAdded;
        }

        public void Dispose()
        {
            characterModel.CharacterAdded -= CharacterModel_CharacterAdded;
        }

        private void CharacterModel_CharacterAdded(Guid id, Character character)
        {
            var cargo = new Cargo();
            cargoModel.AttachCargo(id, cargo);
        }
    }
}
