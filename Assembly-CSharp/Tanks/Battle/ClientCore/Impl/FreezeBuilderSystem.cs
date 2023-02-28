using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class FreezeBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context] [JoinByUser] WeaponNode weaponNode, [Context] [JoinByUser] UserNode user) {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<StreamWeaponControllerComponent>();
            entity.AddComponent<ConicTargetingComponent>();
            entity.AddComponent(new WeaponHitComponent(false, true));
            entity.AddComponent(new StreamWeaponSimpleFeedbackControllerComponent());
            TargetCollectorComponent component = new(new TargetCollector(selfTank.Entity), new PenetrationTargetValidator(selfTank.Entity));
            entity.AddComponent(component);
        }

        public class WeaponNode : Node {
            public BattleGroupComponent battleGroup;

            public FreezeComponent freeze;
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
    }
}