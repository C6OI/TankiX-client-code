using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TeamBattleScoreIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void ShowTDMScoreIndicator(NodeAddedEvent e, SingleNode<TDMComponent> tdmBattle, [Context] [JoinByBattle] ScoreIndicatorContainserNode indicatorContainer) {
            indicatorContainer.teamBattleScoreIndicatorContainer.TdmScoreIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowCTFScoreIndicator(NodeAddedEvent e, SingleNode<CTFComponent> ctfBattle, [Context] [JoinByBattle] ScoreIndicatorContainserNode indicatorContainer) {
            indicatorContainer.teamBattleScoreIndicatorContainer.CtfScoreIndicator.SetActive(true);
        }

        [OnEventFire]
        public void HideTeamScoreIndicator(NodeRemoveEvent e, SingleNode<BattleComponent> battle, [Context] [JoinByBattle] ScoreIndicatorContainserNode indicatorContainer) {
            indicatorContainer.teamBattleScoreIndicatorContainer.TdmScoreIndicator.SetActive(false);
            indicatorContainer.teamBattleScoreIndicatorContainer.CtfScoreIndicator.SetActive(false);
        }

        [OnEventComplete]
        public void ScoreUpdate(RoundScoreUpdatedEvent e, RoundNode node, [JoinByBattle] BattleWithLimitNode battleWithLimit, [JoinByBattle] IndicatorNode indicator,
            [JoinByBattle] ICollection<TeamNode> teams) {
            int redScore = 0;
            int blueScore = 0;

            foreach (TeamNode team in teams) {
                switch (team.teamColor.TeamColor) {
                    case TeamColor.RED:
                        redScore = team.teamScore.Score;
                        break;

                    case TeamColor.BLUE:
                        blueScore = team.teamScore.Score;
                        break;
                }
            }

            indicator.teamBattleScoreIndicator.UpdateScore(blueScore, redScore, battleWithLimit.scoreLimit.ScoreLimit);
        }

        [OnEventComplete]
        public void ScoreUpdate(RoundScoreUpdatedEvent e, RoundNode node, [JoinByBattle] BattleWithoutLimitNode battleWithoutLimit, [JoinByBattle] IndicatorNode indicator,
            [JoinByBattle] ICollection<TeamNode> teams) {
            int num = 0;
            int num2 = 0;

            foreach (TeamNode team in teams) {
                switch (team.teamColor.TeamColor) {
                    case TeamColor.RED:
                        num = team.teamScore.Score;
                        break;

                    case TeamColor.BLUE:
                        num2 = team.teamScore.Score;
                        break;
                }
            }

            indicator.teamBattleScoreIndicator.UpdateScore(num2, num, Mathf.Max(num2, num));
        }

        [OnEventFire]
        public void InitIndicator(NodeAddedEvent e, BattleWithoutLimitNode battleWithoutLimit, [Context] [JoinByBattle] RoundNode round,
            [Context] [JoinByBattle] IndicatorNode indicator) {
            int scoreRed = battleWithoutLimit.battleScore.ScoreRed;
            int scoreBlue = battleWithoutLimit.battleScore.ScoreBlue;
            indicator.teamBattleScoreIndicator.UpdateScore(scoreBlue, scoreRed, Mathf.Max(scoreRed, scoreBlue));
        }

        [OnEventFire]
        public void InitIndicator(NodeAddedEvent e, BattleWithLimitNode battleWithLimit, [Context] [JoinByBattle] RoundNode round, [Context] [JoinByBattle] IndicatorNode indicator) {
            indicator.teamBattleScoreIndicator.UpdateScore(battleWithLimit.battleScore.ScoreBlue, battleWithLimit.battleScore.ScoreRed, battleWithLimit.scoreLimit.ScoreLimit);
        }

        public class IndicatorNode : Node {
            public BattleGroupComponent battleGroup;
            public TeamBattleScoreIndicatorComponent teamBattleScoreIndicator;
        }

        public class ScoreBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleScoreComponent battleScore;

            public TeamBattleComponent teamBattle;
        }

        public class RoundNode : Node {
            public BattleGroupComponent battleGroup;

            public RoundComponent round;
        }

        public class BattleWithLimitNode : ScoreBattleNode {
            public ScoreLimitComponent scoreLimit;
        }

        [Not(typeof(ScoreLimitComponent))]
        public class BattleWithoutLimitNode : ScoreBattleNode { }

        public class TeamNode : Node {
            public TeamComponent team;

            public TeamColorComponent teamColor;

            public TeamGroupComponent teamGroup;

            public TeamScoreComponent teamScore;
        }

        public class ScoreIndicatorContainserNode : Node {
            public BattleGroupComponent battleGroup;
            public TeamBattleScoreIndicatorContainerComponent teamBattleScoreIndicatorContainer;
        }
    }
}