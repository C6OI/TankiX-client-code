using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class CanNotRemoveGroupComponentFromRootGroupEntityForNotEmptyGroupException : Exception {
        public CanNotRemoveGroupComponentFromRootGroupEntityForNotEmptyGroupException(Type groupClass, Entity entity)
            : base("group=" + groupClass.FullName + " entity=" + entity) { }
    }
}