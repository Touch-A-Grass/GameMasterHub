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

        private void PaneVisibleChangedClickHandler(object? sender, RoutedEventArgs e)
        {
            ViewModel.IsPaneOpen = !ViewModel.IsPaneOpen;
        }

        private void NavigationList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (DataContext is HomeViewModel viewModel && sender is ListBox listBox)
            {
                if (listBox.SelectedItem is ListBoxItem selectedItem && selectedItem.Tag is string tag)
                {
                    viewModel.SwitchCurrentViewModel(tag);
                }
            }
        }

        private void LogoutClickHandler(object? sender, RoutedEventArgs e)
        {
            ViewModel.Logout();
        }
    }
}

