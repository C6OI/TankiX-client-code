using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ProfileSummarySectionUISystem : ECSSystem {
        [OnEventFire]
        public void SetLevelInfo(NodeAddedEvent e, ProfileSummarySectionUINode sectionUI, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserStatisticsNode statistics,
            [JoinAll] RanksNamesNode rankNames) {
            GetUserLevelInfoEvent getUserLevelInfoEvent = new();
            ScheduleEvent(getUserLevelInfoEvent, statistics);
            sectionUI.profileSummarySectionUI.SetLevelInfo(getUserLevelInfoEvent.Info, rankNames.ranksNames.Names[getUserLevelInfoEvent.Info.Level + 1]);
        }

        [OnEventFire]
        public void SetLeagueInfo(NodeAddedEvent e, ProfileSummarySectionUINode sectionUI, ProfileScreenWithUserGroupNode profileScreen, [Context] LeagueUINode uiNode,
            [JoinByUser] UserWithLeagueNode user, [JoinByLeague] [Context] LeagueNode league) {
            uiNode.leagueUI.SetLeague(league.leagueName.Name, league.leagueIcon.SpriteUid, user.userReputation.Reputation);
            sectionUI.profileSummarySectionUI.showRewardsButton.SetActive(user.Entity.HasComponent<SelfUserComponent>());
        }

        [OnEventFire]
        public void HideLeaguePlaceInfo(NodeRemoveEvent e, LeaguePlaceUINode leaguePlaceUI) {
            leaguePlaceUI.leaguePlaceUI.Hide();
        }

        [OnEventFire]
        public void ShowLeaguePlaceInfo(UpdateTopLeagueInfoEvent e, UserWithLeagueNode selfUser, [JoinAll] LeaguePlaceUINode leaguePlaceUI, [JoinByUser] UserWithLeagueNode user) {
            if (e.UserId.Equals(user.Entity.Id)) {
                leaguePlaceUI.leaguePlaceUI.SetPlace(e.Place);
            }
        }

        [OnEventFire]
        public void SetStatisticsInfo(NodeAddedEvent e, ProfileSummarySectionUINode sectionUI, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] UserStatisticsNode statistics) {
            Dictionary<string, long> statistics2 = statistics.userStatistics.Statistics;
            long parameterValue = StatsTool.GetParameterValue(statistics2, "VICTORIES");
            long parameterValue2 = StatsTool.GetParameterValue(statistics2, "DEFEATS");
            long battlesCount = StatsTool.GetParameterValue(statistics2, "ALL_BATTLES_PARTICIPATED") - StatsTool.GetParameterValue(statistics2, "ALL_CUSTOM_BATTLES_PARTICIPATED");
            sectionUI.profileSummarySectionUI.SetWinLossStatistics(parameterValue, parameterValue2, battlesCount);
        }

        [OnEventFire]
        public void GetUserTotalBattlesCount(GetChangeTurretTutorialValidationDataEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> selfUser,
            [JoinByUser] UserStatisticsNode statistics) {
            Dictionary<string, long> statistics2 = statistics.userStatistics.Statistics;
            e.BattlesCount = StatsTool.GetParameterValue(statistics2, "ALL_BATTLES_PARTICIPATED") - StatsTool.GetParameterValue(statistics2, "ALL_CUSTOM_BATTLES_PARTICIPATED");
        }

        [OnEventFire]
        public void SetEquipmentStatisticsInfo(NodeAddedEvent e, SingleNode<MostPlayedEquipmentUIComponent> ui, ProfileScreenWithUserGroupNode profileScreen,
            [JoinByUser] UserStatisticsNode statistics, [JoinAll] SingleNode<SelectedPresetComponent> selectedPreset) {
            Dictionary<long, long> hullStatistics = statistics.favoriteEquipmentStatistics.HullStatistics;
            Dictionary<long, long> turretStatistics = statistics.favoriteEquipmentStatistics.TurretStatistics;

            if (hullStatistics.Any() || turretStatistics.Any()) {
                ui.component.SwitchState(true);
                Entity entityById = GetEntityById(StatsTool.GetItemWithLagestValue(hullStatistics));
                Entity entityById2 = GetEntityById(StatsTool.GetItemWithLagestValue(turretStatistics));
                string name = entityById.GetComponent<DescriptionItemComponent>().Name;
                string name2 = entityById2.GetComponent<DescriptionItemComponent>().Name;
                string hullUID = entityById.GetComponent<MarketItemGroupComponent>().Key.ToString();
                string turretUID = entityById2.GetComponent<MarketItemGroupComponent>().Key.ToString();
                ui.component.SetMostPlayed(turretUID, name2, hullUID, name);
            } else {
                ui.component.SwitchState(false);
            }
        }

        [OnEventFire]
        public void ShowLeagueReward(ButtonClickEvent e, PreviewLegaueRewardButtonNode previewLeagueRewardButton, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            LeagueRewardDialog leagueRewardDialog = dialogs.component.Get<LeagueRewardDialog>();
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            leagueRewardDialog.Show(animators);
        }

        public class ProfileSummarySectionUINode : Node {
            public ProfileSummarySectionUIComponent profileSummarySectionUI;
        }

        public class UserStatisticsNode : Node {
            public FavoriteEquipmentStatisticsComponent favoriteEquipmentStatistics;

            public KillsEquipmentStatisticsComponent killsEquipmentStatistics;
            public UserStatisticsComponent userStatistics;
        }

        public class PreviewLegaueRewardButtonNode : Node {
            public PreviewLeagueRewardButtonComponent previewLeagueRewardButton;
        }

        public class LeagueUINode : Node {
            public LeagueUIComponent leagueUI;
        }

        public class LeaguePlaceUINode : Node {
            public LeaguePlaceUIComponent leaguePlaceUI;
        }

        public class UserWithLeagueNode : Node {
            public LeagueGroupComponent leagueGroup;
            public UserGroupComponent userGroup;

            public UserReputationComponent userReputation;
        }

        public class LeagueNode : Node {
            public LeagueComponent league;

            public LeagueGroupComponent leagueGroup;

            public LeagueIconComponent leagueIcon;

            public LeagueNameComponent leagueName;
        }

        public class ProfileScreenWithUserGroupNode : Node {
            public ActiveScreenComponent activeScreen;
            public ProfileScreenComponent profileScreen;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(UserNotificatorRankNamesComponent))]
        public class RanksNamesNode : Node {
            public RanksNamesComponent ranksNames;
        }
    }
}