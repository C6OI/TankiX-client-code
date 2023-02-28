using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public abstract class ComponentEvent : Event {
        protected ComponentEvent(Type componentType) => ComponentType = componentType;

        public Type ComponentType { get; }
    }
}