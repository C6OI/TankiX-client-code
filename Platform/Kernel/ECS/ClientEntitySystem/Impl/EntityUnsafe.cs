using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface EntityUnsafe {
        Component GetComponentUnsafe(Type componentType);
    }
}