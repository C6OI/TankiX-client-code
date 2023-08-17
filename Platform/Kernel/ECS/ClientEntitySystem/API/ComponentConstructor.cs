using System;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ComponentConstructor {
        bool IsAcceptable(Type componentType, EntityInternal entity);

        Component GetComponentInstance(Type componentType, EntityInternal entity);
    }
}