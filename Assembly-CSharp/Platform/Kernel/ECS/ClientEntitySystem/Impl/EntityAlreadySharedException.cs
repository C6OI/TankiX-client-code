using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityAlreadySharedException : Exception {
        public EntityAlreadySharedException(Entity entity)
            : base("entity=" + entity) { }
    }
}