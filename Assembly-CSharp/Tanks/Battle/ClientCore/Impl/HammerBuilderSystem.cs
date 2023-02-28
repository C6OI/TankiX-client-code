using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class HammerBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildBot(NodeAddedEvent evt, AutopilotTankNode botTank, [Context] [JoinByUser] WeaponNode weaponNode) {
            BuildWeapon(botTank.Entity, weaponNode);
        }

        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode) {
            BuildWeapon(selfTank.Entity, weaponNode);
        }

        void BuildWeapon(Entity tank, WeaponNode weaponNode) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<DiscreteWeaponControllerComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            entity.AddComponent(new WeaponHitComponent(true, false));
            HammerTargetCollectorComponent component = new(new TargetCollector(tank), new TargetValidator(tank));
            entity.AddComponent(component);
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, WeaponNode weaponNode, [JoinByBattle] SingleNode<TeamBattleComponent> battle) {
            weaponNode.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, WeaponNode weaponNode, [JoinByBattle] SingleNode<CTFComponent> battle) {
            weaponNode.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        public class WeaponNode : Node {
            public BattleGroupComponent battleGroup;

            public DiscreteWeaponComponent discreteWeapon;

            public HammerComponent hammer;
            public UserGroupComponent userGroup;
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public TankComponent tank;
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