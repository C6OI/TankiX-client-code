using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LeagueRewardScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitLeagueScreen(NodeAddedEvent e, SingleNode<LeagueRewardUIComponent> screen, [JoinAll] ICollection<LeagueNode> leagueNodes,
            [JoinAll] UserWithLeagueNode userWithLeagueNode, [JoinAll] SingleNode<SeasonEndDateComponent> leaguesConfig) {
            Entity entity = null;

            foreach (LeagueNode item in SortedLeagues(leagueNodes)) {
                LeagueTitleUIComponent leagueTitleUIComponent = screen.component.AddLeagueItem(item.Entity);
                leagueTitleUIComponent.Init(item.Entity);

                if (item.leagueGroup.Key == userWithLeagueNode.leagueGroup.Key) {
                    entity = item.Entity;
                }
            }

            screen.component.SetChestScoreLimit(userWithLeagueNode.gameplayChestScore.Limit);
            screen.component.SetLeaguesCount(leagueNodes.Count);
            screen.component.SelectUserLeague(entity, userWithLeagueNode.userReputation.Reputation);
            screen.component.SetSeasonEndDate(leaguesConfig.component.EndDate);
            screen.component.SelectBar(0);
        }

        [OnEventFire]
        public void GetLeagueByIndex(GetLeagueByIndexEvent e, Node any, [JoinAll] ICollection<LeagueNode> leagues) {
            foreach (LeagueNode item in SortedLeagues(leagues)) {
                if (item.leagueConfig.LeagueIndex != e.index) {
                    continue;
                }

                e.leagueEntity = item.Entity;
                break;
            }
        }

        [OnEventFire]
        public void PutReputationToEnter(UpdateTopLeagueInfoEvent e, UserWithLeagueNode userWithLeagueNode, [JoinAll] SingleNode<LeagueRewardUIComponent> screen,
            [JoinAll] TopLeagueNode topLeague) {
            screen.component.PlaceInTopLeague = e.Place;
            screen.component.PutReputationToEnter(topLeague.Entity.Id, e.LastPlaceReputation);
            screen.component.UpdateLeagueRewardUI();
        }

        static List<LeagueNode> SortedLeagues(ICollection<LeagueNode> leagueNodes) {
            List<LeagueNode> list = leagueNodes.ToList();
            list.Sort(LeagueCompare);
            return list;
        }

        static int LeagueCompare(LeagueNode a, LeagueNode b) => a.leagueConfig.LeagueIndex - b.leagueConfig.LeagueIndex;

        [OnEventFire]
        public void AttachLeagueRewardDialogToUserGroup(NodeAddedEvent e, SingleNode<LeagueRewardDialog> leagueRewardDialog, [JoinAll] UserWithLeagueNode userWithLeagueNode) {
            userWithLeagueNode.userGroup.Attach(leagueRewardDialog.Entity);
        }

        public class UserWithLeagueNode : Node {
            public GameplayChestScoreComponent gameplayChestScore;

            public LeagueGroupComponent leagueGroup;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserReputationComponent userReputation;
        }

        public class LeagueNode : Node {
            public ChestBattleRewardComponent chestBattleReward;

            public CurrentSeasonRewardForClientComponent currentSeasonRewardForClient;
            public LeagueComponent league;

            public LeagueConfigComponent leagueConfig;

            public LeagueGroupComponent leagueGroup;

            public LeagueIconComponent leagueIcon;

            public LeagueNameComponent leagueName;
        }

        public class TopLeagueNode : Node {
            public TopLeagueComponent topLeague;
        }
    }
}