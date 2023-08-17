using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientHUD.Impl {
    public class CombatEventLogSystem : ECSSystem {
        [OnEventFire]
        public void AddUILogComponent(NodeAddedEvent evt, SingleNode<CombatEventLogComponent> combatEventLog) =>
            combatEventLog.Entity.AddComponent(new UILogComponent(CombatEventLogUtil.GetUILog(combatEventLog.component)));

        [OnEventFire]
        public void RedirectEventToTargetOnTargetDeath(KillEvent e, BattleUserNode battleUser, [JoinByUser] UserNode user,
            BattleUserNode battleUser2Team, [JoinByTeam] Optional<TeamNode> team) {
            ShowMessageAfterKilledEvent showMessageAfterKilledEvent = new();
            showMessageAfterKilledEvent.KillerUserUid = user.userUid.Uid;
            showMessageAfterKilledEvent.killerRank = user.userRank.Rank;
            showMessageAfterKilledEvent.killerTeam = team.IsPresent() ? team.Get().teamColor.TeamColor : TeamColor.NONE;
            ScheduleEvent(showMessageAfterKilledEvent, e.Target);
        }

        [OnEventFire]
        public void ShowMessageOnUserSuicides(SelfDestructionBattleUserEvent e, BattleUserNode user,
            [JoinByUser] UserNode suicidedUser, BattleUserNode user2Team, [JoinByTeam] Optional<TeamNode> team,
            [JoinAll] CombatEventLogNode combatEventLog) {
            string suicideMessage = combatEventLog.combatLogCommonMessages.SuicideMessage;
            Color teamColor = GetTeamColor(team, combatEventLog);

            suicideMessage = CombatEventLogUtil.ApplyPlaceholder(suicideMessage,
                "{user}",
                suicidedUser.userRank.Rank,
                suicidedUser.userUid.Uid,
                teamColor);

            combatEventLog.uiLog.UILog.AddMessage(suicideMessage);
        }

        [OnEventFire]
        public void ShowKilledMessage(ShowMessageAfterKilledEvent e, TankNode victimTank, [JoinByUser] UserNode victimUser,
            TankNode victimTank2Team, [JoinByTeam] Optional<TeamNode> team, [JoinAll] CombatEventLogNode combatEventLog) {
            string killMessage = combatEventLog.combatLogCommonMessages.KillMessage;

            killMessage = CombatEventLogUtil.ApplyPlaceholder(killMessage,
                "{victim}",
                victimUser.userRank.Rank,
                victimUser.userUid.Uid,
                GetTeamColor(team, combatEventLog));

            killMessage = CombatEventLogUtil.ApplyPlaceholder(killMessage,
                "{killer}",
                e.killerRank,
                e.KillerUserUid,
                CombatEventLogUtil.GetTeamColor(e.killerTeam, combatEventLog.combatEventLog));

            combatEventLog.uiLog.UILog.AddMessage(killMessage);
        }

        [OnEventFire]
        public void OnUserAdded(NodeAddedEvent e, TeamBattleUserNode teamBattleUserNode,
            [JoinByUser] NotSelfUserNode userNode, TeamBattleUserNode teamBattleUser2Node, [JoinByTeam] TeamNode teamNode,
            [JoinAll] CombatEventLogNode combatEventLogNode) =>
            AddUserAddedMessage(userNode, teamNode.teamColor.TeamColor, combatEventLogNode);

        [OnEventFire]
        public void OnUserAdded(NodeAddedEvent e, BattleUserNode battleUser, [JoinByUser] NotSelfUserNode userNode,
            [JoinByBattle] SingleNode<DMComponent> dm, [JoinAll] CombatEventLogNode combatEventLogNode) =>
            AddUserAddedMessage(userNode, TeamColor.NONE, combatEventLogNode);

        void AddUserAddedMessage(NotSelfUserNode userNode, TeamColor userTeamColor, CombatEventLogNode combatEventLogNode) {
            string userJoinBattleMessage = combatEventLogNode.combatLogCommonMessages.UserJoinBattleMessage;
            Color teamColor = CombatEventLogUtil.GetTeamColor(userTeamColor, combatEventLogNode.combatEventLog);

            userJoinBattleMessage = CombatEventLogUtil.ApplyPlaceholder(userJoinBattleMessage,
                "{user}",
                userNode.userRank.Rank,
                userNode.userUid.Uid,
                teamColor);

            combatEventLogNode.uiLog.UILog.AddMessage(userJoinBattleMessage);
        }

        [OnEventFire]
        public void NotifyAboutUserExit(NodeRemoveEvent e, BattleUserNode battleUser, [JoinByUser] UserNode user,
            BattleUserNode battleUser2Team, [JoinByTeam] Optional<TeamNode> team,
            [JoinAll] CombatEventLogNode combatEventLog) {
            string userLeaveBattleMessage = combatEventLog.combatLogCommonMessages.UserLeaveBattleMessage;

            userLeaveBattleMessage = CombatEventLogUtil.ApplyPlaceholder(userLeaveBattleMessage,
                "{user}",
                user.userRank.Rank,
                user.userUid.Uid,
                GetTeamColor(team, combatEventLog));

            combatEventLog.uiLog.UILog.AddMessage(userLeaveBattleMessage);
        }

        [OnEventFire]
        public void NotifyAboutScheduledGold(GoldScheduledNotificationEvent evt, Node any,
            [JoinAll] CombatEventLogNode combatEventLog) {
            string goldScheduledMessage = combatEventLog.combatLogCommonMessages.GoldScheduledMessage;
            combatEventLog.uiLog.UILog.AddMessage(goldScheduledMessage);
        }

        [OnEventFire]
        public void NotifyAboutTakenGold(GoldTakenNotificationEvent e, BattleUserNode battleUser, [JoinByUser] UserNode user,
            [JoinByUser] RoundUserNode roundUser, [JoinByTeam] Optional<TeamNode> team,
            [JoinAll] CombatEventLogNode combatEventLog) {
            string goldTakenMessage = combatEventLog.combatLogCommonMessages.GoldTakenMessage;

            goldTakenMessage = CombatEventLogUtil.ApplyPlaceholder(goldTakenMessage,
                "{user}",
                user.userRank.Rank,
                user.userUid.Uid,
                GetTeamColor(team, combatEventLog));

            combatEventLog.uiLog.UILog.AddMessage(goldTakenMessage);
        }

        [OnEventFire]
        public void ActivateCombatLog(NodeAddedEvent e, SelfBattleUserNode selfBattleUser,
            InactiveCombatLogNode combatLog) => combatLog.Entity.AddComponent<ActiveCombatLogComponent>();

        [OnEventFire]
        public void DeactivateCombatLogOnExit(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser,
            [JoinAll] CombatEventLogNode combatEventLog) =>
            combatEventLog.Entity.RemoveComponent<ActiveCombatLogComponent>();

        [OnEventFire]
        public void ClearCombatLogOnExit(NodeRemoveEvent e, CombatEventLogNode combatEventLog) =>
            combatEventLog.uiLog.UILog.Clear();

        Color GetTeamColor(Optional<TeamNode> team, CombatEventLogNode combatEventLog) {
            TeamColor teamColor = team.IsPresent() ? team.Get().teamColor.TeamColor : TeamColor.NONE;
            return CombatEventLogUtil.GetTeamColor(teamColor, combatEventLog.combatEventLog);
        }

        public class TankNode : Node {
            public TankComponent tank;
            public UserGroupComponent userGroup;
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleUserComponent battleUser;

            public UserGroupComponent userGroup;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class TeamBattleUserNode : BattleUserNode {
            public TeamGroupComponent teamGroup;
        }

        public class SelfBattleUserNode : Node {
            public BattleUserComponent battleUser;

            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;

            public UserUidComponent userUid;
        }

        public class SelfUserNode : UserNode {
            public SelfUserComponent selfUser;
        }

        [Not(typeof(SelfUserComponent))]
        public class NotSelfUserNode : UserNode { }

        public class CombatEventLogNode : Node {
            public ActiveCombatLogComponent activeCombatLog;

            public CombatEventLogComponent combatEventLog;
            public CombatLogCommonMessagesComponent combatLogCommonMessages;

            public UILogComponent uiLog;
        }

        [Not(typeof(ActiveCombatLogComponent))]
        public class InactiveCombatLogNode : Node {
            public CombatEventLogComponent combatEventLog;
            public CombatLogCommonMessagesComponent combatLogCommonMessages;
        }

        public class RoundUserNode : Node {
            public RoundUserComponent roundUser;

            public UserGroupComponent userGroup;
        }

        public class TeamNode : Node {
            public TeamColorComponent teamColor;
            public TeamGroupComponent teamGroup;
        }

        public class ShowMessageAfterKilledEvent : Event {
            public int killerRank;

            public TeamColor killerTeam;

            public string KillerUserUid { get; set; }
        }
    }
}