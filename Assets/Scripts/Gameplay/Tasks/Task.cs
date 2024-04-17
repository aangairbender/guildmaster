using Gameplay.Characters;
using UnityEngine;
using VContainer;

namespace Gameplay.Tasks
{
    // every tasks involves
    // 1. (optional) come to some location
    // 2. perform action (which changes game state)
    public abstract class Task
    {
        /// <summary>
        /// returns whether task is completed (before starting the task)
        /// </summary>
        public abstract bool Apply(Character agent, float deltaTime);

        // need also a way to define interactive way to build a task
        // e.g. for TravelTask need to ask player for destination
        // maybe that can be done separately in a more hardcoded way

        // maybe need a method to estimate completion time for most efficient planning

        public abstract Vector3? LocationHint { get; }
    }
}
