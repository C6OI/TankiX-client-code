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

        public void Clear() {
            nextArgumentEntityNodes.Clear();
        }

        public EntityNode Init(ArgumentNode argumentNode, Entity entity) {
            this.entity = entity;
            this.argumentNode = argumentNode;
            nextArgumentEntityNodes.Clear();
            invokeArgument = null;
            return this;
        }

        public void PrepareInvokeArgument() {
            NodeClassInstanceDescription classInstanceDescription = argumentNode.argument.ClassInstanceDescription;
            invokeArgument = ((EntityInternal)entity).GetNode(classInstanceDescription);
        }

        public void ConvertToOptional() {
            MethodInfo method = argumentNode.argument.ArgumentType.GetMethod("nullableOf");
            invokeArgument = method.Invoke(null, new object[1] { invokeArgument });
        }
    }
}