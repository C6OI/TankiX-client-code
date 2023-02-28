using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ItemBundleLimitBundleEffectsSystem : ECSSystem {
        [OnEventFire]
        public void Empty(NodeAddedEvent e, SingleNode<InventoryLimitBundleEffectsComponent> node) { }
    }
}