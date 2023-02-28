using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultsScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitDMScreen(NodeAddedEvent e, DMScoreTableNode scoreTable, BattleResultsNode battleResults, SingleNode<DMBattleResultsScreenComponent> screen,
            [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<FriendsComponent> friends) {
            ScrollRect component = scoreTable.dmScoreTable.gameObject.GetComponent<ScrollRect>();
            component.content.anchoredPosition = default;
            PlayerStatInfoUI rowPrefab = scoreTable.battleResultsScoreTable.rowPrefab;
            int selfScore = 0;
            int num = 0;

            foreach (UserResult dmUser in battleResults.battleResults.ResultForClient.DmUsers) {
                bool flag = dmUser.UserId == selfUser.userGroup.Key;
                bool containerLeft = false;
                bool isFriend = friends.component.AcceptedFriendsIds.Contains(dmUser.UserId);
                int leagueIndex = dmUser.League.GetComponent<LeagueConfigComponent>().LeagueIndex;
                bool isDm = true;

                if (flag) {
                    selfScore = dmUser.ScoreWithoutPremium;
                }

                PlayerStatInfoUI playerStatInfoUI = Object.Instantiate(rowPrefab);

                playerStatInfoUI.Init(leagueIndex, dmUser.Uid, dmUser.Kills, dmUser.Deaths, dmUser.KillAssists, dmUser.ScoreWithoutPremium,
                    !flag ? Color.white : scoreTable.scoreTableRowColor.selfRowColor, dmUser.HullId, dmUser.WeaponId, dmUser.UserId, battleResults.battleResults.ResultForClient.BattleId,
                    dmUser.AvatarId, flag, isDm, isFriend, containerLeft);

                playerStatInfoUI.transform.SetParent(component.content, false);

                if (dmUser.ScoreWithoutPremium > num) {
                    num = dmUser.ScoreWithoutPremium;
                }
            }

            string name = Flow.Current.EntityRegistry.GetEntity(battleResults.battleResults.ResultForClient.MapId).GetComponent<DescriptionItemComponent>().Name;
            screen.component.Init(selfScore, num, name);
        }

        [OnEventFire]
        public void InitTeamScreen(NodeAddedEvent e, BattleResultsNode battleResults, SingleNode<TeamBattleResultsScreenComponent> screen) {
            BattleResultForClient resultForClient = battleResults.battleResults.ResultForClient;

            if (resultForClient.BattleMode != 0) {
                string name = Flow.Current.EntityRegistry.GetEntity(resultForClient.MapId).GetComponent<DescriptionItemComponent>().Name;

                if (resultForClient.Spectator) {
                    screen.component.Init(resultForClient.BattleMode.ToString(), resultForClient.BlueTeamScore, resultForClient.RedTeamScore, name, true);
                } else if (resultForClient.PersonalResult.UserTeamColor == TeamColor.BLUE) {
                    screen.component.Init(resultForClient.BattleMode.ToString(), resultForClient.BlueTeamScore, resultForClient.RedTeamScore, name, false);
                } else {
                    screen.component.Init(resultForClient.BattleMode.ToString(), resultForClient.RedTeamScore, resultForClient.BlueTeamScore, name, false);
                }
            }
        }

        [OnEventFire]
        public void InitTeamScoreTables(NodeAddedEvent e, [Combine] TeamScoreTableNode scoreTable, BattleResultsNode battleResults, SingleNode<TeamBattleResultsScreenComponent> screen,
            [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<FriendsComponent> friends) {
            BattleResultForClient resultForClient = battleResults.battleResults.ResultForClient;
            ScrollRect component = scoreTable.uiTeam.gameObject.GetComponent<ScrollRect>();
            component.content.anchoredPosition = default;
            PlayerStatInfoUI rowPrefab = scoreTable.battleResultsScoreTable.rowPrefab;
            ICollection<UserResult> collection = null;

            switch (scoreTable.uiTeam.TeamColor) {
                case TeamColor.BLUE:
                    collection = !resultForClient.Spectator && resultForClient.PersonalResult.UserTeamColor != TeamColor.BLUE ? resultForClient.RedUsers : resultForClient.BlueUsers;
                    break;

                case TeamColor.RED:
                    collection = !resultForClient.Spectator && resultForClient.PersonalResult.UserTeamColor != TeamColor.BLUE ? resultForClient.BlueUsers : resultForClient.RedUsers;
                    break;
            }

            foreach (UserResult item in collection) {
                PlayerStatInfoUI playerStatInfoUI = Object.Instantiate(rowPrefab);
                bool flag = item.UserId == selfUser.userGroup.Key;
                bool isFriend = friends.component.AcceptedFriendsIds.Contains(item.UserId);
                bool containerLeft = false;
                int leagueIndex = item.League.GetComponent<LeagueConfigComponent>().LeagueIndex;
                bool isDm = false;

                playerStatInfoUI.Init(leagueIndex, item.Uid, item.Kills, item.Deaths, item.KillAssists, item.ScoreWithoutPremium,
                    !flag ? Color.white : scoreTable.scoreTableRowColor.selfRowColor, item.HullId, item.WeaponId, item.UserId, battleResults.battleResults.ResultForClient.BattleId,
                    item.AvatarId, flag, isDm, isFriend, containerLeft);

                playerStatInfoUI.transform.SetParent(component.content, false);
            }
        }

        public class DMScoreTableNode : Node {
            public BattleResultsScoreTableComponent battleResultsScoreTable;
            public DMScoreTableComponent dmScoreTable;

            public ScoreTableRowColorComponent scoreTableRowColor;
        }

        public class TeamScoreTableNode : Node {
            public BattleResultsScoreTableComponent battleResultsScoreTable;

            public ScoreTableRowColorComponent scoreTableRowColor;
            public UITeamComponent uiTeam;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserExperienceComponent userExperience;
            public UserGroupComponent userGroup;
        }

        public class BattleResultsNode : Node {
            public BattleResultsComponent battleResults;
        }
    }
}