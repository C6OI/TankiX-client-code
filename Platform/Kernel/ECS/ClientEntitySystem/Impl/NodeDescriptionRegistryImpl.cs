using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeDescriptionRegistryImpl : NodeDescriptionRegistry {
        readonly IDictionary<Type, NodeClassInstanceDescription> nodeClassDescByNodeClass =
            new Dictionary<Type, NodeClassInstanceDescription>();

        readonly MultiMap<Type, NodeClassInstanceDescription> nodeClassDescsByComponent = new();

        readonly ICollection<NodeDescription> nodeDescriptions;
        readonly IDictionary<Type, ICollection<NodeDescription>> nodeDescriptionsByAnyComponent;

        readonly IDictionary<Type, ICollection<NodeDescription>> nodeDescriptionsByNotComponent;

        readonly ICollection<NodeDescription> nodeDescriptionsWithNotComponentsOnly;

        public NodeDescriptionRegistryImpl() {
            nodeDescriptionsByAnyComponent = new Dictionary<Type, ICollection<NodeDescription>>();
            nodeDescriptionsByNotComponent = new Dictionary<Type, ICollection<NodeDescription>>();
            nodeDescriptionsWithNotComponentsOnly = new HashSet<NodeDescription>();
            nodeDescriptions = new HashSet<NodeDescription>();
        }

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
            if (nodeDescription.IsEmpty) {
                return;
            }

            nodeDescription.Components.ForEach(delegate(Type clazz) {
                nodeDescriptionsByAnyComponent.ComputeIfAbsent(clazz, k => new HashSet<NodeDescription>())
                    .Add(nodeDescription);
            });

            nodeDescription.NotComponents.ForEach(delegate(Type clazz) {
                nodeDescriptionsByAnyComponent.ComputeIfAbsent(clazz, k => new HashSet<NodeDescription>())
                    .Add(nodeDescription);
            });

            nodeDescription.NotComponents.ForEach(delegate(Type clazz) {
                nodeDescriptionsByNotComponent.ComputeIfAbsent(clazz, k => new HashSet<NodeDescription>())
                    .Add(nodeDescription);
            });

            if (nodeDescription.Components.Count == 0) {
                nodeDescriptionsWithNotComponentsOnly.Add(nodeDescription);
            }

            nodeDescriptions.Add(nodeDescription);
        }

        public ICollection<NodeDescription> GetNodeDescriptions(Type componentClass) =>
            nodeDescriptionsByAnyComponent.GetOrDefault(componentClass, Collections.EmptyList<NodeDescription>);

        public ICollection<NodeDescription> GetNodeDescriptionsByNotComponent(Type componentClass) =>
            nodeDescriptionsByNotComponent.GetOrDefault(componentClass, Collections.EmptyList<NodeDescription>);

        public ICollection<NodeDescription> GetNodeDescriptionsWithNotComponentsOnly() =>
            nodeDescriptionsWithNotComponentsOnly;

        public void AssertRegister(NodeDescription nodeDescription) {
            if (!nodeDescriptions.Contains(nodeDescription)) {
                throw new NodeNotRegisteredException(nodeDescription);
            }
        }

        public NodeClassInstanceDescription GetOrCreateNodeClassDescription(Type nodeClass) {
            StandardNodeDescription standardNodeDescription = new(nodeClass);

            if (standardNodeDescription.IsEmpty) {
                return NodeClassInstanceDescription.EMPTY;
            }

            NodeClassInstanceDescription nodeClassInstanceDescription = nodeClassDescByNodeClass.ComputeIfAbsent(nodeClass,
                k => new NodeClassInstanceDescription(k, new StandardNodeDescription(nodeClass)));

            Collections.Enumerator<Type> enumerator =
                Collections.GetEnumerator(nodeClassInstanceDescription.NodeDescription.Components);

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