using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class VisualScoreSystem : ECSSystem {
        [OnEventFire]
        public void ClearLog(NodeRemoveEvent e, KillAssistElementNode killAssistNode) {
            killAssistNode.killAssist.Clear();
        }

        [OnEventFire]
        public void ShowHealMessage(VisualScoreHealEvent e, BattleUserNode battleUser, [JoinAll] KillAssistElementNode killAssistNode) {
            killAssistNode.killAssist.AddHealMessage(e.Score);
        }

        [OnEventFire]
        public void ShowKilledMessage(VisualScoreKillEvent e, BattleUserNode battleUser, [JoinAll] KillAssistElementNode killAssistNode) {
            killAssistNode.killAssist.AddKillMessage(e.Score, e.TargetUid, e.TargetRank);
        }

        [OnEventFire]
        public void ShowAssistMessage(VisualScoreAssistEvent e, BattleUserNode battleUser, [JoinAll] KillAssistElementNode killAssistNode) {
            killAssistNode.killAssist.AddAssistMessage(e.Score, e.Percent, e.TargetUid);
        }

        [OnEventFire]
        public void ShowFlagDeliverMessage(VisualScoreFlagDeliverEvent e, BattleUserNode battleUser, [JoinAll] KillAssistElementNode killAssistNode) {
            killAssistNode.killAssist.AddFlagDeliveryMessage(e.Score);
        }

        [OnEventFire]
        public void ShowFlagDeliverMessage(VisualScoreFlagReturnEvent e, BattleUserNode battleUser, [JoinAll] KillAssistElementNode killAssistNode) {
            killAssistNode.killAssist.AddFlagReturnMessage(e.Score);
        }

        [OnEventFire]
        public void ShowStreakMessage(VisualScoreStreakEvent e, BattleUserNode battleUser, [JoinAll] KillAssistElementNode killAssistNode) {
            killAssistNode.killAssist.AddKillStreakMessage(e.Score);
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;

            public UserUidComponent userUid;
        }

        public class TankNode : Node {
            public TankComponent tank;
            public UserGroupComponent userGroup;
        }

        public class BattleUserNode : Node {
            public BattleUserComponent battleUser;

            public SelfBattleUserComponent selfBattleUser;
        }

        public class KillAssistElementNode : Node {
            public KillAssistComponent killAssist;
        }

        public class SelfUserNode : UserNode {
            public SelfUserComponent selfUser;
        }
    }
}