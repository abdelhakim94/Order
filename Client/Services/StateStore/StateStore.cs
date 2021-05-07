using System;
using System.Collections.Generic;
using Order.Client.Constants;
using Order.Shared.Contracts;
using Order.Shared.Dto;

namespace Order.Client.Services
{
    public class StateStore : IStateStore, ISingletonService
    {
        private readonly Dictionary<StoreKey, object> store;
        public event EventHandler<StoreUpdateArgs> OnUpdate;

        public StateStore(IHubConnectionService hubConnection)
        {
            store = new();
        }

        public T Get<T>(StoreKey key) where T : ICloneable<T>
        {
            if (store.ContainsKey(key) && store[key] is T value)
            {
                return value.Clone();
            }
            return default(T);
        }

        public void Set<T>(StoreKey key, T obj) where T : ICloneable<T>
        {
            if (store.ContainsKey(key))
            {
                store[key] = obj.Clone();
            }
            else
            {
                store.Add(key, obj.Clone());
            }
            OnUpdate?.Invoke(this, new(key, obj.Clone()));
        }
    }
}
