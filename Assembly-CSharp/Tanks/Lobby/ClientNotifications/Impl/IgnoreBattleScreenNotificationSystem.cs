using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class IgnoreBattleScreenNotificationSystem : ECSSystem {
        [OnEventFire]
        public void SkipNotificationOnBattleScreen(ShowNotificationEvent evt, SingleNode<IgnoreBattleScreenNotificationComponent> notification, [JoinAll] BattleScreenNode screen) {
            evt.CanShowNotification = false;
        }

        [OnEventFire]
        public void CloseNotificationOnBattleScreen(NodeAddedEvent evt, [Combine] ActiveNotificationNode notification, BattleScreenNode screen) {
            ScheduleEvent<CloseNotificationEvent>(notification);
        }

        public class BattleScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public BattleScreenComponent battleScreen;
            public ScreenComponent screen;
        }

        public class ActiveNotificationNode : Node {
            public ActiveNotificationComponent activeNotification;

            public IgnoreBattleScreenNotificationComponent ignoreBattleScreenNotification;
        }
    }
}