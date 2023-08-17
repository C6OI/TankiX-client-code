using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class GroupNotRegisteredException : Exception {
        public GroupNotRegisteredException(Type componentClass)
            : base(componentClass.FullName) { }
    }
}