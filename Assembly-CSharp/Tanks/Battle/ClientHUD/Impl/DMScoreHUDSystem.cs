using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class DMScoreHUDSystem : ECSSystem {
        [OnEventFire]
        public void ActivateForTank(NodeAddedEvent e, HUDNode hud, HUDNodes.SelfBattleUserAsTankNode battleUser, SingleNode<DMHUDMessagesComponent> messages,
            [JoinByBattle] BattleNode battle) {
            hud.dmScoreHUD.gameObject.SetActive(true);
            hud.mainHUD.ShowMessageWithPriority(messages.component.MainMessage);
            hud.mainHUD.SetMessageTDMPosition();
        }

        [OnEventFire]
        public void SetScoresTDMPosition(NodeAddedEvent e, SingleNode<TeamScoreHUDComponent> hud, HUDNodes.SelfBattleUserAsTankNode battleUser, [JoinByBattle] BattleNode battle) {
            hud.component.SetTdmMode();
        }

        [OnEventFire]
        public void DeactivateForSpectator(NodeAddedEvent e, SingleNode<DMScoreHUDComponent> hud, HUDNodes.SelfBattleUserAsSpectatorNode battleUser, [JoinByBattle] BattleNode battle) {
            hud.component.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void SetScore(NodeAddedEvent e, SingleNode<DMScoreHUDComponent> hud, HUDNodes.SelfBattleUserNode battleUser, [JoinByBattle] [Context] BattleNode battle) {
            ScheduleEvent<UpdateDMHUDScoreEvent>(battle);
        }

        [OnEventFire]
        public void OnUserEnterBattle(NodeAddedEvent e, RoundUserNode roundUser, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] BattleNode battle) {
            ScheduleEvent<UpdateDMHUDScoreEvent>(battle);
        }

        [OnEventFire]
        public void OnUserExitBattle(NodeRemoveEvent e, RoundUserNode roundUser, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] BattleNode battle) {
            NewEvent<UpdateDMHUDScoreEvent>().Attach(battle).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateScore(RoundScoreUpdatedEvent e, RoundNode round, [JoinByBattle] HUDNodes.SelfBattleUserNode selfBattleUser, [JoinByBattle] BattleNode battle) {
            ScheduleEvent<UpdateDMHUDScoreEvent>(battle);
        }

        [OnEventFire]
        public void UpdateScore(RoundUserStatisticsUpdatedEvent e, RoundUserNode roundUser, [JoinByBattle] BattleNode battle, [JoinByBattle] HUDNodes.SelfBattleUserNode selfBattleUser) {
            ScheduleEvent<UpdateDMHUDScoreEvent>(battle);
        }

        [OnEventFire]
        public void UpdateSelfScore(UpdateDMHUDScoreEvent e, BattleNode battle, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByUser] RoundUserNode selfRoundUser,
            [JoinAll] SingleNode<DMScoreHUDComponent> hud) {
            hud.component.Place = selfRoundUser.roundUserStatistics.Place;
            hud.component.PlayerScore = selfRoundUser.roundUserStatistics.ScoreWithoutBonuses;
        }

        [OnEventFire]
        public void UpdateMaxScore(UpdateDMHUDScoreEvent e, BattleNode battle, [JoinByBattle] Optional<ScoreLimitNode> scoreLimit, BattleNode battle2,
            [JoinByBattle] HUDNodes.SelfBattleUserAsTankNode self, BattleNode battle3, [JoinByBattle] ICollection<RoundUserNode> roundUsers,
            [JoinAll] SingleNode<DMScoreHUDComponent> hud) {
            hud.component.MaxScore = !scoreLimit.IsPresent() ? roundUsers.Min().roundUserStatistics.ScoreWithoutBonuses : scoreLimit.Get().scoreLimit.ScoreLimit;
            hud.component.Players = roundUsers.Count;
        }

        public class RoundUserNode : Node, IComparable<RoundUserNode> {
            public BattleGroupComponent battleGroup;

            public RoundUserStatisticsComponent roundUserStatistics;
            public UserGroupComponent userGroup;

            public int CompareTo(RoundUserNode other) => roundUserStatistics.CompareTo(other.roundUserStatistics);
        }

        public class RoundNode : Node {
            public BattleGroupComponent battleGroup;

            public RoundComponent round;
        }

        public class BattleNode : Node {
            public DMComponent dm;
        }

        public class ScoreLimitNode : Node {
            public ScoreLimitComponent scoreLimit;
        }

        public class HUDNode : Node {
            public DMScoreHUDComponent dmScoreHUD;
            public MainHUDComponent mainHUD;
        }

        public class UpdateDMHUDScoreEvent : Event { }
    }
}