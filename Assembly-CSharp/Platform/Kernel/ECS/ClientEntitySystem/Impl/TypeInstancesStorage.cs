using System;
using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class TypeInstancesStorage<T> {
        static readonly Dictionary<Type, T> Storage = new();

        public void AddInstance(Type type) {
            if (!Storage.ContainsKey(type)) {
                Storage.Add(type, (T)Activator.CreateInstance(type));
            }
        }

        public bool HasInstance(Type type) => Storage.ContainsKey(type);

        public bool TryGetInstance(Type type, out T instance) => Storage.TryGetValue(type, out instance);
    }
}