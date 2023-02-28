using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentNotFoundException : Exception {
        public ComponentNotFoundException(Entity entity, Type componentClass)
            : base(string.Format("{0} entity={1}", componentClass.Name, entity)) { }
    }
}