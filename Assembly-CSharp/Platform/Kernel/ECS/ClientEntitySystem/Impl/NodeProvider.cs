using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeProvider {
        readonly Dictionary<Node, NodeClassInstanceDescription> descriptionByNode = new(10);

        readonly Node emptyNode;
        readonly EntityInternal entity;

        readonly Dictionary<NodeClassInstanceDescription, Node> nodeByDescription = new(10);

        public NodeProvider(EntityInternal entity) {
            this.entity = entity;
            emptyNode = new Node();
            emptyNode.Entity = entity;
        }

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public Node GetNode(NodeClassInstanceDescription nodeClassInstanceDescription) {
            if (nodeClassInstanceDescription == NodeClassInstanceDescription.EMPTY) {
                return emptyNode;
            }

            AssertCanCast(nodeClassInstanceDescription.NodeDescription);
            Node value;

            if (!nodeByDescription.TryGetValue(nodeClassInstanceDescription, out value)) {
                value = CreateNode(nodeClassInstanceDescription);
                nodeByDescription[nodeClassInstanceDescription] = value;
            }

            return value;
        }

        public void CleanNodes() {
            Dictionary<NodeClassInstanceDescription, Node>.Enumerator enumerator = nodeByDescription.GetEnumerator();

            while (enumerator.MoveNext()) {
                KeyValuePair<NodeClassInstanceDescription, Node> current = enumerator.Current;
                NodeClassInstanceDescription key = current.Key;
                Node value = current.Value;
                key.FreeNode(value);
            }

            nodeByDescription.Clear();
            descriptionByNode.Clear();
        }

        void AssertCanCast(NodeDescription nodeDescription) {
            if (!CanCast(nodeDescription)) {
                throw new ConvertEntityToNodeException(nodeDescription, entity);
            }
        }

        public bool CanCast(NodeDescription nodeDescription) {
            if (nodeDescription.IsEmpty) {
                return true;
            }

            return entity.Contains(nodeDescription);
        }

        Node CreateNode(NodeClassInstanceDescription description) {
            Node node = description.CreateNode(entity);
            descriptionByNode[node] = description;
            return node;
        }

        public void OnComponentAdded(Component component) {
            UpdateComponentValue(component, component.GetType());
        }

        public void OnComponentAdded(Component component, Type componentType) {
            UpdateComponentValue(component, componentType);
        }

        public void OnComponentChanged(Component component) {
            UpdateComponentValue(component, component.GetType());
        }

        void UpdateComponentValue(Component component, Type componentType) {
            ICollection<NodeClassInstanceDescription> classInstanceDescriptionByComponent = NodeDescriptionRegistry.GetClassInstanceDescriptionByComponent(componentType);
            IEnumerator<NodeClassInstanceDescription> enumerator = classInstanceDescriptionByComponent.GetEnumerator();

            while (enumerator.MoveNext()) {
                NodeClassInstanceDescription current = enumerator.Current;
                Node value;

                if (nodeByDescription.TryGetValue(current, out value)) {
                    current.SetComponent(value, componentType, component);
                }
            }
        }
    }
}