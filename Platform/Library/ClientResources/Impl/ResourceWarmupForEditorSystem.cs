using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;

namespace Platform.Library.ClientResources.Impl {
    public class ResourceWarmupForEditorSystem : ECSSystem {
        [OnEventFire]
        public void SetWarmUpResourcesAsPrepared(NodeAddedEvent e, WarmupResourcesNode node) =>
            node.Entity.AddComponent<WarmupResourcesPreparedComponent>();

        public class WarmupResourcesNode : Node {
            public WarmupResourcesComponent warmupResources;
        }
    }
}