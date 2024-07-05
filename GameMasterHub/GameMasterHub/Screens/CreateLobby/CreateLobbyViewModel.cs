using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Screens.Home;
using GameMasterHub.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace GameMasterHub.Screens.CreateLobby
{
    public class CreateLobbyViewModel : ViewModelBase
    {
        private readonly HomeViewModel? _homeViewModel;
        private readonly LobbyRepository _lobbyRepository;
        
        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public ReactiveCommand<Unit, Unit> NavigateToLobbyCommand { get; }

        public CreateLobbyViewModel(HomeViewModel homeViewModel, LobbyRepository lobbyRepository)
        {
            _lobbyRepository = lobbyRepository;
            _homeViewModel = homeViewModel;

            NavigateToLobbyCommand = ReactiveCommand.Create(NavigateToLobby);
        }
        private async void NavigateToLobby()
        {
            IsLoading = true;
            if (await _lobbyRepository.CreateLobby())
            {
                _homeViewModel?.NavigateToLobby();
            }
            else
            {
                IsLoading = false;
            }
        }
    }
}
