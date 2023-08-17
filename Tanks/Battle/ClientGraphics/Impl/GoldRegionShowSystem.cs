using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GoldRegionShowSystem : ECSSystem {
        [OnEventFire]
        public void ShowGoldRegion(NodeAddedEvent e, GoldBonusNode gold,
            [JoinByBonusRegion] InvisibleGoldBonusRegionNode region) =>
            region.Entity.AddComponent<VisibleBonusRegionComponent>();

        [OnEventFire]
        public void HideGoldRegion(NodeAddedEvent e, GoldBonusGroundedNode gold,
            [JoinByBonusRegion] VisibleGoldBonusRegionNode region) =>
            region.Entity.RemoveComponent<VisibleBonusRegionComponent>();

        [OnEventFire]
        public void HideGoldRegion(NodeRemoveEvent e, GoldBonusNode gold,
            [JoinByBonusRegion] VisibleGoldBonusRegionNode region) =>
            region.Entity.RemoveComponent<VisibleBonusRegionComponent>();

        public class GoldBonusNode : Node {
            public BonusComponent bonus;

            public BonusRegionGroupComponent bonusRegionGroup;
        }

        public class GoldBonusGroundedNode : Node {
            public BonusComponent bonus;

            public BonusGroundedStateComponent bonusGroundedState;

            public BonusRegionGroupComponent bonusRegionGroup;
        }

        public class VisibleGoldBonusRegionNode : Node {
            public BonusRegionGroupComponent bonusRegionGroup;

            public GoldBonusRegionComponent goldBonusRegion;

            public VisibleBonusRegionComponent visibleBonusRegion;
        }

        [Not(typeof(VisibleBonusRegionComponent))]
        public class InvisibleGoldBonusRegionNode : Node {
            public BonusRegionGroupComponent bonusRegionGroup;

            public GoldBonusRegionComponent goldBonusRegion;
        }
    }
}