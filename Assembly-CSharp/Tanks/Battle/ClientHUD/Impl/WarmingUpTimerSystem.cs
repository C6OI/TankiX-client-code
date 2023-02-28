using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class WarmingUpTimerSystem : ECSSystem {
        [OnEventFire]
        public void ShowWarmingUpTimer(NodeAddedEvent e, RoundWarmingUpStateNode round, MainHUDNode hud) {
            hud.mainHUDTimers.ShowWarmingUpTimer();
        }

        [OnEventFire]
        public void HideWarmingUpTimer(NodeRemoveEvent e, RoundWarmingUpStateNode round, MainHUDNode hud) {
            hud.mainHUDTimers.HideWarmingUpTimer();
        }

        public class RoundWarmingUpStateNode : Node {
            public RoundActiveStateComponent roundActiveState;
            public RoundStopTimeComponent roundStopTime;

            public RoundWarmingUpStateComponent roundWarmingUpState;
        }

        public class MainHUDNode : Node {
            public MainHUDComponent mainHUD;

            public MainHUDTimersComponent mainHUDTimers;
        }
    }
}