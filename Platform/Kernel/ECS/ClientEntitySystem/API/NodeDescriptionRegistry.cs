using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface NodeDescriptionRegistry {
        void AddNodeDescription(NodeDescription nodeDescription);

        ICollection<NodeDescription> GetNodeDescriptions(Type componentClass);

        ICollection<NodeDescription> GetNodeDescriptionsByNotComponent(Type componentClass);

        ICollection<NodeDescription> GetNodeDescriptionsWithNotComponentsOnly();

        NodeClassInstanceDescription GetOrCreateNodeClassDescription(Type nodeClass);

        ICollection<NodeClassInstanceDescription> GetClassInstanceDescriptionByComponent(Type component);

        void AssertRegister(NodeDescription nodeDescription);
    }
}