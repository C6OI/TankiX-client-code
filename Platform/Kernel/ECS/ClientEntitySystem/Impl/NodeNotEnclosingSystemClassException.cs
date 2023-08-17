using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeNotEnclosingSystemClassException : NodeDescriptionException {
        public NodeNotEnclosingSystemClassException(Type nodeClass)
            : base(nodeClass) { }
    }
}