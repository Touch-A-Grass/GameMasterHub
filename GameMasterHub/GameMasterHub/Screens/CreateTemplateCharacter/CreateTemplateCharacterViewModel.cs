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

        private ObservableCollection<AttributeModel>? _attributes;
        public ObservableCollection<AttributeModel>? Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }

        private ObservableCollection<SkillModel>? _skills;
        public ObservableCollection<SkillModel>? Skills
        {
            get => _skills;
            set => this.RaiseAndSetIfChanged(ref _skills, value);
        }

        public ReactiveCommand<Unit, Unit> AddAttributeCommand { get; }
        public ReactiveCommand<Unit, Unit> AddSkillCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public CreateTemplateCharacterViewModel()
        {
            Attributes = new ObservableCollection<AttributeModel>();
            Skills = new ObservableCollection<SkillModel>();

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
            Attributes?.Add(new AttributeModel());
        }

        private void AddSkill()
        {
            Skills?.Add(new SkillModel());
        }
    }

    public class AttributeModel : ReactiveObject
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }

    public class SkillModel : ReactiveObject
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }
}
