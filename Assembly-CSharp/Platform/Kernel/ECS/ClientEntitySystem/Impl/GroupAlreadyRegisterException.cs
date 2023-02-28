using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class GroupAlreadyRegisterException : Exception {
        public GroupAlreadyRegisterException(Type componentClass)
            : base(componentClass.FullName) { }
    }
}