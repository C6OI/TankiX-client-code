using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteFriendToBattleNotificationSystem : ECSSystem {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, NotificationNode notification,
            [JoinByUser] [Context] SingleNode<UserUidComponent> fromUser, NotificationNode notificationToMap,
            [Context] [JoinByMap] SingleNode<MapNameComponent> map) {
            string message = string.Format(notification.inviteFriendToBattleNotification.MessageTemplate,
                fromUser.component.Uid,
                map.component.Name,
                notification.battleMode.BattleMode);

            notification.Entity.AddComponent(new NotificationMessageComponent(message));
        }

        [OnEventFire]
        public void GoToBattle(NotificationClickEvent e, NotificationNode notification,
            [JoinAll] Optional<SingleNode<SelfBattleUserComponent>> userInBattle) {
            if (!userInBattle.IsPresent()) {
                ScheduleEvent(new ShowBattleEvent(notification.battleGroup.Key), EngineService.EntityStub);
            } else {
                ScheduleEvent<ShowScreenNoAnimationEvent<BattleSelectLoadScreenComponent>>(notification);
            }
        }

        public class NotificationNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleModeComponent battleMode;
            public InviteFriendToBattleNotificationComponent inviteFriendToBattleNotification;

            public MapGroupComponent mapGroup;

            public UserGroupComponent userGroup;
        }
    }
}