using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class SmokyBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode, [Context] [JoinByUser] UserNode user) {
            BuildSelf(weaponNode.Entity, selfTank.Entity);
        }

        [OnEventFire]
        public void BuildBot(NodeAddedEvent evt, AutopilotTankNode botTank, [Context] [JoinByUser] WeaponNode weaponNode, [Context] [JoinByUser] UserNode user) {
            BuildSelf(weaponNode.Entity, botTank.Entity);
        }

        public void BuildSelf(Entity weapon, Entity tank) {
            weapon.AddComponent<CooldownTimerComponent>();
            weapon.AddComponent<DiscreteWeaponControllerComponent>();
            weapon.AddComponent<VerticalSectorsTargetingComponent>();
            weapon.AddComponent<DirectionEvaluatorComponent>();
            weapon.AddComponent<WeaponShotComponent>();
            weapon.AddComponent(new WeaponHitComponent(true, false));
            weapon.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            TargetCollectorComponent component = new(new TargetCollector(tank), new TargetValidator(tank));
            weapon.AddComponent(component);
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

            public SmokyComponent smoky;

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

        public class UserNode : Node {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }
    }
}