using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SupplyRegionShowSystem : ECSSystem {
        [OnEventFire]
        public void HideBonusRegion(NodeAddedEvent e, GroundedBonusNode bonus, [JoinByBonusRegion] VisibleSupplyBonusRegionNode bonusRegion) {
            bonusRegion.Entity.RemoveComponent<VisibleBonusRegionComponent>();
        }

        [OnEventFire]
        public void ShowBonusRegion(NodeRemoveEvent e, GroundedBonusNode bonus, [JoinByBonusRegion] InvisibleSupplyBonusRegionNode bonusRegion) {
            bonusRegion.Entity.AddComponent<VisibleBonusRegionComponent>();
        }

        public class GroundedBonusNode : Node {
            public BonusGroundedStateComponent bonusGroundedState;

            public BonusRegionGroupComponent bonusRegionGroup;
        }

        public class VisibleSupplyBonusRegionNode : Node {
            public BonusRegionGroupComponent bonusRegionGroup;

            public SupplyBonusRegionComponent supplyBonusRegion;

            public VisibleBonusRegionComponent visibleBonusRegion;
        }

        [Not(typeof(VisibleBonusRegionComponent))]
        public class InvisibleSupplyBonusRegionNode : Node {
            public BonusRegionGroupComponent bonusRegionGroup;

            public SupplyBonusRegionComponent supplyBonusRegion;
        }
    }
}