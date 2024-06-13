using ReactiveUI;
using System.Reactive;
using GameMasterHub.Screens.Auth;
using GameMasterHub.Screens.Home;

namespace GameMasterHub.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        public MainViewModel()
        {
            Router.Navigate.Execute(new HomeViewModel(this));
        }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToAuth =>
            ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new AuthViewModel(this)));

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToHome =>
            ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new HomeViewModel(this)));
    }
}
