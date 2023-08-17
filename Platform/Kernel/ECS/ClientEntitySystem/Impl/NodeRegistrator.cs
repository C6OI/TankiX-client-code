using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeRegistrator {
        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public void Register(Type nodeType) =>
            NodeDescriptionRegistry.AddNodeDescription(new StandardNodeDescription(nodeType));
    }
}