using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityNotFoundException : Exception {
        public EntityNotFoundException(long entityId)
            : base("entityId = " + entityId) { }
    }
}