using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeRegistrator {
        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public void Register(Type nodeType, ICollection<Type> additionalComponents) {
            NodeDescriptionRegistry.AddNodeDescription(new StandardNodeDescription(nodeType, additionalComponents));
        }
    }
}