using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityNode {
        public ArgumentNode argumentNode;

        public Entity entity;

        public object invokeArgument;

        public List<EntityNode> nextArgumentEntityNodes;

        public EntityNode() => nextArgumentEntityNodes = new List<EntityNode>();

        public void Clear() => nextArgumentEntityNodes.Clear();

        public EntityNode Init(ArgumentNode argumentNode, object invokeArgumenet, Entity entity = null) {
            this.entity = entity;
            this.argumentNode = argumentNode;
            invokeArgument = invokeArgumenet;
            nextArgumentEntityNodes.Clear();
            return this;
        }

        public void ConvertToOptional() {
            MethodInfo method = argumentNode.argument.ArgumentType.GetMethod("nullableOf");
            invokeArgument = method.Invoke(null, new object[1] { invokeArgument });
        }
    }
}