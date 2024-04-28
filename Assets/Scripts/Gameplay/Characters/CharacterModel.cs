using System;
using System.Collections.Generic;

namespace Gameplay.Characters
{
    public interface ICharacterModel
    {
        Guid AddCharacter(Character character);
        bool RemoveCharacter(Guid id);
        event Action<Guid, Character> CharacterAdded;
        event Action<Guid> CharacterRemoved;
    }

    public class CharacterModel : ICharacterModel
    {
        private readonly Dictionary<Guid, Character> characters = new();

        public event Action<Guid, Character> CharacterAdded;
        public event Action<Guid> CharacterRemoved;

        public Guid AddCharacter(Character character)
        {
            var id = Guid.NewGuid();
            characters.Add(id, character);
            CharacterAdded?.Invoke(id, character);
            return id;
        }

        public bool RemoveCharacter(Guid id)
        {
            if (characters.Remove(id))
            {
                CharacterRemoved?.Invoke(id);
                return true;
            }
            return false;
        }
    }
}
