using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace GameMasterHub.Screens.Auth
{
    public partial class AuthView : ReactiveUserControl<AuthViewModel>
    {
        public AuthView()
        {
            InitializeComponent();
        }

        private void LoginClickHandler(object? sender, RoutedEventArgs e)
        {
            ViewModel?.Login();
        }

        private void RegisterClickHandler(object? sender, RoutedEventArgs e)
        {
            ViewModel?.Register();
        }
    }
}

