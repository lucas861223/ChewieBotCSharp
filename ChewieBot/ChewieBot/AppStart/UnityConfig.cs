using ChewieBot.Database.Repository;
using ChewieBot.Database.Repository.Implementation;
using ChewieBot.Scripting.Services;
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
            container.RegisterType<ITwitchClient, TwitchClient>(new TransientLifetimeManager(), new InjectionConstructor(typeof(IUserService)));
            container.RegisterType<ITwitchApi, TwitchAPI>(new TransientLifetimeManager(), new InjectionConstructor(typeof(IUserService)));
            container.RegisterType<ITwitchService, TwitchService>(new TransientLifetimeManager(), new InjectionConstructor(typeof(ITwitchClient), typeof(ITwitchApi), typeof(IUserService), typeof(ICommandService)));
            container.RegisterType<ICommandService, CommandService>(new TransientLifetimeManager());
            container.RegisterType<ScriptUserService>(new TransientLifetimeManager(), new InjectionConstructor(typeof(IUserService)));
        }

        public static T Resolve<T>()
        {
            return GetConfiguredContainer().Resolve<T>();
        }
    }
}
