using GameMasterHub.Infrastructure.Repositories;
using GameMasterHub.Screens.Auth;
using GameMasterHub.Screens.Home;
using ReactiveUI;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;

namespace GameMasterHub.Screens.MainView
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        private readonly AuthRepository _authRepository;
        private readonly GameRepository _gameRepository;
        public RoutingState Router { get; } = new RoutingState();

        public MainViewModel(AuthRepository authRepository, GameRepository gameRepository, HttpClient _httpClient)
        {
            _authRepository = authRepository;
            _gameRepository = gameRepository;
            Router.Navigate.Execute(new AuthViewModel(this, _authRepository));
            _authRepository.WatchToken().Subscribe(token =>
                {
                    if (token == null && Router.NavigationStack.Last() is not AuthViewModel)
                    {
                        NavigateToAuth();
                    }
                    else if (token != null && Router.NavigationStack.Last() is AuthViewModel)
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            return Router.Navigate.Execute(new HomeViewModel(this, _authRepository, _gameRepository));
        }

        public ReactiveCommand<Unit, IRoutableViewModel> ClickedNavigateToAuth =>
            ReactiveCommand.CreateFromObservable(NavigateToAuth);

        public ReactiveCommand<Unit, IRoutableViewModel> ClickedNavigateToHome =>
            ReactiveCommand.CreateFromObservable(NavigateToHome);
    }
}
