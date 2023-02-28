using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class TeamScoreHUDSystem : ECSSystem {
        [OnEventComplete]
        public void ScoreUpdate(RoundScoreUpdatedEvent e, RoundNode node, [JoinByBattle] ScoreBattleNode battle, [JoinByBattle] HUDNodes.SelfBattleUserNode self,
            [JoinByBattle] ICollection<TeamNode> teams, [JoinAll] SingleNode<TeamScoreHUDComponent> hud) {
            SetScore(teams, hud.component);
        }

        [OnEventFire]
        public void InitIndicator(NodeAddedEvent e, ScoreBattleNode battle, [Context] [JoinByBattle] RoundNode round, [Context] [JoinByBattle] HUDNodes.SelfBattleUserNode self,
            [JoinByBattle] ICollection<TeamNode> teams, [Context] SingleNode<TeamScoreHUDComponent> hud) {
            hud.component.gameObject.SetActive(true);
            SetScore(teams, hud.component);
        }

        void SetScore(ICollection<TeamNode> teams, TeamScoreHUDComponent hud) {
            int redScore = 0;
            int blueScore = 0;

            foreach (TeamNode team in teams) {
                switch (team.colorInBattle.TeamColor) {
                    case TeamColor.RED:
                        redScore = team.teamScore.Score;
                        break;

                    case TeamColor.BLUE:
                        blueScore = team.teamScore.Score;
                        break;
                }
            }

            hud.BlueScore = blueScore;
            hud.RedScore = redScore;
        }

        [OnEventFire]
        public void SetScoresTDMPosition(NodeAddedEvent e, SingleNode<TeamScoreHUDComponent> hud, HUDNodes.SelfTankNode tank, [JoinByBattle] TDMBattleNode battle) {
            hud.component.SetTdmMode();
        }

        [OnEventFire]
        public void SetScoresCTFPosition(NodeAddedEvent e, SingleNode<TeamScoreHUDComponent> hud, HUDNodes.SelfTankNode tank, [JoinByBattle] CTFBattleNode battle) {
            hud.component.SetCtfMode();
        }

        [OnEventFire]
        public void SetTDMMessage(NodeAddedEvent e, HUDNode hud, HUDNodes.SelfTankNode tank, SingleNode<TDMHUDMessagesComponent> messageNode, [JoinByBattle] TDMBattleNode battle) {
            hud.mainHUD.ShowMessageWithPriority(messageNode.component.MainMessage);
            hud.mainHUD.SetMessageTDMPosition();
        }

        [OnEventFire]
        public void SetCTFMessage(NodeAddedEvent e, HUDNode hud, HUDNodes.SelfTankNode tank, SingleNode<CTFHUDMessagesComponent> messageNode, [JoinByBattle] CTFBattleNode battle) {
            hud.mainHUD.ShowMessageWithPriority(messageNode.component.CaptureFlagMessage);
            hud.mainHUD.SetMessageCTFPosition();
        }

        [OnEventFire]
        public void InitFlags(NodeAddedEvent e, CTFBattleNode battle, [Context] [JoinByBattle] RoundNode round, [Context] [JoinByBattle] HUDNodes.SelfBattleUserNode self,
            [Context] SingleNode<FlagsHUDComponent> hud) {
            hud.component.BlueFlagNormalizedPosition = 0f;
            hud.component.RedFlagNormalizedPosition = 0f;
            hud.component.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void DisableFlags(NodeRemoveEvent e, CTFBattleNode battle, SingleNode<FlagsHUDComponent> hud) {
            hud.component.gameObject.SetActive(false);
        }

        public class ScoreBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleScoreComponent battleScore;

            public TeamBattleComponent teamBattle;
        }

        public class TDMBattleNode : ScoreBattleNode {
            public TDMComponent tdm;
        }

        public class CTFBattleNode : ScoreBattleNode {
            public CTFComponent ctf;
        }

        public class RoundNode : Node {
            public BattleGroupComponent battleGroup;

            public RoundComponent round;
        }

        public class TeamNode : Node {
            public ColorInBattleComponent colorInBattle;
            public TeamComponent team;

            public TeamGroupComponent teamGroup;

            public TeamScoreComponent teamScore;
        }

        public class HUDNode : Node {
            public MainHUDComponent mainHUD;
        }
    }
}