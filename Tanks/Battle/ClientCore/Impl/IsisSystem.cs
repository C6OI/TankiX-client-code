using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IsisSystem : ECSSystem {
        [OnEventFire]
        public void SetMaskForIsisTargeting(NodeAddedEvent evt, IsisTargetCollectorNode weapon) =>
            weapon.targetCollector.Mask = LayerMasks.GUN_TARGETING_WITHOUT_DEAD_UNITS;

        [OnEventFire]
        public void EnableStreamHit(NodeAddedEvent e, WorkingNode weapon, [JoinByTank] [Context] SelfTankNode tank) =>
            weapon.Entity.AddComponent<StreamHitCheckingComponent>();

        [OnEventFire]
        public void DisableStreamHit(NodeRemoveEvent e, WorkingNode weapon, [Context] [JoinByTank] SelfTankNode tank) =>
            weapon.Entity.RemoveComponent<StreamHitCheckingComponent>();

        public class IsisTargetCollectorNode : Node {
            public IsisComponent isis;

            public TargetCollectorComponent targetCollector;
        }

        public class WorkingNode : Node {
            public IsisComponent isis;

            public StreamWeaponWorkingComponent streamWeaponWorking;

            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public UserGroupComponent userGroup;
        }
    }
}