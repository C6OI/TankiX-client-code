using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class UserNotificationHUDSystem : ECSSystem {
        [OnEventFire]
        public void CreateUpdateUserRankMessage(UpdateRankEffectFinishedEvent evt, SelfBattleUserNode battleUser, [JoinByUser] SelfUserNode user, [JoinAll] ScreenNode screen,
            [JoinByScreen] UserNotificatorHUDNode notificator, [JoinByScreen] UserNotificatorRanksNamesNode notificatorNames) {
            UserRankNotificationMessageBehaviour userRankNotificationMessageBehaviour =
                InstantiateUserNotification(notificator.userNotificatorHUD, notificator.userNotificatorHUD.UserRankNotificationMessagePrefab);

            userRankNotificationMessageBehaviour.Icon.SelectSprite(user.userRank.Rank.ToString());
            userRankNotificationMessageBehaviour.IconImage.SetNativeSize();

            userRankNotificationMessageBehaviour.Message.text =
                string.Format(notificator.userNotificatorHUDText.UserRankMessageFormat, notificatorNames.ranksNames.Names[user.userRank.Rank]);

            notificator.userNotificatorHUD.Push(userRankNotificationMessageBehaviour);
        }

        T InstantiateUserNotification<T>(UserNotificatorHUDComponent notificator, T notificationPrefab) where T : BaseUserNotificationMessageBehaviour {
            T result = Object.Instantiate(notificationPrefab);
            Transform transform = result.transform;
            Transform transform2 = notificator.transform;
            transform.SetParent(transform2);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            return result;
        }

        public class ScreenNode : Node {
            public BattleScreenComponent battleScreen;
            public ScreenGroupComponent screenGroup;
        }

        public class SelfBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;
        }

        public class UserNotificatorHUDNode : Node {
            public ScreenGroupComponent screenGroup;
            public UserNotificatorHUDComponent userNotificatorHUD;

            public UserNotificatorHUDTextComponent userNotificatorHUDText;
        }

        public class UserNotificatorRanksNamesNode : Node {
            public RanksNamesComponent ranksNames;

            public ScreenGroupComponent screenGroup;
            public UserNotificatorRankNamesComponent userNotificatorRankNames;
        }
    }
}