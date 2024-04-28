using System;
using System.Collections.Generic;

namespace Gameplay.Shipment
{
    public interface ICargoModel
    {
        void AttachCargo(Guid target, Cargo cargo);
        bool TryGetCargo(Guid target, out Cargo cargo);
        void UnloadCargo(Guid source, Guid destination);
    }

    public class CargoModel : ICargoModel
    {
        private readonly Dictionary<Guid, Cargo> cargos = new();

        public void AttachCargo(Guid target, Cargo cargo)
        {
            if (cargos.ContainsKey(target))
                throw new GameplayException("The target already has cargo attached");
            cargos.Add(target, cargo);
        }

        public bool TryGetCargo(Guid target, out Cargo cargo)
        {
            return cargos.TryGetValue(target, out cargo);
        }

        public void UnloadCargo(Guid source, Guid destination)
        {
            if (!cargos.ContainsKey(source))
                throw new GameplayException("Source doesn't have cargo attached");
            if (!cargos.ContainsKey(destination))
                throw new GameplayException("Destination doesn't have cargo attached");

            throw new NotImplementedException();
        }
    }
}
