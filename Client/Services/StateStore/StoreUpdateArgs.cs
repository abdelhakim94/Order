using System;
using Order.Client.Constants;

namespace Order.Client.Services
{
    public class StoreUpdateArgs : EventArgs
    {
        public StoreKey Key { get; }
        public object Value { get; }

        public StoreUpdateArgs(StoreKey key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
