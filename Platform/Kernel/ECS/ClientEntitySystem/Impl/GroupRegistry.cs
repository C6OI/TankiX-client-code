using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface GroupRegistry {
        T FindOrCreateGroup<T>(long key) where T : GroupComponent;

        GroupComponent FindOrCreateGroup(Type groupClass, long key);

        T FindOrRegisterGroup<T>(T groupComponent) where T : GroupComponent;

        GroupComponent FindOrRegisterGroup(GroupComponent groupComponent);
    }
}