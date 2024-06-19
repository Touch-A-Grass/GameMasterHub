using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Splat;

namespace GameMasterHub.Screens.Home
{
    public partial class HomeView : ReactiveUserControl<HomeViewModel>
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void PaneVisiblaChangedClickHandler(object? sender, RoutedEventArgs e)
        {
            ViewModel.IsPaneOpen = !ViewModel.IsPaneOpen;
        }
    }
}

