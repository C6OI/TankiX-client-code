using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    [SerialVersionUID(1431504291271L)]
    public class AddChildGroupToGroupEvent : Event {
        public AddChildGroupToGroupEvent(GroupComponent groupComponent, Type childGroupClass) {
            Group = groupComponent;
            ChildGroupClass = childGroupClass;
        }

        public GroupComponent Group { get; }

        public Type ChildGroupClass { get; }
    }
}