using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentAlreadyExistsInEntityException : Exception {
        public ComponentAlreadyExistsInEntityException(EntityInternal entity, Type componentClass)
            : base(string.Format("{0} entity={1}", componentClass.Name, entity)) { }
    }
}