using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class UserExperienceIndicatorSystem : ECSSystem {
        [Mandatory]
        [OnEventFire]
        public void GetUserLevelInfo(GetUserLevelInfoEvent e, UserNode user, [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig) {
            e.Info = LevelInfo.Get(user.userExperience.Experience, ranksExperiencesConfig.component.RanksExperiences);
        }

        [OnEventFire]
        public void SetCurrentAndNextRankExperience(NodeAddedEvent e, CurrentAndNextRankExperienceNode currentAndNextRankExperience, [Context] [JoinByUser] UserNode user,
            [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig) {
            int[] ranksExperiences = ranksExperiencesConfig.component.RanksExperiences;
            int obj = ranksExperiences.Length + 1;
            Text text = currentAndNextRankExperience.currentAndNextRankExperience.Text;

            if (user.userRank.Rank.Equals(obj)) {
                text.text = user.userExperience.Experience.ToStringSeparatedByThousands();
                return;
            }

            int value = ranksExperiences[user.userRank.Rank];
            text.text = user.userExperience.Experience.ToStringSeparatedByThousands() + "/" + value.ToStringSeparatedByThousands();
        }

        [OnEventFire]
        public void SetUserExperienceProgress(NodeAddedEvent e, UserExperienceProgressBarNode userExperienceProgressBar, [Context] [JoinByUser] UserNode user,
            [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig) {
            int[] ranksExperiences = ranksExperiencesConfig.component.RanksExperiences;
            int obj = ranksExperiences.Length + 1;

            if (user.userRank.Rank.Equals(obj)) {
                userExperienceProgressBar.userExperienceProgressBar.SetProgress(1f);
                return;
            }

            int num = ranksExperiences[user.userRank.Rank];
            float num2 = 0f;

            if (user.userRank.Rank.Equals(1)) {
                num2 = user.userExperience.Experience / (float)num;
            } else {
                int num3 = ranksExperiences[user.userRank.Rank - 1];
                num2 = (user.userExperience.Experience - num3) / (float)(num - num3);
            }

            num2 = Mathf.Clamp01(num2);
            userExperienceProgressBar.userExperienceProgressBar.SetProgress(num2);
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserExperienceComponent userExperience;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;
        }

        public class CurrentAndNextRankExperienceNode : Node {
            public CurrentAndNextRankExperienceComponent currentAndNextRankExperience;

            public UserGroupComponent userGroup;
        }

        public class UserExperienceProgressBarNode : Node {
            public UserExperienceProgressBarComponent userExperienceProgressBar;

            public UserGroupComponent userGroup;
        }
    }
}