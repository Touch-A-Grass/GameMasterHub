using GameMasterHub.ViewModels;
using ReactiveUI;

namespace GameMasterHub.Screens.Auth
{
    public class AuthViewModel(IScreen screen) : ViewModelBase, IRoutableViewModel
    {

        public string? UrlPathSegment { get; } = "auth";
        public IScreen HostScreen { get; } = screen;

    }
}
