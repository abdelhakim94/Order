using System;
using System.Collections.Generic;
using Order.Client.Constants;
using Order.Shared.Contracts;

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

        public T Get<T>(StoreKey key)
        {
            if (store.ContainsKey(key) && store[key] is T value)
            {
                return value;
            }
            return default(T);
        }

        public void Set<T>(StoreKey key, T obj)
        {
            if (store.ContainsKey(key))
            {
                store[key] = obj;
            }
            else
            {
                store.Add(key, obj);
            }
            OnUpdate?.Invoke(this, new(key, obj));
        }
    }
}
