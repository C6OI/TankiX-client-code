using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class GroupOrganizerNotFoundException : Exception {
        public GroupOrganizerNotFoundException(Type componentClass)
            : base(componentClass.FullName) { }
    }
}