using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class RicochetBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildAll(NodeAddedEvent evt, WeaponNode smokyNode) {
            Entity entity = smokyNode.Entity;
        }

        [OnEventFire]
        public void BuildBot(NodeAddedEvent evt, AutopilotTankNode botTank, [Context] [JoinByUser] WeaponNode weaponNode, [Context] [JoinByUser] UserNode user) {
            BuildWeapon(botTank.Entity, weaponNode);
        }

        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode, [Context] [JoinByUser] UserNode user) {
            BuildWeapon(selfTank.Entity, weaponNode);
        }

        void BuildWeapon(Entity tank, WeaponNode weaponNode) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<DiscreteWeaponControllerComponent>();
            entity.AddComponent<VerticalTargetingComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            TargetCollectorComponent component = new(new TargetCollector(tank), new RicochetTargetValidator(tank, 0.5f));
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

            public RicochetComponent ricochet;

            public UserGroupComponent userGroup;
        }

        public class SelfTankNode : Node {
            public AssembledTankComponent assembledTank;

            public SelfTankComponent selfTank;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class AutopilotTankNode : Node {
            public AssembledTankComponent assembledTank;

            public TankAutopilotComponent tankAutopilot;
            public UserGroupComponent userGroup;
        }
    }
}