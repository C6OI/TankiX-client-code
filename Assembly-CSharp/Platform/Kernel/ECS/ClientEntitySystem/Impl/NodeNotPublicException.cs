using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeNotPublicException : NodeDescriptionException {
        public NodeNotPublicException(Type nodeClass)
            : base(nodeClass) { }
    }
}