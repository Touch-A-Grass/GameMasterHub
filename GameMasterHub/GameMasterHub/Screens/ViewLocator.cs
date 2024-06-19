using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GameMasterHub.Screens.Auth;
using GameMasterHub.Screens.CreateLobby;
using GameMasterHub.Screens.Home;
using GameMasterHub.ViewModels;
using ReactiveUI;

namespace GameMasterHub;

public class ViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        AuthViewModel context => new AuthView { DataContext = context },
        HomeViewModel context => new HomeView { DataContext = context },
        CreateLobbyViewModel context => new CreateLobbyView {DataContext = context},
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}