using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankUpsideDownSystem : ECSSystem {
        [OnEventFire]
        public void InitTankChecking(NodeAddedEvent e, SelfTankNode selfTank) {
            NewEvent<CheckUpsideDownStateEvent>().Attach(selfTank).SchedulePeriodic(1f);
        }

        [OnEventFire]
        public void MarkTankAsUpsideDown(CheckUpsideDownStateEvent e, SelfTankNode tank, [JoinByUser] SingleNode<UpsideDownConfigComponent> config) {
            float y = tank.rigidbody.RigidbodyTransform.up.y;

            if (y < config.component.GetUpsideDownCosToCheck()) {
                UpsideDownTankComponent upsideDownTankComponent = new();
                upsideDownTankComponent.TimeTankBecomesUpsideDown = Date.Now;
                tank.Entity.AddComponent(upsideDownTankComponent);
            }
        }

        [OnEventFire]
        public void ClearUpsideDownMark(CheckUpsideDownStateEvent e, UpsideDownSelfTankNode tank, [JoinByUser] SingleNode<UpsideDownConfigComponent> config) {
            float y = tank.rigidbody.RigidbodyTransform.up.y;

            if (y >= config.component.GetUpsideDownCosToCheck()) {
                tank.upsideDownTank.Removed = true;
                tank.Entity.RemoveComponent<UpsideDownTankComponent>();
            }
        }

        [OnEventFire]
        public void ClearUpsideDownMarkOnTankRemove(NodeRemoveEvent e, UpsideDownSelfTankForNRNode nr, [JoinSelf] UpsideDownSelfTankNode tank) {
            if (!tank.upsideDownTank.Removed) {
                tank.Entity.RemoveComponent<UpsideDownTankComponent>();
            }
        }

        [Not(typeof(UpsideDownTankComponent))]
        public class SelfTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public RigidbodyComponent rigidbody;

            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;
        }

        public class UpsideDownSelfTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public RigidbodyComponent rigidbody;

            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;

            public UpsideDownTankComponent upsideDownTank;
        }

        public class UpsideDownSelfTankForNRNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public RigidbodyComponent rigidbody;

            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;
        }
    }
}