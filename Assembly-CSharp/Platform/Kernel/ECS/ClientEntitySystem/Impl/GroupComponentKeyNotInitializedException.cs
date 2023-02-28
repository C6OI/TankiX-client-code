using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class GroupComponentKeyNotInitializedException : Exception {
        public GroupComponentKeyNotInitializedException(GroupComponent groupComponent)
            : base("groupComponent=" + groupComponent.GetType()) { }
    }
}