using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.ReactiveUI;
using GameMasterHub.Screens.CreateLobby;
using System.IO;

namespace GameMasterHub.Screens.Lobby
{
    public partial class LobbyView : ReactiveUserControl<LobbyViewModel>
    {
        public LobbyView()
        {
            InitializeComponent();
        }
    }
}

