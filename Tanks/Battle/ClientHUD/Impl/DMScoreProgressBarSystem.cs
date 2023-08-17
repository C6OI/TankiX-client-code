using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class DMScoreProgressBarSystem : ECSSystem {
        static readonly List<RoundUserNode> roundUsers = new();

        [OnEventFire]
        public void JoinProgressBarToSelfUser(NodeAddedEvent e,
            [Combine] SingleNode<DMBattleScoreIndicatorComponent> scoreProgressBar, SelfBattleUserNode selfBattleUser) {
            if (!scoreProgressBar.Entity.HasComponent<UserGroupComponent>()) {
                scoreProgressBar.Entity.AddComponent(new UserGroupComponent(selfBattleUser.userGroup.Key));
            }
        }

        [OnEventFire]
        public void InitKillsForLimitedBattle(NodeAddedEvent e, ScoreProgressBarNode scoreProgressBarNode,
            [JoinByUser] [Context] RoundUserNode roundUser, [JoinByBattle] ScoreLimitedBattleNode battle) {
            scoreProgressBarNode.dmBattleScoreIndicator.LimitVisible = true;
            UpdateScoreBarForLimitedBattle(scoreProgressBarNode, battle, roundUser);
        }

        [OnEventFire]
        public void InitKillsForUnlimitedBattle(NodeAddedEvent e, ScoreProgressBarNode scoreProgressBarNode,
            [JoinByUser] [Context] RoundUserNode roundUser, [JoinByUser] SelfBattleUserNode selfBattleUser,
            [JoinByBattle] ScoreUnlimitedBattleNode battle, [JoinAll] ICollection<RoundUserNode> allRoundUsers) =>
            UpdateScoreBarForUnlimitedBattle(scoreProgressBarNode, selfBattleUser, allRoundUsers);

        [OnEventFire]
        public void InitKillsForLimitedBattle(NodeAddedEvent e, ScoreProgressBarNode scoreProgressBarNode,
            [Context] [JoinByUser] SelfBattleSpectatorNode spectator, [JoinByBattle] ScoreLimitedBattleNode battle,
            [JoinAll] ICollection<RoundUserNode> allRoundUsers) {
            scoreProgressBarNode.dmBattleScoreIndicator.LimitVisible = true;
            UpdateScoreBarForLimitedBattle(scoreProgressBarNode, battle, allRoundUsers);
        }

        [OnEventFire]
        public void InitKillsForUnlimitedBattle(NodeAddedEvent e, ScoreProgressBarNode scoreProgressBarNode,
            [JoinByUser] [Context] SelfBattleSpectatorNode spectator, [JoinByBattle] ScoreUnlimitedBattleNode battle,
            [JoinAll] ICollection<RoundUserNode> allRoundUsers) =>
            UpdateScoreBarForUnlimitedBattle(scoreProgressBarNode, allRoundUsers);

        [OnEventFire]
        public void UpdateKills(RoundUserStatisticsUpdatedEvent e, RoundUserNode roundUser,
            [JoinByUser] SelfBattleUserNode selfBattleUser, [JoinByBattle] ScoreLimitedBattleNode battle,
            [JoinAll] ScoreProgressBarNode scoreProgressBarNode) =>
            UpdateScoreBarForLimitedBattle(scoreProgressBarNode, battle, roundUser);

        [OnEventFire]
        public void UpdateKills(RoundScoreUpdatedEvent e, RoundNode round, [JoinByBattle] SelfBattleUserNode selfBattleUser,
            [JoinByBattle] ScoreUnlimitedBattleNode battle, [JoinAll] ScoreProgressBarNode scoreProgressBarNode,
            [JoinAll] ICollection<RoundUserNode> allRoundUsers) =>
            UpdateScoreBarForUnlimitedBattle(scoreProgressBarNode, selfBattleUser, allRoundUsers);

        [OnEventFire]
        public void OnUserExitBattle(NodeRemoveEvent e, BattleUserNode userNode,
            [JoinByBattle] ScoreUnlimitedBattleNode battle, [JoinByBattle] SelfBattleUserNode selfBattleUser,
            [JoinByBattle] ICollection<RoundUserNode> allRoundUsers, [JoinAll] ScoreProgressBarNode scoreProgressBarNode) {
            foreach (RoundUserNode allRoundUser in allRoundUsers) {
                if (allRoundUser.userGroup.Key != userNode.userGroup.Key) {
                    roundUsers.Add(allRoundUser);
                }
            }

            UpdateScoreBarForUnlimitedBattle(scoreProgressBarNode, selfBattleUser, roundUsers);
            roundUsers.Clear();
        }

        void UpdateScoreBarForLimitedBattle(ScoreProgressBarNode scoreProgressBarNode, ScoreLimitedBattleNode battle,
            RoundUserNode selfRoundUser) {
            DMBattleScoreIndicatorComponent dmBattleScoreIndicator = scoreProgressBarNode.dmBattleScoreIndicator;
            int kills = selfRoundUser.roundUserStatistics.Kills;
            dmBattleScoreIndicator.ScoreLimit = battle.scoreLimit.ScoreLimit;
            UpdateUIElement(dmBattleScoreIndicator, kills, kills / (float)battle.scoreLimit.ScoreLimit);
        }

        void UpdateScoreBarForUnlimitedBattle(ScoreProgressBarNode scoreProgressBarNode, SelfBattleUserNode selfBattleUser,
            ICollection<RoundUserNode> allRoundUsers) {
            RoundUserNode selfRoundUser = getSelfRoundUser(selfBattleUser, allRoundUsers);

            if (selfRoundUser != null) {
                DMBattleScoreIndicatorComponent dmBattleScoreIndicator = scoreProgressBarNode.dmBattleScoreIndicator;
                int kills = selfRoundUser.roundUserStatistics.Kills;
                RoundUserNode roundUserNode = allRoundUsers.Min();
                int kills2 = roundUserNode.roundUserStatistics.Kills;
                bool flag = selfRoundUser.roundUserStatistics.Kills == kills2;
                float value = kills2 != 0 ? kills / (float)kills2 : 0f;
                value = Mathf.Clamp01(value);
                UpdateUIElement(dmBattleScoreIndicator, kills, value);
                scoreProgressBarNode.dmBattleScoreIndicator.ScoreLimit = kills2;
                scoreProgressBarNode.dmBattleScoreIndicator.LimitVisible = kills2 > 0 && !flag;
            } else {
                UpdateScoreBarForUnlimitedBattle(scoreProgressBarNode, allRoundUsers);
            }
        }

        void UpdateScoreBarForLimitedBattle(ScoreProgressBarNode scoreProgressBarNode, ScoreLimitedBattleNode battle,
            ICollection<RoundUserNode> allRoundUsers) {
            DMBattleScoreIndicatorComponent dmBattleScoreIndicator = scoreProgressBarNode.dmBattleScoreIndicator;
            int num = 0;

            if (allRoundUsers.Count() > 0) {
                RoundUserNode roundUserNode = allRoundUsers.Min();
                num = roundUserNode.roundUserStatistics.Kills;
            }

            dmBattleScoreIndicator.ScoreLimit = battle.scoreLimit.ScoreLimit;
            UpdateUIElement(dmBattleScoreIndicator, num, num / (float)battle.scoreLimit.ScoreLimit);
        }

        void UpdateScoreBarForUnlimitedBattle(ScoreProgressBarNode scoreProgressBarNode,
            ICollection<RoundUserNode> allRoundUsers) {
            DMBattleScoreIndicatorComponent dmBattleScoreIndicator = scoreProgressBarNode.dmBattleScoreIndicator;
            int kills = 0;

            if (allRoundUsers.Count() > 0) {
                RoundUserNode roundUserNode = allRoundUsers.Min();
                kills = roundUserNode.roundUserStatistics.Kills;
            }

            UpdateUIElement(dmBattleScoreIndicator, kills, 0f);
            scoreProgressBarNode.dmBattleScoreIndicator.LimitVisible = false;
        }

        void UpdateUIElement(DMBattleScoreIndicatorComponent scoreProgressBar, int kills, float progressValue) {
            scoreProgressBar.Score = kills;
            scoreProgressBar.ProgressValue = progressValue;
        }

        static RoundUserNode getSelfRoundUser(SelfBattleUserNode selfBattleUser, ICollection<RoundUserNode> allRoundUsers) {
            foreach (RoundUserNode allRoundUser in allRoundUsers) {
                if (allRoundUser.userGroup.Key == selfBattleUser.userGroup.Key) {
                    return allRoundUser;
                }
            }

            return null;
        }

        public class RoundUserNode : Node, IComparable<RoundUserNode> {
            public BattleGroupComponent battleGroup;

            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;

            public int CompareTo(RoundUserNode other) => roundUserStatistics.CompareTo(other.roundUserStatistics);
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;

            public int CompareTo(RoundUserNode other) => roundUserStatistics.CompareTo(other.roundUserStatistics);
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;
        }

        public class SelfBattleSpectatorNode : SelfBattleUserNode {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class ScoreProgressBarNode : Node {
            public DMBattleScoreIndicatorComponent dmBattleScoreIndicator;
            public UserGroupComponent userGroup;
        }

        public class RoundNode : Node {
            public BattleGroupComponent battleGroup;

            public RoundComponent round;
        }

        public class ScoreLimitedBattleNode : Node {
            public DMComponent dm;

            public ScoreLimitComponent scoreLimit;
        }

        [Not(typeof(ScoreLimitComponent))]
        public class ScoreUnlimitedBattleNode : Node {
            public DMComponent dm;
        }
    }
}