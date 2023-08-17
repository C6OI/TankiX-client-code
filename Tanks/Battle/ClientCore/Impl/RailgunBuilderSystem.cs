using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class RailgunBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode,
            [JoinByUser] SingleNode<UserComponent> userNode) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<ChargingWeaponControllerComponent>();
            entity.AddComponent<ReadyRailgunChargingWeaponComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<PenetrationTargetCollectorComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent(new WeaponHitComponent(true, false));
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, WeaponNode weaponNode,
            [JoinByBattle] SingleNode<TeamBattleComponent> battle) =>
            weaponNode.Entity.AddComponent<TeamTargetEvaluatorComponent>();

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, WeaponNode weaponNode,
            [JoinByBattle] SingleNode<CTFComponent> battle) => weaponNode.Entity.AddComponent<CTFTargetEvaluatorComponent>();

        public class WeaponNode : Node {
            public BattleGroupComponent battleGroup;

            public RailgunComponent railgun;

            public RailgunChargingWeaponComponent railgunChargingWeapon;
            public UserGroupComponent userGroup;
        }

        public class SelfTankNode : Node {
            public AssembledTankComponent assembledTank;

            public SelfTankComponent selfTank;
            public UserGroupComponent userGroup;
        }
    }
}