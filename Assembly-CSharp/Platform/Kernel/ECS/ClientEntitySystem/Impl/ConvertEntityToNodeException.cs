using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ConvertEntityToNodeException : Exception {
        public ConvertEntityToNodeException(NodeDescription nodeDescription, Entity entity)
            : base(string.Format("nodeDescription = {0}, entity = {1}", nodeDescription, entity)) { }

        public ConvertEntityToNodeException(NodeDescription nodeDescription, Entity entity, Exception e)
            : base(string.Format("nodeDescription = {0}, entity = {1}", nodeDescription, entity), e) { }

        public ConvertEntityToNodeException(Type nodeClass, Entity entity, Exception e)
            : base(string.Format("nodeClass = {0}, entity = {1}", nodeClass, entity), e) { }
    }
}