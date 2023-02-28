using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class VulcanBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode, [Context] [JoinByUser] UserNode user) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent(new WeaponHitComponent(false, false));
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            TargetCollectorComponent component = new(new TargetCollector(selfTank.Entity), new TargetValidator(selfTank.Entity));
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
            public UserGroupComponent userGroup;

            public VulcanComponent vulcan;
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
    }
}