using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace GameMasterHub.Screens.CreateGame
{
    public partial class CreateGameView : ReactiveUserControl<CreateGameViewModel>
    {
        public CreateGameView()
        {
            InitializeComponent();
        }
    }
}

