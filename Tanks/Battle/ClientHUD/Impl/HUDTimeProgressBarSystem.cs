using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientHUD.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HUDTimeProgressBarSystem : ECSSystem {
        [OnEventFire]
        public void SetTimeLimit(NodeAddedEvent e, TimeProgressBarNode timeProgressBarNode, BattleNode battle) =>
            timeProgressBarNode.hudTimeProgressBar.GetComponent<TimeProgressBar>()
                .SetTotalRoundTime(battle.timeLimit.TimeLimitSec);

        [OnEventFire]
        public void ActivateTimeProgressBar(NodeAddedEvent e, SingleNode<BattleScreenComponent> battleScreen,
            BattleNode battle) => battleScreen.component.timeProgressBar.gameObject.SetActive(true);

        [OnEventFire]
        public void DeactivateTimeProgressBar(NodeRemoveEvent e, SingleNode<HUDTimeProgressBarComponent> timeProgressBar) =>
            timeProgressBar.component.gameObject.SetActive(false);

        [OnEventFire]
        public void Init(NodeAddedEvent e, TimeProgressBarNode timeProgressBarNode, RoundNode round,
            BattleWithTimeNode timeLimitedBattle) {
            VisibilityPrerequisitesComponent visibilityPrerequisites = timeProgressBarNode.visibilityPrerequisites;
            visibilityPrerequisites.RemoveAll();
            TimeProgressBar component = timeProgressBarNode.hudTimeProgressBar.GetComponent<TimeProgressBar>();

            component.Activate(timeLimitedBattle.battleStartTime.RoundStartTime.UnityTime,
                timeLimitedBattle.timeLimit.TimeLimitSec,
                timeProgressBarNode.visibilityPeriods.lastBlinkingIntervalInSec);

            SetDelayedStartForNextVisibilityPeriod(timeProgressBarNode,
                round.roundStopTime,
                timeLimitedBattle.timeLimit.TimeLimitSec);
        }

        [OnEventFire]
        public void DeactivateTimer(NodeRemoveEvent e, TimeProgressBarNode timeProgressBarNode, RoundNode round,
            BattleNode timeLimitedBattle) =>
            timeProgressBarNode.hudTimeProgressBar.GetComponent<TimeProgressBar>().Deactivate();

        [OnEventFire]
        public void OnStartVisibilityPeriod(ShowTimeProgressBarEvent e, TimeProgressBarNode timeProgressBar,
            [JoinAll] RoundNode round, [JoinAll] BattleNode timeLimitedBattle) {
            VisibilityPeriodsComponent visibilityPeriods = timeProgressBar.visibilityPeriods;
            float num = timeLimitedBattle.timeLimit.TimeLimitSec;
            float remainingRoundTimeInSec = GetRemainingRoundTimeInSec(round.roundStopTime, num);

            float currentPeriodInterval = VisibilityPeriodsUtil.GetCurrentPeriodInterval(visibilityPeriods,
                num - remainingRoundTimeInSec,
                remainingRoundTimeInSec);

            ScheduleEvent(new StartVisiblePeriodEvent(currentPeriodInterval), timeProgressBar);
        }

        [OnEventFire]
        public void OnStopVisibilityPeriod(StopVisiblePeriodEvent e, TimeProgressBarNode timeProgressBar,
            [JoinAll] RoundNode round, [JoinAll] BattleNode timeLimitedBattle) =>
            SetDelayedStartForNextVisibilityPeriod(timeProgressBar,
                round.roundStopTime,
                timeLimitedBattle.timeLimit.TimeLimitSec);

        void SetDelayedStartForNextVisibilityPeriod(TimeProgressBarNode timeProgressBar,
            RoundStopTimeComponent roundStopTime, float totalRoundTimeInSec) {
            float remainingRoundTimeInSec = GetRemainingRoundTimeInSec(roundStopTime, totalRoundTimeInSec);

            if (remainingRoundTimeInSec > 0f) {
                float currentPeriodDelay = VisibilityPeriodsUtil.GetCurrentPeriodDelay(timeProgressBar.visibilityPeriods,
                    totalRoundTimeInSec - remainingRoundTimeInSec,
                    remainingRoundTimeInSec);

                NewEvent<ShowTimeProgressBarEvent>().Attach(timeProgressBar.Entity).ScheduleDelayed(currentPeriodDelay);
            }
        }

        static float GetRemainingRoundTimeInSec(RoundStopTimeComponent roundStopTime, float totalRoundTimeInSec) {
            float value = roundStopTime.StopTime.UnityTime - Date.Now.UnityTime;
            return Mathf.Clamp(value, 0f, totalRoundTimeInSec);
        }

        public class BattleNode : Node {
            public SelfComponent self;
            public TimeLimitComponent timeLimit;
        }

        public class BattleWithTimeNode : BattleNode {
            public BattleStartTimeComponent battleStartTime;
        }

        public class RoundNode : Node {
            public RoundActiveStateComponent roundActiveState;
            public RoundStopTimeComponent roundStopTime;
        }

        public class TimeProgressBarNode : Node {
            public HUDTimeProgressBarComponent hudTimeProgressBar;
            public VisibilityPeriodsComponent visibilityPeriods;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }
    }
}