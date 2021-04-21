using System;
using Order.Client.Constants;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IStateStore : ISingletonService
    {
        event EventHandler<StoreUpdateArgs> OnUpdate;
        T Get<T>(StoreKey key);
        void Set<T>(StoreKey key, T obj);
    }
}
