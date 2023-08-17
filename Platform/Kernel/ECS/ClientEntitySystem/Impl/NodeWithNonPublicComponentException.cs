using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeWithNonPublicComponentException : NodeDescriptionException {
        public NodeWithNonPublicComponentException(Type nodeClass)
            : base(nodeClass) { }
    }
}