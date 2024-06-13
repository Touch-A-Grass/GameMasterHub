using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace GameMasterHub.Screens.Home
{
    public partial class HomeView : ReactiveUserControl<HomeViewModel>
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}

