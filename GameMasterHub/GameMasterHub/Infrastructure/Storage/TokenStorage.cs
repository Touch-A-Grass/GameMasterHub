using GameMasterHub.Infrastructure.Managers;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace GameMasterHub.Infrastructure.Storage
{
    public class TokenStorage
    {
        private readonly BehaviorSubject<string?> _dataSubject = new BehaviorSubject<string?>(null);

        public TokenStorage()
        {
            var token = ApplicationDataManager.GetAuthToken();
            Set(token);
        }
        public IObservable<string?> Watch()
        {
            return _dataSubject.AsObservable();
        }

        public void Set(string? token)
        {
            ApplicationDataManager.SaveAuthToken(token);
            _dataSubject.OnNext(token);
        }

        public void Clear()
        {
            ApplicationDataManager.RemoveAuthToken();
            _dataSubject.OnNext(null);
        }

    }
}
