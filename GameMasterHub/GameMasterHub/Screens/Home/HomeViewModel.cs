using GameMasterHub.Screens.CreateLobby;
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
        
        public HomeViewModel(IScreen screen)
        {
            CurrentView = new CreateLobbyViewModel();
        }
        
    }
}
