using GameMasterHub.Domain.Models;
using GameMasterHub.Infrastructure.Managers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace GameMasterHub.Infrastructure.Storage
{
    public class LobbyStorage
    {
        private readonly BehaviorSubject<string?> _lobbyCode = new BehaviorSubject<string?>(null);

        public IObservable<string?> WatchLobbyCode() => _lobbyCode.AsObservable();
        public void SetLobbyCode(string? token) => _lobbyCode.OnNext(token);
        public void ClearLobbyCode() => _lobbyCode.OnNext(null);
        public string? GetLobbyCode() => _lobbyCode.Value;
        

        private readonly BehaviorSubject<List<UserModel>> _users = new BehaviorSubject<List<UserModel>>(new List<UserModel>());

        public IObservable<List<UserModel>> WatchUsers() => _users.AsObservable();

        public void AddUser(UserModel user)
        {
            var currentUsers = _users.Value;
            currentUsers.Add(user);
            _users.OnNext(currentUsers);
        }

        public void RemoveUser(UserModel user)
        {
            var currentUsers = _users.Value;
            currentUsers.Remove(user);
            _users.OnNext(currentUsers);
        }

        public void ClearUsers() => _users.OnNext(new List<UserModel>());

        public List<UserModel> GetUsers() => _users.Value;
    }
}
