using Gameplay;
using Gameplay.Cameras;
using Gameplay.Characters;
using Gameplay.Input;
using Gameplay.Shipment;
using Gameplay.Time;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] GameConfig gameConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(gameConfig);

            // Unity services
            builder.UseComponents(components =>
            {
                components.AddInHierarchy<TimeService>().AsImplementedInterfaces();
                components.AddInHierarchy<InputService>().AsImplementedInterfaces();
                components.AddInHierarchy<CameraService>().AsImplementedInterfaces();
            });

            // Gameplay modrls
            builder.Register<ICharacterModel, CharacterModel>(Lifetime.Singleton);
            builder.Register<ICargoModel, CargoModel>(Lifetime.Singleton);

            // Controllers
            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<Gameplay.Characters.CharacterController>();
                entryPoints.Add<CargoController>();
                entryPoints.Add<GamePresenter>();
            });
        }
    }
}
