using Gameplay;
using Gameplay.Cameras;
using Gameplay.Characters;
using Gameplay.Time;
using Gameplay.Trees;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] CameraService cameraService;

        protected override void Configure(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            // builder.RegisterMessageBroker<CharacterCreated>(options);

            builder.Register<ITimeService, TimeService>(Lifetime.Singleton);
            builder.RegisterComponent(cameraService);

            var gameWorld = new GameWorld();
            GameWorld.Default = gameWorld;

            builder.RegisterInstance(gameWorld);

            builder.RegisterComponentInHierarchy<GamePresenter>();
            builder.RegisterComponentInHierarchy<GameController>();
            builder.RegisterComponentInHierarchy<TreeSpawnerOnClick>();
            builder.RegisterComponentInHierarchy<CharacterUI>();
        }
    }
}
