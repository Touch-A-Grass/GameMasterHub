using GameMasterHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace GameMasterHub.Infrastructure.Storage
{
    public class GameStorage
    {
        private readonly BehaviorSubject<TemplateCharacterModel> _templateCharacterDataSubject =
            new BehaviorSubject<TemplateCharacterModel>(new TemplateCharacterModel());

        private int _gameId;

        public int GameId
        {
            get => _gameId;
            set => _gameId = value;
        }

        public IObservable<TemplateCharacterModel> Watch()
        {
            return _templateCharacterDataSubject.AsObservable();
        }

        public void Set(TemplateCharacterModel model)
        {
            _templateCharacterDataSubject.OnNext(model);
        }

        public void Clear()
        {
            _templateCharacterDataSubject.OnNext(new TemplateCharacterModel());
        }
    }
}
