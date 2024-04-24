using Unity.Entities;

namespace Gameplay.Characters.Components
{
    public class Character : IComponentData
    {
        public const float Speed = 2.0f;

        public CharacterState State;
    }

    public enum CharacterState
    {
        Idle,
        Walking,
        Chopping,
    }
}
