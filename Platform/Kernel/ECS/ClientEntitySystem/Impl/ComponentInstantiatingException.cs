using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentInstantiatingException : Exception {
        public ComponentInstantiatingException(Type componentClass)
            : base(componentClass.FullName) { }
    }
}