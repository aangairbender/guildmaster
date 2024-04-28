using UnityEngine;

namespace Gameplay.Characters
{
    public class CharacterView : MonoBehaviour
    {
        // should be set only once actually
        Character character;

        public Character Character => character;

        public void SetCharacter(Character character)
        {
            this.character = character;
            Invalidate();
        }

        void Update()
        {
            Invalidate();
        }

        void Invalidate()
        {
            transform.SetPositionAndRotation(character.Position, character.Rotation);
        }
    }
}
