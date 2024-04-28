using Gameplay.Characters;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay
{
    public class GamePresenter : IStartable, IDisposable
    {
        [Inject] GameConfig gameConfig;
        [Inject] ICharacterModel characterModel;

        public void Start()
        {
            characterModel.CharacterAdded += CharacterService_CharacterAdded;
        }
        public void Dispose()
        {
            characterModel.CharacterAdded -= CharacterService_CharacterAdded;
        }

        private void CharacterService_CharacterAdded(Guid id, Character character)
        {
            var characterView = GameObject.Instantiate(gameConfig.CharacterPrefab, character.Position, character.Rotation);
            characterView.SetCharacter(character);
            characterView.gameObject.name = id.ToString();
        }
    }
}
