using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableUserLabelIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetUserLabel(NodeAddedEvent e, UserNode user, [Context] [JoinByUser] UserLabelIndicatorNode userLabelIndicator, [JoinByScoreTable] ScoreTableNode scoreTable) {
            GameObject userLabel = userLabelIndicator.scoreTableUserLabelIndicator.userLabel;
            bool premium = user.Entity.HasComponent<PremiumAccountBoostComponent>();
            UserLabelBuilder userLabelBuilder = new(user.Entity.Id, userLabel, user.userAvatar.Id, premium);
            LeagueNode leagueNode = Select<LeagueNode>(user.Entity, typeof(LeagueGroupComponent)).FirstOrDefault();

            if (leagueNode != null) {
                userLabelBuilder.SetLeague(leagueNode.leagueConfig.LeagueIndex);
            }

            userLabelBuilder.SkipLoadUserFromServer();

            if (scoreTable.scoreTableUserAvatar.EnableShowUserProfileOnAvatarClick) {
                userLabelBuilder.SubscribeAvatarClick();
            }

            userLabelBuilder.Build();
        }

        public class ScoreTableNode : Node {
            public ScoreTableComponent scoreTable;

            public ScoreTableGroupComponent scoreTableGroup;

            public ScoreTableUserAvatarComponent scoreTableUserAvatar;
        }

        public class UserLabelIndicatorNode : Node {
            public ScoreTableUserLabelIndicatorComponent scoreTableUserLabelIndicator;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public UserAvatarComponent userAvatar;
            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class LeagueNode : Node {
            public LeagueConfigComponent leagueConfig;

            public LeagueGroupComponent leagueGroup;
        }
    }
}