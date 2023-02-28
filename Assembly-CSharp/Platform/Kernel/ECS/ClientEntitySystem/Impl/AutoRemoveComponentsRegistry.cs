using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface AutoRemoveComponentsRegistry {
        bool IsComponentAutoRemoved(Type componentType);
    }
}