using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [SerialVersionUID(1429857568723L)]
    public class ChangeComponentEvent : ComponentEvent {
        public ChangeComponentEvent(Component component)
            : base(component.GetType()) => Component = component;

        public Component Component { get; set; }
    }
}