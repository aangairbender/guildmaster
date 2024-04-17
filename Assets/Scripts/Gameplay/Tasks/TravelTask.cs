using Gameplay.Characters;
using Gameplay.Time;
using UnityEngine;
using VContainer;

namespace Gameplay.Tasks
{
    public class TravelTask : Task
    {
        public Vector3 Destination { get; }

        public override Vector3? LocationHint => null;

        public TravelTask(Vector3 destination)
        {
            Destination = destination;
        }

        public override bool Apply(Character agent, float deltaTime)
        {
            // if (carryingAnything) return PrimitiveAction.Drop;

            // checking that agent reached destination
            if (Vector3.Distance(agent.Position, Destination) < 0.001) return true;

            agent.MoveTo(Destination, deltaTime);

            return false;
        }
    }
}
