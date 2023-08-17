using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    [SerialVersionUID(1431504312177L)]
    public class RemoveChildGroupFromGroupEvent : Event {
        public RemoveChildGroupFromGroupEvent(GroupComponent groupComponent, Type childGroupClass) {
            Group = groupComponent;
            ChildGroupClass = childGroupClass;
        }

        public GroupComponent Group { get; }

        public Type ChildGroupClass { get; }
    }
}