using GameMasterHub.Infrastructure.Managers;
using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Infrastructure.Storage;
using GameMasterHub.Screens.Auth;
using GameMasterHub.Screens.CreateGame;
using GameMasterHub.Screens.CreateLobby;
using GameMasterHub.Screens.CreateTemplateCharacter;
using GameMasterHub.Screens.Home;
using GameMasterHub.Screens.Lobby;
using GameMasterHub.Screens.MainView;
using GameMasterHub.Screens.TemplatesCharacters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GameMasterHub.Di
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            Uri baseAddress = new Uri("http://147.45.158.230:8080/");
            string webSocketAddress = "https://example.com/hub";

            collection.AddSingleton<ApplicationDataManager>();
            collection.AddSingleton<TokenStorage>();
            collection.AddSingleton<GameStorage>();
            collection.AddSingleton<LobbyStorage>();

            collection.AddHttpClient<AuthRepository>(client =>
                {
                    client.BaseAddress = baseAddress;
                });
            collection.AddHttpClient<GameRepository>(client =>
                {
                    client.BaseAddress = baseAddress;
                });
            collection.AddHttpClient<LobbyRepository>(client =>
                {
                    client.BaseAddress = baseAddress;
                });

            collection.AddTransient<AuthViewModel>();
            collection.AddTransient<HomeViewModel>();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<CreateLobbyViewModel>();
            collection.AddTransient<LobbyViewModel>();
            collection.AddTransient<CreateGameViewModel>();
            collection.AddTransient<TemplatesCharactersViewModel>();
            collection.AddTransient<CreateTemplateCharacterViewModel>();
        }
    }
}
