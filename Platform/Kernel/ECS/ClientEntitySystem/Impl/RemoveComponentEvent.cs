using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    [SerialVersionUID(1429778419932L)]
    public class RemoveComponentEvent : ComponentEvent {
        public RemoveComponentEvent(Type componentType)
            : base(componentType) { }
    }
}