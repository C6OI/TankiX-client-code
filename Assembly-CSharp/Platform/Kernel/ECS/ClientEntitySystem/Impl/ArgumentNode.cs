using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ArgumentNode {
        public HandlerArgument argument;

        public List<EntityNode> entityNodes;

        public bool filled;
        public bool linkBreak;

        public ArgumentNode(HandlerArgument argument) {
            this.argument = argument;
            entityNodes = new List<EntityNode>();
        }

        [Inject] public static FlowInstancesCache Cache { get; set; }

        public ArgumentNode Init() {
            Clear();
            return this;
        }

        public void Clear() {
            entityNodes.Clear();
            filled = false;
            linkBreak = argument.SelectAll || argument.Collection;
        }

        public bool IsEmpty() => entityNodes.Count == 0;

        public void ConvertToCollection() {
            IList collection = GetCollection();
            entityNodes.Clear();
            EntityNode entityNode = Cache.entityNode.GetInstance().Init(this, null);
            entityNode.invokeArgument = collection;
            entityNodes.Add(entityNode);
        }

        IList GetCollection() {
            IList genericListInstance = Cache.GetGenericListInstance(argument.ClassInstanceDescription.NodeClass, entityNodes.Count);

            for (int i = 0; i < entityNodes.Count; i++) {
                genericListInstance.Add(entityNodes[i].invokeArgument);
            }

            return genericListInstance;
        }

        public void ConvertToOptional() {
            if (IsEmpty()) {
                linkBreak = true;
                EntityNode entityNode = Cache.entityNode.GetInstance().Init(this, null);
                entityNode.ConvertToOptional();
                entityNodes.Add(entityNode);
            } else {
                for (int i = 0; i < entityNodes.Count; i++) {
                    entityNodes[i].ConvertToOptional();
                }
            }
        }

        public bool TryGetEntityNode(Entity entity, out EntityNode entityNode) {
            NodeClassInstanceDescription classInstanceDescription = argument.ClassInstanceDescription;
            entityNode = null;

            if (!((EntityInternal)entity).CanCast(classInstanceDescription.NodeDescription)) {
                return false;
            }

            entityNode = Cache.entityNode.GetInstance().Init(this, entity);
            return true;
        }

        public void PrepareInvokeArguments() {
            for (int i = 0; i < entityNodes.Count; i++) {
                entityNodes[i].PrepareInvokeArgument();
            }
        }
    }
}