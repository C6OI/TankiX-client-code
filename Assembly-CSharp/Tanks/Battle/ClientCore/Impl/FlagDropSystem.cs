using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlagDropSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void FlagDropRequest(UpdateEvent e, CarriedFlagNode flag, [JoinByTank] SelfTankNode tank, [JoinByBattle] SingleNode<RoundActiveStateComponent> round,
            [JoinByBattle] SingleNode<CTFConfigComponent> ctfConfig) {
            if (InputManager.GetActionKeyDown(CTFActions.DROP_FLAG)) {
                ScheduleEvent<SendTankMovementEvent>(tank);
                NewEvent<FlagDropRequestEvent>().Attach(flag).Attach(tank).Schedule();
                NewEvent<TankFlagCollisionEvent>().Attach(flag).Attach(tank).ScheduleDelayed(ctfConfig.component.enemyFlagActionMinIntervalSec);
            }
        }

        [OnEventComplete]
        public void FlagCheckCollision(NodeAddedEvent e, GroundedFlagNode flag) {
            flag.flagInstance.FlagInstance.SetActive(false);
            flag.flagInstance.FlagInstance.SetActive(true);
        }

        public class CarriedFlagNode : Node {
            public FlagComponent flag;

            public TankGroupComponent tankGroup;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }

        public class GroundedFlagNode : Node {
            public FlagGroundedStateComponent flagGroundedState;

            public FlagInstanceComponent flagInstance;
            public FlagPositionComponent flagPosition;
        }
    }
}