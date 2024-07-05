using GameMasterHub.Domain.Models;
using GameMasterHub.Infrastructure.Storage;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GameMasterHub.Infrastructure.Repositories
{
    public class LobbyRepository(HttpClient httpClient, LobbyStorage lobbyStorage)
    {
        private readonly HttpClient _httpClient = httpClient;

        // private readonly HubConnection _hubConnection = hubConnection;
        private readonly LobbyStorage _lobbyStorage = lobbyStorage;

        async public Task<bool> CreateLobby()
        {
            string lobbyCode = "8J1U43";

            await Task.Delay(2000);
            lobbyStorage.SetLobbyCode(lobbyCode);

            return true;
        }

        public string GetLobbyToken()
        {
            return lobbyStorage.GetLobbyCode();
        }

        public IObservable<List<UserModel>> WatchUsers()
        {
            return _lobbyStorage.WatchUsers();
        }

        async public Task GenerateUsers()
        {
            await Task.Delay(5000);
            var random = new Random();
            var user = new UserModel
            {
                Username = "krushiler"
            };

            _lobbyStorage.AddUser(user);
        }

    }
}
