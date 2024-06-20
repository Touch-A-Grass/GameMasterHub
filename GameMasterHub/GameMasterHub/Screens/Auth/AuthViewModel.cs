using Avalonia.Controls;
using GameMasterHub.Infrastructure.Repositories;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GameMasterHub.Screens.Auth
{
    public class AuthViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly AuthRepository _authRepository;

        public string? UrlPathSegment => "auth";
        public IScreen HostScreen { get; }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

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

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        public AuthViewModel(IScreen screen, AuthRepository authRepository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));

            var canLogin = this.WhenAnyValue(
                x => x.IsLoading,
                x => x.Username,
                x => x.Password,
                (isLoading, username, password) => !isLoading && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)
            );

            LoginCommand = ReactiveCommand.CreateFromTask(Login, canLogin);
            RegisterCommand = ReactiveCommand.CreateFromTask(Register, canLogin);
        }

        private async Task Login()
        {
            try
            {
                IsLoading = true;
                await _authRepository.LoginAsync(Username, Password);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task Register()
        {
            try
            {
                IsLoading = true;
                await _authRepository.RegisterAsync(Username, Password);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
