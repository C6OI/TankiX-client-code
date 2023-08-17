using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BattleShutdownNotificationSystem : ECSSystem {
        [OnEventFire]
        public void AddNotificationText(NodeAddedEvent e, NotificationNode notification) =>
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));

        [OnEventFire]
        public void UpdateTimeNotification(UpdateEvent e, NotificationMessageNode notification,
            [JoinAll] SelfBattleNode battle, [JoinAll] SingleNode<LocalizedTimerComponent> timer) {
            float num = Date.Now - battle.battleStartTime.RoundStartTime;
            float num2 = battle.timeLimit.TimeLimitSec - num;

            if (num2 < 0f) {
                num2 = 0f;
            }

            string arg = timer.component.GenerateTimerString(num2);
            notification.notificationMessage.Message = string.Format(notification.battleShutdownText.Text, arg);
        }

        public class NotificationNode : Node {
            public BattleShutdownNotificationComponent battleShutdownNotification;

            public BattleShutdownTextComponent battleShutdownText;
        }

        public class NotificationMessageNode : Node {
            public BattleShutdownNotificationComponent battleShutdownNotification;

            public BattleShutdownTextComponent battleShutdownText;

            public NotificationMessageComponent notificationMessage;
        }

        public class SelfBattleNode : Node {
            public BattleComponent battle;

            public BattleStartTimeComponent battleStartTime;

            public SelfComponent self;

            public TimeLimitComponent timeLimit;
        }
    }
}