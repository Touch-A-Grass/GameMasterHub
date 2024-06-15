using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Screens.Auth;
using GameMasterHub.Screens.Home;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;

namespace GameMasterHub.Screens.MainView
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        private readonly AuthRepository _authRepository;
        public RoutingState Router { get; } = new RoutingState();

        public MainViewModel(AuthRepository authRepository)
        {
            _authRepository = authRepository;
            Router.Navigate.Execute(new AuthViewModel(this, _authRepository));
            _authRepository.WatchToken().Subscribe(token =>
                {
                    if (token == null && Router.NavigationStack.Last() is not AuthViewModel)
                    {
                        NavigateToAuth();
                    }
                    else if (token != null && Router.NavigationStack.Last() is AuthViewModel)
                    {
                        NavigateToHome();
                    }
                    
                });
        }

        private IObservable<IRoutableViewModel> NavigateToAuth()
        {
            return Router.Navigate.Execute(new AuthViewModel(this, _authRepository));
        }
        
        private IObservable<IRoutableViewModel> NavigateToHome()
        {
            return Router.Navigate.Execute(new HomeViewModel(this));
        }

        public ReactiveCommand<Unit, IRoutableViewModel> ClickedNavigateToAuth =>
            ReactiveCommand.CreateFromObservable(NavigateToAuth);

        public ReactiveCommand<Unit, IRoutableViewModel> ClickedNavigateToHome =>
            ReactiveCommand.CreateFromObservable(NavigateToHome);
    }
}
