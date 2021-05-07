using System;
using Order.Client.Constants;
using Order.Shared.Contracts;
using Order.Shared.Dto;

namespace Order.Client.Services
{
    public interface IStateStore : ISingletonService
    {
        event EventHandler<StoreUpdateArgs> OnUpdate;
        T Get<T>(StoreKey key) where T : ICloneable<T>;
        void Set<T>(StoreKey key, T obj) where T : ICloneable<T>;
    }
}
