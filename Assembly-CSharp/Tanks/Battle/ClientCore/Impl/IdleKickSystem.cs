using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IdleKickSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void Sync(IdleBeginTimeSyncEvent e, BattleUserNode battleUser) {
            battleUser.idleBeginTime.IdleBeginTime = e.IdleBeginTime;
        }

        [OnEventFire]
        public void InputListner(UpdateEvent e, BattleUserNode user) {
            int checkPeriodicTimeSec = user.idleKickConfig.CheckPeriodicTimeSec;

            if (InputManager.IsAnyKey() || IsMouseMovement()) {
                Date now = Date.Now;

                if (now - user.idleKickCheckLastTime.CheckLastTime > checkPeriodicTimeSec) {
                    ScheduleEvent<ResetIdleKickTimeEvent>(user);
                    user.idleKickCheckLastTime.CheckLastTime = now;
                }
            }
        }

        bool IsMouseMovement() => InputManager.GetAxis(CameraRotationActions.MOUSEX_ROTATE) != 0f || InputManager.GetAxis(CameraRotationActions.MOUSEY_ROTATE) != 0f ||
                                  InputManager.GetAxis(CameraRotationActions.MOUSEY_MOVE_SHAFT_AIM) != 0f;

        public class BattleUserNode : Node {
            public BattleUserComponent battleUser;

            public IdleBeginTimeComponent idleBeginTime;

            public IdleCounterComponent idleCounter;

            public IdleKickCheckLastTimeComponent idleKickCheckLastTime;

            public IdleKickConfigComponent idleKickConfig;
            public SelfComponent self;
        }
    }
}