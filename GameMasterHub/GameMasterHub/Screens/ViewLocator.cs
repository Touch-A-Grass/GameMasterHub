using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GameMasterHub.Screens.Auth;
using GameMasterHub.Screens.CreateGame;
using GameMasterHub.Screens.CreateLobby;
using GameMasterHub.Screens.CreateTemplateCharacter;
using GameMasterHub.Screens.Home;
using GameMasterHub.Screens.TemplatesCharacters;
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
        CreateGameViewModel context => new CreateGameView() {DataContext = context},
        TemplatesCharactersViewModel context => new TemplatesCharactersView {DataContext = context},
        CreateTemplateCharacterViewModel context => new CreateTemplateCharacterView {DataContext = context},
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}