using DynamicData;
using GameMasterHub.Domain.Models;
using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Infrastructure.Storage;
using GameMasterHub.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;

namespace GameMasterHub.Screens.CreateTemplateCharacter
{
    public class CreateTemplateCharacterViewModel : ViewModelBase
    {
        private string _walletValue = "0";
        public string WalletValue
        {
            get => _walletValue;
            set => this.RaiseAndSetIfChanged(ref _walletValue, value);
        }

        private ObservableCollection<AttributeTemplate> _attributes = new ObservableCollection<AttributeTemplate>();
        public ObservableCollection<AttributeTemplate> Attributes
        {
            get => _attributes;
            set
            {
                if (_attributes != null)
                {
                    _attributes.CollectionChanged -= AttributesCollectionChanged;
                    UnsubscribeFromAttributes(_attributes); 
                }

                this.RaiseAndSetIfChanged(ref _attributes, value);

                if (_attributes != null)
                {
                    _attributes.CollectionChanged += AttributesCollectionChanged;
                    SubscribeToAttributes(_attributes);
                }

                UpdateIntAttributesForSkills();
            }
        }

        private ObservableCollection<AttributeTemplate> _intAttributesForSkills = new ObservableCollection<AttributeTemplate>();
        public ObservableCollection<AttributeTemplate> IntAttributesForSkills
        {
            get => _intAttributesForSkills;
            private set => this.RaiseAndSetIfChanged(ref _intAttributesForSkills, value);
        }

        private void AttributesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (AttributeTemplate item in e.NewItems)
                {
                    item.PropertyChanged += AttributePropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (AttributeTemplate item in e.OldItems)
                {
                    item.PropertyChanged -= AttributePropertyChanged;
                }
            }

            UpdateIntAttributesForSkills();
        }

        private void AttributePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AttributeTemplate.Type))
            {
                UpdateIntAttributesForSkills();
            }
        }

        private void UpdateIntAttributesForSkills()
        {
            var selectedAttributes = Skills?.ToDictionary(skill => skill, skill => skill.Attribute);

            var attributesToAdd = _attributes.Where(a => a.Type == "int" && !_intAttributesForSkills.Contains(a)).ToList();
            var attributesToRemove = _intAttributesForSkills.Where(a => a.Type != "int" || !_attributes.Contains(a)).ToList();

            foreach (var attribute in attributesToRemove)
            {
                _intAttributesForSkills.Remove(attribute);
            }

            foreach (var attribute in attributesToAdd)
            {
                _intAttributesForSkills.Add(attribute);
            }

            if (selectedAttributes != null)
            {
                foreach (var skill in selectedAttributes.Keys)
                {
                    var selectedAttribute = selectedAttributes[skill];
                    if (selectedAttribute != null && _intAttributesForSkills.Contains(selectedAttribute))
                    {
                        skill.Attribute = selectedAttribute;
                    }
                    else
                    {
                        skill.Attribute = _intAttributesForSkills.FirstOrDefault();
                    }
                }
            }
            WalletValue = Math.Round(_intAttributesForSkills.Count * 4.5).ToString();
        }

        private void SubscribeToAttributes(ObservableCollection<AttributeTemplate> attributes)
        {
            foreach (var attribute in attributes)
            {
                attribute.PropertyChanged += AttributePropertyChanged;
            }
        }

        private void UnsubscribeFromAttributes(ObservableCollection<AttributeTemplate> attributes)
        {
            foreach (var attribute in attributes)
            {
                attribute.PropertyChanged -= AttributePropertyChanged;
            }
        }

        private ObservableCollection<SkillTemplate> _skills = new ObservableCollection<SkillTemplate>();
        private readonly GameRepository _gameRepository;
        public ObservableCollection<SkillTemplate> Skills
        {
            get => _skills;
            set => this.RaiseAndSetIfChanged(ref _skills, value);
        }

        public ReactiveCommand<Unit, Unit> AddAttributeCommand { get; }
        public ReactiveCommand<AttributeTemplate, Unit> DeleteAttributeCommand { get; }
        public ReactiveCommand<Unit, Unit> AddSkillCommand { get; }
        public ReactiveCommand<SkillTemplate, Unit> DeleteSkillCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public CreateTemplateCharacterViewModel(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            AddAttribute();
            AddSkill();

            AddAttributeCommand = ReactiveCommand.Create(AddAttribute);
            DeleteAttributeCommand = ReactiveCommand.Create<AttributeTemplate>(DeleteAttribute);

            AddSkillCommand = ReactiveCommand.Create(AddSkill);
            DeleteSkillCommand = ReactiveCommand.Create<SkillTemplate>(DeleteSkill);

            var canSave = this.WhenAnyValue(
                x => x.Attributes,
                x => x.Skills,
                x => x.WalletValue,
                (attributes, skills, walletValue) =>
                    attributes.Count > 0 &&
                    skills.Count > 0 &&
                    !string.IsNullOrWhiteSpace(walletValue) &&
                    attributes.All(a => !string.IsNullOrWhiteSpace(a.Name) && !string.IsNullOrWhiteSpace(a.Type)) &&
                    skills.All(s => !string.IsNullOrWhiteSpace(s.Name) && s.Attribute != null && !string.IsNullOrWhiteSpace(s.Attribute.Name))
            );

            SaveCommand = ReactiveCommand.Create(Save, canSave);

            _attributes.CollectionChanged += AttributesCollectionChanged;
            SubscribeToAttributes(_attributes);
            
            _gameRepository.WatchGame().Subscribe(template =>
                {
                    if (template.Id is not null)
                    {
                        Console.WriteLine("New View");
                    }
                });

            UpdateIntAttributesForSkills();
        }

        public async void Save()
        {
            Console.WriteLine("Attributes: " + string.Join(", ", Attributes!.Select(a => a.Name)));
            foreach (var attr in IntAttributesForSkills)
            {
                Console.WriteLine($"Name {attr.Name}, Type {attr.Type}");
            }
            Console.WriteLine("Skills: " + string.Join(", ", Skills!.Select(s => s.Name)));

            TemplateCharacterModel template = new TemplateCharacterModel();
            template.GameId = _gameRepository.GetGameId();
            template.AttributesWallet = Convert.ToInt32(WalletValue);

            foreach (var attr in Attributes)
            {
                template.Attributes.Add(new AttributeModel()
                {
                    Name = attr.Name,
                    Type = attr.Type
                });
            }

            foreach (var skill in Skills)
            {
                var attributeModel = template.Attributes.FirstOrDefault(a => a.Name == skill.Attribute.Name);
                template.Skills.Add(new SkillModel
                {
                    Name = skill.Name,
                    Attribute = attributeModel
                });
            }

            if (await _gameRepository.CreateTemplateCharacterAsync(template))
            {
                Console.WriteLine("New View");
                
            }
            else
            {
                Console.WriteLine("Tilt");
            }
        }

        private void AddAttribute()
        {
            Attributes?.Add(new AttributeTemplate());
        }

        private void DeleteAttribute(AttributeTemplate attribute)
        {
            Attributes?.Remove(attribute);
        }

        private void AddSkill()
        {
            Skills?.Add(new SkillTemplate());
        }

        private void DeleteSkill(SkillTemplate skill)
        {
            Skills?.Remove(skill);
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

        private string _type = "";
        public string Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        private List<string> _availableTypes;
        public List<string> AvailableTypes
        {
            get => _availableTypes;
            set => this.RaiseAndSetIfChanged(ref _availableTypes, value);
        }

        public AttributeTemplate()
        {
            AvailableTypes = new List<string>
            {
                "int",
                "bool",
                "string"
            };
            Type = AvailableTypes[0];
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

        private AttributeTemplate _attribute;
        public AttributeTemplate Attribute
        {
            get => _attribute;
            set => this.RaiseAndSetIfChanged(ref _attribute, value);
        }
    }
}
