using System;

namespace Gameplay
{
    public class GameplayException : Exception
    {
        public GameplayException(string message) : base(message) { }
    }
}
