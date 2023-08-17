using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ComponentBitIdRegistry {
        int GetComponentBitId(Type componentClass);
    }
}