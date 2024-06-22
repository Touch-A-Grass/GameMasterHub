using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Screens.CreateTemplateCharacter;
using GameMasterHub.Screens.Home;
using GameMasterHub.ViewModels;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace GameMasterHub.Screens.TemplatesCharacters
{
    public class TemplatesCharactersViewModel : ViewModelBase
    {
        private readonly HomeViewModel? _homeViewModel;
        private readonly GameRepository _gameRepository;

        private ObservableCollection<TemplateCharacterTemplate>? _templates;
        public ObservableCollection<TemplateCharacterTemplate>? Templates
        {
            get => _templates;
            set => this.RaiseAndSetIfChanged(ref _templates, value);
        }

        public ReactiveCommand<Unit, Unit> NavigateToCreateTemplateCharacterCommand { get; }
        public ReactiveCommand<TemplateCharacterTemplate, Unit> ShowDetailsCommand { get; set; }

        public TemplatesCharactersViewModel(HomeViewModel homeViewModel, GameRepository gameRepository)
        {
            _homeViewModel = homeViewModel;
            _gameRepository = gameRepository;

            Templates = new ObservableCollection<TemplateCharacterTemplate>();

            NavigateToCreateTemplateCharacterCommand = ReactiveCommand.Create(NavigateToCreateTemplateCharacter);
            ShowDetailsCommand = ReactiveCommand.Create<TemplateCharacterTemplate>(ShowDetailsToConsole);

            GetTemplatesCharacters();
        }

        private async void GetTemplatesCharacters()
        {
            var templates = await _gameRepository.GetTemplatesCharactersAsync();

            Templates?.Clear();

            foreach (var template in templates)
            {
                Templates?.Add(new TemplateCharacterTemplate
                {
                    Id = template.Id,
                    Name = template.Name,
                    Attributes = string.Join(", ", template.Attributes.ToArray()),
                    Skills = string.Join(", ", template.Skills.ToArray())
                });
            }
        }

        public void NavigateToCreateTemplateCharacter()
        {
            _homeViewModel?.NavigateToCreateTemplateCharacter();
        }

        private void ShowDetailsToConsole(TemplateCharacterTemplate template)
        {
            Console.WriteLine($"Clicked on item: {template.Name} {template.Id}");
            Console.WriteLine($"Attributes: {template.Attributes}");
            Console.WriteLine($"Skills: {template.Skills}");
        }
    }

    public class TemplateCharacterTemplate : ReactiveObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _attributes = "";
        public string Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }

        private string _skills = "";
        public string Skills
        {
            get => _skills;
            set => this.RaiseAndSetIfChanged(ref _skills, value);
        }
    }
}
