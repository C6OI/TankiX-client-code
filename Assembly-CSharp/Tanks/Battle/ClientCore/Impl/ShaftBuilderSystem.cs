using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode, [Context] [JoinByUser] UserNode user) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<ShaftStateControllerComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void Build(NodeAddedEvent evt, TankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode) {
            TargetCollectorComponent component = new(new TargetCollector(selfTank.Entity), new TargetValidator(selfTank.Entity));
            weaponNode.Entity.AddComponent(component);
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

            public ShaftComponent shaft;

            public UserGroupComponent userGroup;
        }

        public class SelfTankNode : Node {
            public AssembledTankComponent assembledTank;

            public SelfTankComponent selfTank;
            public UserGroupComponent userGroup;
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public TankComponent tank;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class DMBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public DMComponent dm;
        }

        public class TDMBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public TDMComponent tdm;
        }

        public class CTFBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public CTFComponent ctf;
        }
    }
}