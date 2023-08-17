using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization.Utilities {
    public sealed class SerializerState : IDisposable {
        readonly IDictionary<Type, object> items = new Dictionary<Type, object>();

        public void Dispose() {
            foreach (IDisposable item in items.Values.OfType<IDisposable>()) {
                item.Dispose();
            }
        }

        public T Get<T>() where T : class, new() {
            object value;

            if (!items.TryGetValue(typeof(T), out value)) {
                value = new T();
                items.Add(typeof(T), value);
            }

            return (T)value;
        }

        public void OnDeserialization() {
            foreach (IPostDeserializationCallback item in items.Values.OfType<IPostDeserializationCallback>()) {
                item.OnDeserialization();
            }
        }
    }
}