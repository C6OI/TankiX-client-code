using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.Impl {
    public class LoadByEventRequestComponent : Component {
        public Type ResourceDataComponentType { get; set; }

        public Entity Owner { get; set; }
    }
}