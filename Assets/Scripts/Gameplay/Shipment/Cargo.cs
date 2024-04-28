using Gameplay.Gathering.Data;
using System.Collections.Generic;

namespace Gameplay.Shipment
{
    public struct Cargo
    {
        private readonly Dictionary<Resource, int> resources;

        public IReadOnlyDictionary<Resource, int> Resources => resources;
        public int SizeLimit { get; set; }

        public bool IsEmpty() => Resources.Count == 0;
    }
}
