using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityAlreadyRegisteredException : Exception {
        public EntityAlreadyRegisteredException(Entity newEntity)
            : base("entity=" + newEntity) { }
    }
}