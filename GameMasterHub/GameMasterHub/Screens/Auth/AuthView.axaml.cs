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
    }
}

