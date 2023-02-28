using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    [SerialVersionUID(1446717399705L)]
    public class AutoRemoveComponentsEvent : Event {
        public AutoRemoveComponentsEvent() { }

        public AutoRemoveComponentsEvent(List<Type> componentsToRemove) => ComponentsToRemove = componentsToRemove;

        public List<Type> ComponentsToRemove { get; set; }
    }
}