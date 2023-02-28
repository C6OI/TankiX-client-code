using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeCache {
        NodesToChange nodesToChange;

        public NodeCache(EngineServiceInternal engineService) { }

        [Inject] public static FlowInstancesCache flowInstances { get; set; }

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public virtual NodesToChange GetNodesToChange(EntityInternal entity, Type componentClass) {
            NodesToChange result = default;
            result.NodesToAdd = GetAddedNodes(entity, componentClass);
            result.NodesToRemove = GetRemovedNodes(entity, componentClass);
            return result;
        }

        protected ICollection<NodeDescription> GetAddedNodes(EntityInternal entity, Type componentClass) {
            BitSet componentsBitId = entity.ComponentsBitId;
            ICollection<NodeDescription> nodeDescriptions = NodeDescriptionRegistry.GetNodeDescriptions(componentClass);
            List<NodeDescription> instance = flowInstances.listNodeDescription.GetInstance();
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator(nodeDescriptions);

            while (enumerator.MoveNext()) {
                NodeDescription current = enumerator.Current;

                if (componentsBitId.Mask(current.NodeComponentBitId) && componentsBitId.MaskNot(current.NotNodeComponentBitId)) {
                    instance.Add(current);
                }
            }

            return instance;
        }

        protected ICollection<NodeDescription> GetRemovedNodes(EntityInternal entity, Type componentClass) {
            BitSet componentsBitId = entity.ComponentsBitId;
            ICollection<NodeDescription> nodeDescriptionsByNotComponent = NodeDescriptionRegistry.GetNodeDescriptionsByNotComponent(componentClass);
            List<NodeDescription> instance = flowInstances.listNodeDescription.GetInstance();
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator(nodeDescriptionsByNotComponent);

            while (enumerator.MoveNext()) {
                NodeDescription current = enumerator.Current;

                if (componentsBitId.Mask(current.NodeComponentBitId) && !componentsBitId.MaskNot(current.NotNodeComponentBitId) && entity.NodeDescriptionStorage.Contains(current)) {
                    instance.Add(current);
                }
            }

            return instance;
        }
    }
}