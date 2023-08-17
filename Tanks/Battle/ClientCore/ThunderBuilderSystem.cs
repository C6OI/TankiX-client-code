using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore {
    public class ThunderBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [JoinByUser] [Context] ThunderNode weaponNode,
            [JoinByUser] SingleNode<UserComponent> userNode) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<DiscreteWeaponControllerComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<TargetCollectorComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, ThunderNode weaponNode,
            [JoinByBattle] SingleNode<TeamBattleComponent> battle) =>
            weaponNode.Entity.AddComponent<TeamTargetEvaluatorComponent>();

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, ThunderNode weaponNode,
            [JoinByBattle] SingleNode<CTFComponent> battle) => weaponNode.Entity.AddComponent<CTFTargetEvaluatorComponent>();

        public class ThunderNode : Node {
            public BattleGroupComponent battleGroup;
            public ThunderComponent thunder;

            public UserGroupComponent userGroup;
        }

        public class SelfTankNode : Node {
            public AssembledTankComponent assembledTank;
            public SelfTankComponent selfTank;

            public UserGroupComponent userGroup;
        }
    }
}