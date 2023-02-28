using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityNotExistsException : Exception {
        public EntityNotExistsException(long id)
            : base("id=" + id) { }
    }
}