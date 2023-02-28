using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeClassInstanceDescription {
        public static readonly NodeClassInstanceDescription EMPTY = new(typeof(Node), AbstractNodeDescription.EMPTY);

        readonly IDictionary<Type, FieldInfo> fieldByComponent;

        public NodeClassInstanceDescription(Type nodeClass, NodeDescription nodeDescription) {
            NodeClass = nodeClass;
            NodeDescription = nodeDescription;
            fieldByComponent = CreateAndPopulateFieldByComponent();
        }

        [Inject] public static FlowInstancesCache Cache { get; set; }

        public NodeDescription NodeDescription { get; set; }

        public Type NodeClass { get; }

        IDictionary<Type, FieldInfo> CreateAndPopulateFieldByComponent() {
            IDictionary<Type, FieldInfo> dictionary = new Dictionary<Type, FieldInfo>();
            FieldInfo[] fields = NodeClass.GetFields();

            foreach (FieldInfo fieldInfo in fields) {
                if (typeof(Component).IsAssignableFrom(fieldInfo.FieldType)) {
                    dictionary[fieldInfo.FieldType] = fieldInfo;
                }
            }

            return dictionary;
        }

        public void SetComponent(Node node, Type componentClass, Component component) {
            if (fieldByComponent.ContainsKey(componentClass)) {
                fieldByComponent[componentClass].SetValue(node, component);
            }
        }

        public Node CreateNode(EntityInternal entity) {
            Node nodeInstance = Cache.GetNodeInstance(NodeClass);
            nodeInstance.Entity = entity;

            try {
                Collections.Enumerator<Type> enumerator = Collections.GetEnumerator(NodeDescription.Components);

                while (enumerator.MoveNext()) {
                    Type current = enumerator.Current;
                    SetComponent(nodeInstance, current, entity.GetComponent(current));
                }

                return nodeInstance;
            } catch (Exception e) {
                throw new ConvertEntityToNodeException(NodeClass, entity, e);
            }
        }

        public void FreeNode(Node node) {
            Cache.FreeNodeInstance(node);
        }

        public override bool Equals(object o) {
            if (this == o) {
                return true;
            }

            if (o == null || GetType() != o.GetType()) {
                return false;
            }

            NodeClassInstanceDescription nodeClassInstanceDescription = (NodeClassInstanceDescription)o;
            return NodeClass == nodeClassInstanceDescription.NodeClass;
        }

        public override int GetHashCode() => 629 + NodeClass.GetHashCode();
    }
}