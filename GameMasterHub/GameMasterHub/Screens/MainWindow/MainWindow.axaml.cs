using Avalonia;
using Avalonia.ReactiveUI;
using GameMasterHub.Screens.MainView;
using GameMasterHub.ViewModels;
using ReactiveUI;

namespace GameMasterHub.Views
{
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
    }
}
