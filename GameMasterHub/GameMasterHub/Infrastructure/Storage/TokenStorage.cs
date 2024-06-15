using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace GameMasterHub.Infrastructure.Storage
{
    public class TokenStorage
    {
        private readonly BehaviorSubject<string?> _dataSubject = new BehaviorSubject<string?>(null);

        public IObservable<string?> Watch()
        {
            return _dataSubject.AsObservable();
        }

        public void Set(string? token)
        {
            _dataSubject.OnNext(token);
        }

        public void Clear()
        {
            _dataSubject.OnNext(null);
        }

    }
}
