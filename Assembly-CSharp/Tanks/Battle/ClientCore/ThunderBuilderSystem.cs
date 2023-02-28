using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore {
    public class ThunderBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildBot(NodeAddedEvent evt, AutopilotTankNode botTank, [Context] [JoinByUser] ThunderNode weaponNode, [Context] [JoinByUser] SingleNode<UserComponent> userNode) {
            BuildWeapon(botTank.Entity, weaponNode);
        }

        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] ThunderNode weaponNode, [JoinByUser] SingleNode<UserComponent> userNode) {
            BuildWeapon(selfTank.Entity, weaponNode);
        }

        void BuildWeapon(Entity tank, ThunderNode weaponNode) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<DiscreteWeaponControllerComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            TargetCollectorComponent component = new(new TargetCollector(tank), new TargetValidator(tank));
            entity.AddComponent(component);
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, ThunderNode weaponNode, [JoinByBattle] SingleNode<TeamBattleComponent> battle) {
            weaponNode.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, ThunderNode weaponNode, [JoinByBattle] SingleNode<CTFComponent> battle) {
            weaponNode.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

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

        public class AutopilotTankNode : Node {
            public AssembledTankComponent assembledTank;

            public TankAutopilotComponent tankAutopilot;
            public UserGroupComponent userGroup;
        }
    }
}