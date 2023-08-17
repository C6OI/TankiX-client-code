using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EntityTestImpl : EntityImpl, Entity, EntityTest {
        public EntityTestImpl(EngineServiceInternal engineService, long id, string name)
            : base(engineService, id, name) { }

        public EntityTestImpl(EngineServiceInternal engineService, long id, string name,
            Optional<TemplateAccessor> templateAccessor)
            : base(engineService, id, name, templateAccessor) { }

        [Inject] public new static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public T GetComponentInTest<T>() where T : Component => (T)GetComponent(typeof(T));

        public bool HasComponentInTest<T>() where T : Component {
            Type typeFromHandle = typeof(T);
            return HasComponent(typeFromHandle);
        }

        public void AddComponentInTest<RealT>(Component component) where RealT : Component {
            Type typeFromHandle = typeof(RealT);
            storage.AddComponentImmediately(typeFromHandle, component);
            MakeNodes(typeFromHandle, component);
            Flow.Current.SendEvent(new ComponentAddedEvent(component), this);
            nodeAddedEventMaker.MakeIfNeed(this, typeFromHandle);
        }

        public void UpdateNodes() {
            BitSet componentsBitId = ComponentsBitId;

            ICollection<NodeDescription> nodeDescriptions =
                ((NodeDescriptionRegistryImpl)NodeDescriptionRegistry).NodeDescriptions;

            foreach (NodeDescription item in nodeDescriptions) {
                if (!nodeDescriptionStorage.Contains(item) &&
                    componentsBitId.Mask(item.NodeComponentBitId) &&
                    componentsBitId.MaskNot(item.NotNodeComponentBitId)) {
                    AddNode(item);
                }
            }
        }
    }
}