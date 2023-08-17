using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeInheritanceException : NodeDescriptionException {
        public NodeInheritanceException(Type nodeClass)
            : base(nodeClass) { }
    }
}