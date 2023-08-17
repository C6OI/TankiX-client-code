using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityByIdNotFoundException : Exception {
        public EntityByIdNotFoundException(long id)
            : base("id=" + id) { }
    }
}