using GameMasterHub.ViewModels;
using ReactiveUI;

namespace GameMasterHub.Screens.Home
{
    public class HomeViewModel(IScreen screen) : ViewModelBase, IRoutableViewModel
    {

        public string? UrlPathSegment { get; } = "home";
        public IScreen HostScreen { get; } = screen;
    }
}
