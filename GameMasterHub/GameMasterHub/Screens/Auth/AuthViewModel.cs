using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Infrastructure.Storage;
using GameMasterHub.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace GameMasterHub.Screens.Auth
{
    public class AuthViewModel(IScreen screen, AuthRepository authRepository) : ViewModelBase, IRoutableViewModel
    {
        private readonly AuthRepository _authRepository = authRepository;

        public string? UrlPathSegment => "auth";
        public IScreen HostScreen { get; } = screen;

        private string _username = "";
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        async public void Login()
        {
            await _authRepository.LoginAsync(_username, _password);
        }

        async public void Register()
        {
            await _authRepository.RegisterAsync(_username, _password);
        }
    }
}
