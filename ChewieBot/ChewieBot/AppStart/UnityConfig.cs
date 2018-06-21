using ChewieBot.Commands;
using ChewieBot.Database.Repository;
using ChewieBot.Database.Repository.Implementation;
using ChewieBot.Scripting;
using ChewieBot.Services;
using ChewieBot.Services.Implementation;
using ChewieBot.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace ChewieBot.AppStart
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void Setup()
        {
            var container = GetConfiguredContainer();
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserData, UserRepository>(new TransientLifetimeManager());
            container.RegisterType<IUserService, UserService>(new TransientLifetimeManager(), new InjectionConstructor(typeof(IUserData)));
            container.RegisterType<ITwitchApi, TwitchAPI>(new TransientLifetimeManager(), new InjectionConstructor(typeof(IUserService)));
            container.RegisterType<ITwitchService, TwitchService>(new SingletonLifetimeManager(), new InjectionConstructor(typeof(IUserService), typeof(ICommandService)));
            container.RegisterType<ICommandService, CommandService>(new TransientLifetimeManager());
            container.RegisterType<ICommandRepository, CommandRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(IPythonEngine)));
            container.RegisterType<IPythonEngine, PythonEngine>(new ContainerControlledLifetimeManager());
            container.RegisterType<IChatEventService, ChatEventService>(new TransientLifetimeManager());
            container.RegisterType<IChatEventData, ChatEventRepository>(new TransientLifetimeManager());
            container.RegisterType<IEventWinnerService, EventWinnerService>(new TransientLifetimeManager());
            container.RegisterType<IEventWinnerData, EventWinnerRepository>(new TransientLifetimeManager());
            container.RegisterType<ISongQueueService, SongQueueService>(new TransientLifetimeManager());
            container.RegisterType<IYoutubeService, YoutubeService>(new TransientLifetimeManager());
        }

        public static T Resolve<T>()
        {
            return GetConfiguredContainer().Resolve<T>();
        }
    }
}
