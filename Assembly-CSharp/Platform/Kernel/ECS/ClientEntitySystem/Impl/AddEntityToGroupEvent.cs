using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class AddEntityToGroupEvent : Event {
        public AddEntityToGroupEvent(GroupComponent group) => Group = group;

        public GroupComponent Group { get; }
    }
}