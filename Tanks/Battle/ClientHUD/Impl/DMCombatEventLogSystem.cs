using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class DMCombatEventLogSystem : ECSSystem {
        [OnEventFire]
        public void OnBattleStart(NodeAddedEvent e, CombatEventLogNode combatEventLog, BattleUserNode battleUser,
            [JoinByBattle] SingleNode<DMComponent> dm) {
            string battleStartMessage = combatEventLog.combatLogDMMessages.BattleStartMessage;
            combatEventLog.uiLog.UILog.AddMessage(battleStartMessage);
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleUserComponent battleUser;

            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class CombatEventLogNode : Node {
            public ActiveCombatLogComponent activeCombatLog;

            public CombatEventLogComponent combatEventLog;
            public CombatLogDMMessagesComponent combatLogDMMessages;

            public UILogComponent uiLog;
        }
    }
}