using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ProfileStatsSectionUISystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void SetLevelInfo(NodeAddedEvent e, ProfileStatsSectionUINode sectionUI, [JoinAll] UserStatisticsNode statistics) {
            GetUserLevelInfoEvent getUserLevelInfoEvent = new();
            ScheduleEvent(getUserLevelInfoEvent, statistics);
            sectionUI.profileStatsSectionUI.SetRank(getUserLevelInfoEvent.Info);
        }

        public class ProfileStatsSectionUINode : Node {
            public ProfileStatsSectionUIComponent profileStatsSectionUI;
        }

        public class UserStatisticsNode : Node {
            public FavoriteEquipmentStatisticsComponent favoriteEquipmentStatistics;

            public KillsEquipmentStatisticsComponent killsEquipmentStatistics;
            public SelfUserComponent selfUser;

            public UserStatisticsComponent userStatistics;
        }
    }
}