using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class BattleLoadScreenSystem : ECSSystem {
        [Inject] public static MapRegistry MapRegistry { get; set; }

        [OnEventFire]
        public void ShowScreen(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> battleUser, [JoinByBattle] [Context] BattleNode battle, [JoinByMap] [Context] MapNode map) {
            ScheduleEvent<ShowScreenNoAnimationEvent<BattleLoadScreenComponent>>(battleUser);
            ScheduleEvent(new ChangeScreenLogEvent(LogScreen.Battle), battleUser);
        }

        [OnEventFire]
        public void OnShowScreen(NodeAddedEvent e, BattleLoadScreenNode screen, [JoinAll] SingleNode<SelfBattleUserComponent> battleUser, [JoinByBattle] [Context] BattleNode battle,
            [JoinByMap] [Context] SingleNode<MapComponent> map) {
            screen.battleLoadScreen.InitView(battle.Entity, MapRegistry.GetMap(map.Entity));
        }

        [OnEventFire]
        public void CheckScreenReady(UpdateEvent e, NotReadeBattleLoadScreenNode screen) {
            if (screen.battleLoadScreen.isReadyToHide) {
                screen.Entity.AddComponent<BattleLoadScreenReadyToHideComponent>();
            }
        }

        [OnEventFire]
        public void HideNotificationOnBattleLoad(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> seftBattleUser, [JoinAll] ClickableNotificationNode notification) {
            notification.activeNotification.Hide();
        }

        [OnEventFire]
        public void HideNotificationOnBattleLoad(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> seftBattleUser, [JoinAll] SingleNode<AvatarDialogComponent> avatarDialog) {
            avatarDialog.component.Hide();
        }

        [OnEventFire]
        public void RegisterComponent(NodeAddedEvent e, SingleNode<ArcadeBattleComponent> battle) { }

        [OnEventFire]
        public void RegisterComponent(NodeAddedEvent e, SingleNode<EnergyBattleComponent> battle) { }

        [OnEventFire]
        public void RegisterComponent(NodeAddedEvent e, SingleNode<RatingBattleComponent> battle) { }

        public class BattleNode : Node {
            public BattleComponent battle;
            public BattleModeComponent battleMode;
        }

        public class MapNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MapComponent map;
        }

        public class BattleLoadScreenNode : Node {
            public BattleLoadScreenComponent battleLoadScreen;
        }

        [Not(typeof(BattleLoadScreenReadyToHideComponent))]
        public class NotReadeBattleLoadScreenNode : Node {
            public BattleLoadScreenComponent battleLoadScreen;
        }

        [Not(typeof(NotClickableNotificationComponent))]
        public class ClickableNotificationNode : Node {
            public ActiveNotificationComponent activeNotification;

            public EmailConfirmationNotificationComponent emailConfirmationNotification;
            public NotificationComponent notification;

            public NotificationConfigComponent notificationConfig;

            public NotificationMessageComponent notificationMessage;
        }
    }
}