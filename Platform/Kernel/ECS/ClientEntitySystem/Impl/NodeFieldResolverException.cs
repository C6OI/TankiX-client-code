using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeFieldResolverException : Exception {
        public NodeFieldResolverException(NodeClassInstanceDescription description, Exception e)
            : base("description = " + description, e) { }
    }
}