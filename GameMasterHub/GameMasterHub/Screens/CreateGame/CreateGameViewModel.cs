using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Screens.Home;
using GameMasterHub.Screens.TemplatesCharacters;
using GameMasterHub.ViewModels;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace GameMasterHub.Screens.CreateGame
{
    public class CreateGameViewModel : ViewModelBase
    {
        private readonly HomeViewModel? _homeViewModel;
        private readonly GameRepository _gameRepository;
        
        private string _gameTitle = "";
        public string GameTitle
        {
            get => _gameTitle;
            set => this.RaiseAndSetIfChanged(ref _gameTitle, value);
        }
        
        public ReactiveCommand<Unit, Unit> NavigateToCreateTemplateCharacterCommand { get; }



        public CreateGameViewModel(HomeViewModel homeViewModel, GameRepository gameRepository)
        {
            _homeViewModel = homeViewModel;
            _gameRepository = gameRepository;

            var canContinue = this.WhenAnyValue(x => x.GameTitle, title => !string.IsNullOrWhiteSpace(title));

            NavigateToCreateTemplateCharacterCommand = ReactiveCommand.Create(NavigateToCreateTemplateCharacter, canContinue);
        }

        private void NavigateToCreateTemplateCharacter()
        {
            _homeViewModel?.NavigateToCreateTemplateCharacter();
        }
    }
}
