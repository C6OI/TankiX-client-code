using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteFriendToBattleNotificationSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, NotificationNode notification, [Context] [JoinByUser] SingleNode<UserUidComponent> fromUser,
            NotificationNode notificationToMap, [Context] [JoinByMap] MapNode map) {
            string message = string.Format(notification.inviteFriendToBattleNotification.MessageTemplate, fromUser.component.Uid, map.descriptionItem.Name,
                notification.battleMode.BattleMode);

            notification.Entity.AddComponent(new NotificationMessageComponent(message));
        }

        [OnEventFire]
        public void GoToBattle(NotificationClickEvent e, NotificationNode notification, [JoinAll] Optional<SingleNode<SelfBattleUserComponent>> userInBattle) {
            ScheduleEvent(new ShowBattleEvent(notification.battleGroup.Key), EngineService.EntityStub);
        }

        public class NotificationNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleModeComponent battleMode;
            public InviteFriendToBattleNotificationComponent inviteFriendToBattleNotification;

            public MapGroupComponent mapGroup;

            public UserGroupComponent userGroup;
        }

        public class MapNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MapComponent map;
        }
    }
}