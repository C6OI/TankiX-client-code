using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SquadSystem : ECSSystem {
        [OnEventFire]
        public void SquadAdded(NodeAddedEvent e, SquadNode squad) {
            Debug.Log("SquadSystem.SquadAdded " + squad.Entity);
        }

        [OnEventFire]
        public void SquadRemoved(NodeRemoveEvent e, SquadNode squad) {
            Debug.Log("SquadSystem.SquadRemoved " + squad.Entity);
        }

        [OnEventFire]
        public void UserInSquadAdded(NodeAddedEvent e, [Combine] UserInSquadNode user, [JoinBySquad] SquadNode squad) {
            Debug.Log(string.Concat("SquadSystem.UserInSquadAdded ", user.Entity, " ", squad.Entity));
        }

        [OnEventFire]
        public void PayerInSquadAdded(NodeAddedEvent e, SquadPayerNode payer) {
            Debug.Log("SquadSystem.PayerInSquadAdded " + payer.Entity);
        }

        [OnEventFire]
        public void SquadLeaderAdded(NodeAddedEvent e, [Combine] SquadLeaderNode user, [JoinBySquad] SquadNode squad) {
            Debug.Log(string.Concat("SquadSystem.SquadLeaderAdded ", user.Entity, " ", squad.Entity));
        }

        [OnEventFire]
        public void SquadLeaderRemoved(NodeRemoveEvent e, SquadLeaderNode user) {
            Debug.Log("SquadSystem.SquadLeaderRemoved " + user.Entity);
        }

        public class SquadNode : Node {
            public SquadComponent squad;

            public SquadConfigComponent squadConfig;

            public SquadGroupComponent squadGroup;
        }

        public class UserInSquadNode : Node {
            public SquadGroupComponent squadGroup;
            public UserComponent user;
        }

        public class SquadPayerNode : UserInSquadNode {
            public BattleEntrancePayerComponent battleEntrancePayer;
        }

        public class SquadLeaderNode : UserInSquadNode {
            public SquadLeaderComponent squadLeader;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;
        }
    }
}