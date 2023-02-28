using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityAlreadyUnsharedException : Exception {
        public EntityAlreadyUnsharedException(Entity entity)
            : base("entity=" + entity) { }
    }
}