using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeDescriptionException : Exception {
        public NodeDescriptionException(Type nodeClass)
            : base("Node = " + nodeClass) { }

        public NodeDescriptionException(Type nodeClass, string msg)
            : base(string.Format("Node = {0}, msg = {1}", nodeClass, msg)) { }
    }
}