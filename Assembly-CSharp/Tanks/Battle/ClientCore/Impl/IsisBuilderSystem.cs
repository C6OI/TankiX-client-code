using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IsisBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent e, SelfTankNode tank, [Context] [JoinByUser] WeaponNode weapon) {
            weapon.Entity.AddComponent<CooldownTimerComponent>();
            weapon.Entity.AddComponent<StreamWeaponControllerComponent>();
            weapon.Entity.AddComponent<ConicTargetingComponent>();
            weapon.Entity.AddComponent(new WeaponHitComponent(false, false));
            weapon.Entity.AddComponent<DirectionEvaluatorComponent>();
            weapon.Entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            TargetCollectorComponent component = new(new TargetCollector(tank.Entity), new TargetValidator(tank.Entity));
            weapon.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, WeaponNode weaponNode, [JoinByBattle] SingleNode<CTFComponent> battle) {
            weaponNode.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        public class WeaponNode : Node {
            public BattleGroupComponent battleGroup;

            public IsisComponent isis;

            public UserGroupComponent userGroup;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public UserGroupComponent userGroup;
        }
    }
}