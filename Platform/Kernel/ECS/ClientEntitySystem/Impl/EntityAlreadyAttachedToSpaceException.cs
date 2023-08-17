using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityAlreadyAttachedToSpaceException : Exception {
        public EntityAlreadyAttachedToSpaceException(Entity entity)
            : base("entity=" + entity) { }

        public EntityAlreadyAttachedToSpaceException(EntityInternal entity, GroupComponent group)
            : base(string.Concat("entity=", entity, " exists in attached group=", group)) { }
    }
}