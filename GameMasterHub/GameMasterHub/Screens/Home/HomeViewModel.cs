using GameMasterHub.Infrastructure.Managers;
using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Infrastructure.Storage;
using GameMasterHub.Screens.CreateLobby;
using GameMasterHub.Screens.CreateTemplateCharacter;
using GameMasterHub.Screens.MainView;
using GameMasterHub.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive;

namespace GameMasterHub.Screens.Home
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = "home";
        public IScreen HostScreen { get; }
        private readonly AuthRepository _authRepository;


        public RoutingState Router { get; } = new RoutingState();

        private bool _isPaneOpen = false;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
        }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public HomeViewModel(IScreen screen, AuthRepository authRepository)
        {
            _authRepository = authRepository;
            SwitchCurrentViewModel("CreateLobby");
        }

        public void OpenCreateTemplateCharacterPanel()
        {
            CurrentView = new CreateTemplateCharacterViewModel();
        }

        public void SwitchCurrentViewModel(string tag)
        {
            switch (tag)
            {
                case "CreateLobby":
                    CurrentView = new CreateLobbyViewModel();
                    break;
                case "CreateCharacterTemplate":
                    CurrentView = new CreateTemplateCharacterViewModel();
                    break;
                default:
                    CurrentView = new CreateLobbyViewModel();
                    break;
            }
        }

        public void Logout()
        {
            _authRepository.Logout();
        }
    }
}
