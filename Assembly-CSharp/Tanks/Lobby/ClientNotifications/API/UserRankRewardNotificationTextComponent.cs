using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNotifications.API {
    public class UserRankRewardNotificationTextComponent : Component {
        public string RankHeaderText { get; set; }

        public string RewardLabelText { get; set; }
    }
}