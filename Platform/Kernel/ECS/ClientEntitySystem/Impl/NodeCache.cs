using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeCache {
        readonly IDictionary<BitSet, IDictionary<Type, NodesToChange>> cache =
            new Dictionary<BitSet, IDictionary<Type, NodesToChange>>();

        readonly EngineServiceInternal engineService;

        NodesToChange nodesToChange;

        public NodeCache(EngineServiceInternal engineService) => this.engineService = engineService;

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public virtual NodesToChange GetNodesToChange(EntityInternal entity, Type componentClass) {
            IDictionary<Type, NodesToChange> value;

            if (!cache.TryGetValue(entity.ComponentsBitId, out value)) {
                value = new Dictionary<Type, NodesToChange>();
                cache.Add((BitSet)entity.ComponentsBitId.Clone(), value);
            }

            NodesToChange value2;

            if (!value.TryGetValue(componentClass, out value2)) {
                NodesToChange nodesToChange = default;
                nodesToChange.NodesToAdd = GetAddedNodes(entity, componentClass);
                nodesToChange.NodesToRemove = GetRemovedNodes(entity, componentClass);
                value2 = nodesToChange;
                value.Add(componentClass, value2);
            }

            return value2;
        }

        protected ICollection<NodeDescription> GetAddedNodes(EntityInternal entity, Type componentClass) {
            BitSet componentsBitId = entity.ComponentsBitId;
            ICollection<NodeDescription> nodeDescriptions = NodeDescriptionRegistry.GetNodeDescriptions(componentClass);
            List<NodeDescription> list = new(10);
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator(nodeDescriptions);

            while (enumerator.MoveNext()) {
                NodeDescription current = enumerator.Current;

                if (componentsBitId.Mask(current.NodeComponentBitId) &&
                    componentsBitId.MaskNot(current.NotNodeComponentBitId)) {
                    list.Add(current);
                }
            }

            list.Sort();
            return list;
        }

        protected ICollection<NodeDescription> GetRemovedNodes(EntityInternal entity, Type componentClass) {
            BitSet componentsBitId = entity.ComponentsBitId;

            ICollection<NodeDescription> nodeDescriptionsByNotComponent =
                NodeDescriptionRegistry.GetNodeDescriptionsByNotComponent(componentClass);

            List<NodeDescription> list = new(3);
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator(nodeDescriptionsByNotComponent);

            while (enumerator.MoveNext()) {
                NodeDescription current = enumerator.Current;

                if (componentsBitId.Mask(current.NodeComponentBitId) &&
                    !componentsBitId.MaskNot(current.NotNodeComponentBitId) &&
                    entity.NodeDescriptionStorage.Contains(current)) {
                    list.Add(current);
                }
            }

            list.Sort();
            return list;
        }
    }
}