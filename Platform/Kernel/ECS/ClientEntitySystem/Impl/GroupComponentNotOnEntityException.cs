using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class GroupComponentNotOnEntityException : Exception {
        public GroupComponentNotOnEntityException(Type componentType)
            : base("componentType=" + componentType) { }
    }
}