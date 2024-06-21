using GameMasterHub.ViewModels;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace GameMasterHub.Screens.CreateTemplateCharacter
{
    public class CreateTemplateCharacterViewModel : ViewModelBase
    {
        private string _characterName = "";
        public string CharacterName
        {
            get => _characterName;
            set => this.RaiseAndSetIfChanged(ref _characterName, value);
        }

        private ObservableCollection<AttributeTemplate>? _attributes;
        public ObservableCollection<AttributeTemplate>? Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }

        private ObservableCollection<SkillTemplate>? _skills;
        public ObservableCollection<SkillTemplate>? Skills
        {
            get => _skills;
            set => this.RaiseAndSetIfChanged(ref _skills, value);
        }

        public ReactiveCommand<Unit, Unit> AddAttributeCommand { get; }
        public ReactiveCommand<Unit, Unit> AddSkillCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public CreateTemplateCharacterViewModel()
        {
            Attributes = new ObservableCollection<AttributeTemplate>();
            Skills = new ObservableCollection<SkillTemplate>();

            AddAttribute();
            AddSkill();

            AddAttributeCommand = ReactiveCommand.Create(AddAttribute);
            AddSkillCommand = ReactiveCommand.Create(AddSkill);
            SaveCommand = ReactiveCommand.Create(Save);
        }

        public void Save()
        {
            Console.WriteLine("Attributes: " + string.Join(", ", Attributes!.Select(a => a.Name)));
            Console.WriteLine("Skills: " + string.Join(", ", Skills!.Select(s => s.Name)));
        }

        private void AddAttribute()
        {
            Attributes?.Add(new AttributeTemplate());
        }

        private void AddSkill()
        {
            Skills?.Add(new SkillTemplate());
        }
    }

    public class AttributeTemplate : ReactiveObject
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }

    public class SkillTemplate : ReactiveObject
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }
}
