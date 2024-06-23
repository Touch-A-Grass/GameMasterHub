using GameMasterHub.Infrastructure.Managers;
using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Infrastructure.Storage;
using GameMasterHub.Screens.CreateGame;
using GameMasterHub.Screens.CreateLobby;
using GameMasterHub.Screens.CreateTemplateCharacter;
using GameMasterHub.Screens.MainView;
using GameMasterHub.Screens.TemplatesCharacters;
using GameMasterHub.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;

namespace GameMasterHub.Screens.Home
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = "home";
        public IScreen HostScreen { get; }
        private readonly AuthRepository _authRepository;

        private Stack<ViewModelBase> _stackViewModels = new Stack<ViewModelBase>();

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
            set
            {
                _stackViewModels.Push(value);
                this.RaiseAndSetIfChanged(ref _currentView, value);
                UpdateBackButtonVisibility();
            }
        }

        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        private bool _backButtonVisible = false;
        private readonly GameRepository _gameRepository;
        public bool BackButtonVisible
        {
            get => _backButtonVisible;
            set => this.RaiseAndSetIfChanged(ref _backButtonVisible, value);
        }

        public HomeViewModel(IScreen screen, AuthRepository authRepository, GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            _authRepository = authRepository;
            SwitchCurrentViewModel("CreateLobby");

            BackCommand = ReactiveCommand.Create(Back);
        }

        public void SwitchCurrentViewModel(string tag)
        {
            _stackViewModels.Clear();
            BackButtonVisible = false;
            ViewModelBase viewModel;
            switch (tag)
            {
                case "CreateLobby":
                    viewModel = new CreateLobbyViewModel();
                    break;
                case "CreateGame":
                    // viewModel = new TemplatesCharactersViewModel(this, _gameRepository);
                    viewModel = new CreateGameViewModel(this, _gameRepository);
                    break;
                default:
                    viewModel = new CreateLobbyViewModel();
                    break;
            }
            CurrentView = viewModel;
        }

        public void NavigateToCreateTemplateCharacter()
        {
            CurrentView = new CreateTemplateCharacterViewModel(_gameRepository);
            UpdateBackButtonVisibility();
        }

        public void Back()
        {
            if (_stackViewModels.Count > 1)
            {
                _stackViewModels.Pop();
                CurrentView = _stackViewModels.Peek();
            }

            UpdateBackButtonVisibility();
        }

        public void Logout()
        {
            _authRepository.Logout();
        }

        private void UpdateBackButtonVisibility()
        {
            var noBackButtonViews = new List<Type>
            {
                typeof(CreateLobbyViewModel),
                typeof(TemplatesCharactersViewModel)
            };

            BackButtonVisible = _stackViewModels.Count > 1 && !noBackButtonViews.Contains(CurrentView.GetType());
        }
    }
}
