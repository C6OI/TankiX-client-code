using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeDescriptionRegistryImpl : NodeDescriptionRegistry {
        readonly IDictionary<Type, NodeClassInstanceDescription> nodeClassDescByNodeClass = new Dictionary<Type, NodeClassInstanceDescription>();

        readonly MultiMap<Type, NodeClassInstanceDescription> nodeClassDescsByComponent = new();

        readonly ICollection<NodeDescription> nodeDescriptions;
        readonly IDictionary<Type, ICollection<NodeDescription>> nodeDescriptionsByAnyComponent;

        readonly IDictionary<Type, ICollection<NodeDescription>> nodeDescriptionsByNotComponent;

        readonly Dictionary<NodeDescription, NodeDescription> nodeDescriptionStorage = new();

        readonly ICollection<NodeDescription> nodeDescriptionsWithNotComponentsOnly;

        public NodeDescriptionRegistryImpl() {
            nodeDescriptionsByAnyComponent = new Dictionary<Type, ICollection<NodeDescription>>();
            nodeDescriptionsByNotComponent = new Dictionary<Type, ICollection<NodeDescription>>();
            nodeDescriptionsWithNotComponentsOnly = new HashSet<NodeDescription>();
            nodeDescriptions = new HashSet<NodeDescription>();
        }

        [Inject] public static Protocol Protocol { get; set; }

        public ICollection<NodeDescription> NodeDescriptions {
            get {
                HashSet<NodeDescription> result = new();

                nodeDescriptionsByAnyComponent.Values.ForEach(delegate(ICollection<NodeDescription> x) {
                    result.UnionWith(x);
                });

                return result;
            }
        }

        public void AddNodeDescription(NodeDescription nodeDescription) {
            if (!nodeDescription.IsEmpty) {
                nodeDescription = (StandardNodeDescription)nodeDescriptionStorage.ComputeIfAbsent(nodeDescription, d => d);

                nodeDescription.Components.ForEach(delegate(Type clazz) {
                    nodeDescriptionsByAnyComponent.ComputeIfAbsent(clazz, k => new HashSet<NodeDescription>()).Add(nodeDescription);
                });

                nodeDescription.NotComponents.ForEach(delegate(Type clazz) {
                    nodeDescriptionsByAnyComponent.ComputeIfAbsent(clazz, k => new HashSet<NodeDescription>()).Add(nodeDescription);
                });

                nodeDescription.NotComponents.ForEach(delegate(Type clazz) {
                    nodeDescriptionsByNotComponent.ComputeIfAbsent(clazz, k => new HashSet<NodeDescription>()).Add(nodeDescription);
                });

                if (nodeDescription.Components.Count == 0) {
                    nodeDescriptionsWithNotComponentsOnly.Add(nodeDescription);
                }

                nodeDescriptions.Add(nodeDescription);
            }

            if (Protocol == null) {
                return;
            }

            foreach (Type component in nodeDescription.Components) {
                if (SerializationUidUtils.HasSelfUid(component)) {
                    Protocol.RegisterTypeWithSerialUid(component);
                }
            }
        }

        public ICollection<NodeDescription> GetNodeDescriptions(Type componentClass) =>
            nodeDescriptionsByAnyComponent.GetOrDefault(componentClass, Collections.EmptyList<NodeDescription>);

        public ICollection<NodeDescription> GetNodeDescriptionsByNotComponent(Type componentClass) =>
            nodeDescriptionsByNotComponent.GetOrDefault(componentClass, Collections.EmptyList<NodeDescription>);

        public ICollection<NodeDescription> GetNodeDescriptionsWithNotComponentsOnly() => nodeDescriptionsWithNotComponentsOnly;

        public void AssertRegister(NodeDescription nodeDescription) {
            if (!nodeDescriptions.Contains(nodeDescription)) {
                throw new NodeNotRegisteredException(nodeDescription);
            }
        }

        public NodeClassInstanceDescription GetOrCreateNodeClassDescription(Type nodeClass, ICollection<Type> additionalComponents = null) {
            StandardNodeDescription nodeDesc = new(nodeClass, additionalComponents);

            if (nodeDesc.IsEmpty) {
                return NodeClassInstanceDescription.EMPTY;
            }

            nodeDesc = (StandardNodeDescription)nodeDescriptionStorage.ComputeIfAbsent(nodeDesc, d => d);
            NodeClassInstanceDescription nodeClassInstanceDescription = null;

            nodeClassInstanceDescription = !nodeDesc.isAdditionalComponents ? nodeClassDescByNodeClass.ComputeIfAbsent(nodeClass, k => new NodeClassInstanceDescription(k, nodeDesc))
                                               : new NodeClassInstanceDescription(nodeClass, nodeDesc);

            Collections.Enumerator<Type> enumerator = Collections.GetEnumerator(nodeClassInstanceDescription.NodeDescription.Components);

            while (enumerator.MoveNext()) {
                nodeClassDescsByComponent.Add(enumerator.Current, nodeClassInstanceDescription);
            }

            return nodeClassInstanceDescription;
        }

        public ICollection<NodeClassInstanceDescription> GetClassInstanceDescriptionByComponent(Type component) {
            HashSet<NodeClassInstanceDescription> value;

            if (nodeClassDescsByComponent.TryGetValue(component, out value)) {
                return value;
            }

            return Collections.EmptyList<NodeClassInstanceDescription>();
        }
    }
}