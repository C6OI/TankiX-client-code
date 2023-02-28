using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class ProfileScreenLoadSystem : ECSSystem {
        [OnEventFire]
        public void AttachSelfProfileScreenOrLoadRemoteUserProfile(NodeAddedEvent e, ProfileScreenNode profileScreen, [JoinByScreen] ProfileScreenContextNode profileScreenContext,
            [JoinAll] SelfUserNode selfUser) {
            if (profileScreenContext.profileScreenContext.UserId.Equals(selfUser.Entity.Id)) {
                selfUser.userGroup.Attach(profileScreen.Entity);
            } else {
                ScheduleEvent(new RequestLoadUserProfileEvent(profileScreenContext.profileScreenContext.UserId), selfUser);
            }
        }

        [OnEventFire]
        public void AttachProfileScreenToUserGroup(UserProfileLoadedEvent e, RemoteUserNode remoteUser, [JoinAll] ProfileScreenNode screen,
            [JoinByScreen] ProfileScreenContextNode profileScreenContext) {
            if (remoteUser.Entity.Id.Equals(profileScreenContext.profileScreenContext.UserId)) {
                remoteUser.userGroup.Attach(screen.Entity);
            }
        }

        [OnEventFire]
        public void SendRequestUnloadUserProfile(NodeRemoveEvent e, ProfileScreenContextNode context, [JoinByScreen] ProfileScreenNode profileScreen, [JoinAll] SelfUserNode selfUser) {
            if (!context.profileScreenContext.UserId.Equals(selfUser.Entity.Id)) {
                ScheduleEvent(new RequestUnloadUserProfileEvent(context.profileScreenContext.UserId), selfUser);
            }
        }

        [OnEventFire]
        public void Register(GetUserStatisticsInfoEvent e, UserStatisticsNode statistic) {
            Debug.Log("GetUserStatisticsInfoEvent");
            e.Statistics = statistic.userStatistics.Statistics;
            e.FavoriteHullStatistics = statistic.favoriteEquipmentStatistics.HullStatistics;
            e.FavoriteTurretStatistics = statistic.favoriteEquipmentStatistics.TurretStatistics;
            e.KillHullStatistics = statistic.killsEquipmentStatistics.HullStatistics;
            e.KillTurretStatistics = statistic.killsEquipmentStatistics.TurretStatistics;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(SelfUserComponent))]
        public class RemoteUserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class ProfileScreenNode : Node {
            public ProfileScreenComponent profileScreen;

            public ScreenGroupComponent screenGroup;
        }

        public class ProfileScreenContextNode : Node {
            public ProfileScreenContextComponent profileScreenContext;
            public ScreenGroupComponent screenGroup;
        }

        public class UserStatisticsNode : Node {
            public FavoriteEquipmentStatisticsComponent favoriteEquipmentStatistics;

            public KillsEquipmentStatisticsComponent killsEquipmentStatistics;
            public UserStatisticsComponent userStatistics;
        }
    }
}