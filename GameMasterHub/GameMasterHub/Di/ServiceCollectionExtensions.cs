using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Infrastructure.Storage;
using GameMasterHub.Screens.Auth;
using GameMasterHub.Screens.CreateLobby;
using GameMasterHub.Screens.Home;
using GameMasterHub.Screens.MainView;
using GameMasterHub.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GameMasterHub.Di
{
    public static class ServiceCollectionExtensions {
        public static void AddCommonServices(this IServiceCollection collection) {
            collection.AddSingleton<TokenStorage>();
            collection.AddHttpClient<AuthRepository>(client =>
                {
                    client.BaseAddress = new Uri("http://127.0.0.1/");
                });
            
            collection.AddTransient<AuthViewModel>();
            collection.AddTransient<HomeViewModel>();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<CreateLobbyViewModel>();
        }
    }
}
