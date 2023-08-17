using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    [SerialVersionUID(1436427042268L)]
    public class ComponentAddedEvent : Event {
        public ComponentAddedEvent(Component component) => Component = component;

        public Component Component { get; private set; }
    }
}