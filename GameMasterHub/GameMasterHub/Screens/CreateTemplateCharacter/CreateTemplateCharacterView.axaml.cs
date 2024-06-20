using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GameMasterHub.Screens.CreateLobby;
using System.Collections.ObjectModel;

namespace GameMasterHub.Screens.CreateTemplateCharacter
{
    public partial class CreateTemplateCharacterView : ReactiveUserControl<CreateTemplateCharacterViewModel>
    {
        public CreateTemplateCharacterView()
        {
            InitializeComponent();
        }
    }
}
