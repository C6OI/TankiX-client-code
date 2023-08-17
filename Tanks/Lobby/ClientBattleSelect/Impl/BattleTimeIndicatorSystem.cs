using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleTimeIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void UpdateTime(UpdateEvent e, BattleTimeIndicatorNode battleTimeIndicator,
            [JoinByBattle] BattleTimeNode battleTime) {
            float num = 0f;
            long timeLimitSec = battleTime.timeLimit.TimeLimitSec;
            float num2 = timeLimitSec;
            float num3 = 0f;

            if (battleTime.Entity.HasComponent<BattleStartTimeComponent>()) {
                Date roundStartTime = battleTime.Entity.GetComponent<BattleStartTimeComponent>().RoundStartTime;
                num3 = Date.Now - roundStartTime;
                num2 -= num3;
                Date endDate = roundStartTime + timeLimitSec;
                num = Date.Now.GetProgress(roundStartTime, endDate);
            }

            string timerText = TimerUtils.GetTimerText(num2);
            battleTimeIndicator.battleTimeIndicator.Progress = 1f - num;
            battleTimeIndicator.battleTimeIndicator.Time = timerText;
        }

        public class BattleTimeNode : Node {
            public BattleGroupComponent battleGroup;
            public TimeLimitComponent timeLimit;
        }

        public class BattleTimeIndicatorNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleTimeIndicatorComponent battleTimeIndicator;
        }
    }
}