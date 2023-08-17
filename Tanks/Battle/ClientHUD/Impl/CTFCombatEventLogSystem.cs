using System;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class CTFCombatEventLogSystem : ECSSystem {
        [OnEventFire]
        public void CTFStartMessage(NodeAddedEvent e, BattleUserNode selfTank, [JoinByBattle] CTFBattleNode ctfBattle,
            [JoinAll] [Context] CombatLogNode combatEventLog) {
            string battleStartMessage = combatEventLog.combatLogCTFMessages.BattleStartMessage;
            combatEventLog.uiLog.UILog.AddMessage(battleStartMessage);
        }

        [OnEventFire]
        public void AddMessageLog(FlagEvent e, FlagNode flag, [JoinByTank] TankNode tank2Team,
            [JoinByTeam] TeamNode tankTeam, FlagNode flag2User, [JoinByTank] TankNode tank2User, [JoinByUser] UserNode user,
            FlagNode flag2Team, [JoinByTeam] TeamNode flagTeam, [JoinAll] SingleNode<SelfBattleUserComponent> selfUser,
            [JoinByTeam] Optional<TeamNode> selfTeam, [JoinAll] CombatLogNode combatLog) {
            CombatLogCTFMessagesComponent combatLogCTFMessages = combatLog.combatLogCTFMessages;
            string ownFlag = GetOwnFlag(selfTeam, flagTeam, combatLogCTFMessages);

            string message = GetMessage(e, flag.Entity, combatLogCTFMessages)
                .Replace(CombatLogCTFMessagesComponent.OWN, ownFlag);

            Color teamColor = CombatEventLogUtil.GetTeamColor(tankTeam.teamColor.TeamColor, combatLog.combatEventLog);

            message = CombatEventLogUtil.ApplyPlaceholder(message,
                "{user}",
                user.userRank.Rank,
                user.userUid.Uid,
                teamColor);

            combatLog.uiLog.UILog.AddMessage(message);
        }

        [OnEventFire]
        public void AddMessageAutoReturnedFlag(FlagReturnEvent e, NotCarriedFlagNode flag, [JoinByTeam] TeamNode flagTeam,
            [JoinAll] SingleNode<SelfBattleUserComponent> selfUser, [JoinByTeam] Optional<TeamNode> selfTeam,
            [JoinAll] CombatLogNode combatEventLog) {
            CombatLogCTFMessagesComponent combatLogCTFMessages = combatEventLog.combatLogCTFMessages;
            string ownFlag = GetOwnFlag(selfTeam, flagTeam, combatLogCTFMessages);
            string messageText = combatLogCTFMessages.AutoReturned.Replace(CombatLogCTFMessagesComponent.OWN, ownFlag);
            combatEventLog.uiLog.UILog.AddMessage(messageText);
        }

        static string GetOwnFlag(Optional<TeamNode> selfTeam, TeamNode flagTeam,
            CombatLogCTFMessagesComponent logCtfMessages) {
            if (selfTeam.IsPresent()) {
                if (flagTeam.teamGroup.Key == selfTeam.Get().teamGroup.Key) {
                    return logCtfMessages.OurFlag;
                }

                return logCtfMessages.EnemyFlag;
            }

            if (flagTeam.teamColor.TeamColor == TeamColor.BLUE) {
                return logCtfMessages.BlueFlag;
            }

            return logCtfMessages.RedFlag;
        }

        static string GetMessage(FlagEvent e, Entity flag, CombatLogCTFMessagesComponent logCtfMessages) {
            Type type = e.GetType();

            if (type == typeof(FlagPickupEvent)) {
                if (flag.HasComponent<FlagHomeStateComponent>()) {
                    return logCtfMessages.Took;
                }

                return logCtfMessages.PickedUp;
            }

            if (type == typeof(FlagDropEvent)) {
                if (((FlagDropEvent)e).IsUserAction) {
                    return logCtfMessages.Dropped;
                }

                return logCtfMessages.Lost;
            }

            if (type == typeof(FlagDeliveryEvent)) {
                return logCtfMessages.Delivered;
            }

            if (type == typeof(FlagReturnEvent)) {
                return logCtfMessages.Returned;
            }

            throw new ArgumentException();
        }

        [OnEventFire]
        public void FlagHomeStateComponent(NodeAddedEvent e, SingleNode<FlagHomeStateComponent> n) { }

        public class CTFBattleNode : Node {
            public BattleComponent battle;

            public CTFComponent ctf;
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleUserComponent battleUser;
            public SelfComponent self;

            public UserGroupComponent userGroup;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class FlagNode : Node {
            public FlagComponent flag;

            public TeamGroupComponent teamGroup;
        }

        [Not(typeof(TankGroupComponent))]
        public class NotCarriedFlagNode : FlagNode { }

        public class TankNode : Node {
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node {
            public TeamComponent team;

            public TeamColorComponent teamColor;

            public TeamGroupComponent teamGroup;
        }

        public class UserNode : Node {
            public UserComponent user;
            public UserGroupComponent userGroup;

            public UserRankComponent userRank;

            public UserUidComponent userUid;
        }

        public class CombatLogNode : Node {
            public ActiveCombatLogComponent activeCombatLog;

            public CombatEventLogComponent combatEventLog;
            public CombatLogCTFMessagesComponent combatLogCTFMessages;

            public UILogComponent uiLog;
        }
    }
}