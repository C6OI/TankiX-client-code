using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableUserLabelIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetUserLabel(NodeAddedEvent e, UserNode user,
            [Context] [JoinByUser] UserLabelIndicatorNode userLabelIndicator, [JoinByScoreTable] ScoreTableNode scoreTable) {
            UserLabelBuilder userLabelBuilder = new(user.Entity.Id);
            userLabelBuilder.SkipLoadUserFromServer();

            if (scoreTable.scoreTableUserAvatar.EnableShowUserProfileOnAvatarClick) {
                userLabelBuilder.SubscribeAvatarClick();
            }

            GameObject gameObject = userLabelBuilder.Build();
            gameObject.transform.SetParent(userLabelIndicator.scoreTableUserLabelIndicator.gameObject.transform, false);
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
            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }
    }
}