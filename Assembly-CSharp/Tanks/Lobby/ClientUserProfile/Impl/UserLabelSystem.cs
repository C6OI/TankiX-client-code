using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class UserLabelSystem : ECSSystem {
        [OnEventComplete]
        public void AddUserModerator(NodeAddedEvent e, UserModeratorNode user, [JoinByUser] [Context] [Combine] UserLabelNode userLabel) {
            userLabel.uidIndicator.Color = userLabel.uidColor.ModeratorColor;
        }

        [OnEventFire]
        public void UserOnline(NodeAddedEvent e, UserOnlineNode user, [Context] [JoinByUser] [Combine] UserLabelNode userLabel) {
            userLabel.userLabelAvatar.AvatarImage.SpriteUid = user.userAvatar.Id;
            Image component = userLabel.userLabelAvatar.AvatarImage.GetComponent<Image>();
            component.color = userLabel.userLabelAvatar.OnlineColor;
            userLabel.userLabelAvatar.IsPremium = user.Entity.HasComponent<PremiumAccountBoostComponent>();
            userLabel.userLabelAvatar.IsSelf = user.Entity.HasComponent<SelfUserComponent>();
        }

        [OnEventFire]
        public void UserWentOffline(NodeRemoveEvent e, UserOnlineNode user, [JoinByUser] [Combine] UserLabelNode userLabel) {
            MarkUserAvatarAsOffline(userLabel);
        }

        [OnEventFire]
        public void HighlightUserLabel(NodeAddedEvent e, SingleNode<SelfUserComponent> selfUser, [JoinSelf] UserOnlineNode user,
            [Context] [JoinByUser] [Combine] UserLabelWithHighlightningNode userLabel) {
            userLabel.uidIndicator.FontStyle = userLabel.uidHighlighting.selfUser;
        }

        void MarkUserAvatarAsOffline(UserLabelNode userLabel) {
            Image component = userLabel.userLabelAvatar.AvatarImage.GetComponent<Image>();
            component.color = userLabel.userLabelAvatar.OfflineColor;
            userLabel.userLabelAvatar.IsPremium = false;
            userLabel.userLabelAvatar.IsSelf = false;
        }

        [OnEventFire]
        public void UserOnline(NodeAddedEvent e, UserOnlineNode user, [Context] [JoinByUser] [Combine] UserLabelStateNode userLabel) {
            userLabel.userLabelState.UserOnline();
        }

        [OnEventFire]
        public void UserWentOffline(NodeRemoveEvent e, UserOnlineNode user, [JoinByUser] [Combine] UserLabelStateNode userLabel) {
            userLabel.userLabelState.UserOffline();
        }

        [OnEventFire]
        public void SetLeagueBorder(NodeAddedEvent e, LeagueBorderNode leagueBorder, [JoinByUser] UserNode user, [JoinByLeague] LeagueNode league) {
            leagueBorder.leagueBorder.SetLeague(league.leagueConfig.LeagueIndex);
        }

        [OnEventFire]
        public void SetLeagueBorder(NodeAddedEvent e, SingleNode<SelfUserAvatarComponent> ui, [JoinAll] SelfUserNode user, [JoinByLeague] LeagueNode league) {
            ui.component.SetLeagueBorder(league.leagueConfig.LeagueIndex);
            ui.component.SetRank();
        }

        public class UserLabelNode : Node {
            public RankIconComponent rankIcon;

            public UidColorComponent uidColor;

            public UidIndicatorComponent uidIndicator;

            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;

            public UserLabelAvatarComponent userLabelAvatar;
        }

        public class UserLabelStateNode : Node {
            public UserGroupComponent userGroup;
            public UserLabelComponent userLabel;

            public UserLabelStateComponent userLabelState;
        }

        public class UserLabelWithHighlightningNode : UserLabelNode {
            public UidHighlightingComponent uidHighlighting;
        }

        public class UserNode : Node {
            public LeagueGroupComponent leagueGroup;
            public UserComponent user;

            public UserAvatarComponent userAvatar;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;

            public UserUidComponent userUid;
        }

        public class SelfUserNode : UserNode {
            public SelfUserComponent selfUser;
        }

        public class UserOnlineNode : UserNode {
            public UserOnlineComponent userOnline;
        }

        public class UserModeratorNode : UserNode {
            public ModeratorComponent moderator;
        }

        public class UserLabelWaitForInviteResponseIconNode : UserLabelNode {
            public UserLabelWaitIntiveResponseIconComponent userLabelWaitIntiveResponseIcon;
        }

        public class LeagueBorderNode : Node {
            public LeagueBorderComponent leagueBorder;

            public UserGroupComponent userGroup;
        }

        public class LeagueNode : Node {
            public LeagueComponent league;

            public LeagueConfigComponent leagueConfig;

            public LeagueGroupComponent leagueGroup;
        }
    }
}