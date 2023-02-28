using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BattleLabelLoadSystem : ECSSystem {
        [OnEventFire]
        public void OnEnabledBattleLabel(NodeAddedEvent e, BattleLabelReadyNode battleLabel, [JoinAll] SelfUserNode selfUser) {
            long battleId = battleLabel.battleLabel.BattleId;
            ScheduleEvent(new RequestLoadBattleInfoEvent(battleId), selfUser);
        }

        [OnEventFire]
        public void OnEnabledUserLabelState(NodeAddedEvent e, UserLabelStateNode userLabel, [JoinAll] SelfUserNode selfUser) {
            long key = userLabel.battleGroup.Key;
            ScheduleEvent(new RequestLoadBattleInfoEvent(key), selfUser);
        }

        [OnEventFire]
        public void LoadedUser(BattleInfoForLabelLoadedEvent e, SelfUserNode selfUser, [JoinAll] ICollection<EmptyBattleLabelNode> battleLabels) {
            foreach (EmptyBattleLabelNode battleLabel in battleLabels) {
                if (battleLabel.battleLabel.BattleId.Equals(e.BattleId)) {
                    BattleInfoForLabelComponent battleInfoForLabelComponent = new();
                    battleInfoForLabelComponent.BattleMode = e.BattleMode;
                    battleLabel.Entity.AddComponent(battleInfoForLabelComponent);
                    MapGroupComponent component = e.Map.GetComponent<MapGroupComponent>();
                    component.Attach(battleLabel.Entity);
                }
            }
        }

        [OnEventFire]
        public void LoadedUser(BattleInfoForLabelLoadedEvent e, SelfUserNode selfUser, [JoinAll] ICollection<UserLabelStateNode> userLabels) {
            foreach (UserLabelStateNode userLabel in userLabels) {
                if (userLabel.battleGroup.Key.Equals(e.BattleId)) {
                    string battleMode = e.BattleMode;
                    string name = e.Map.GetComponent<DescriptionItemComponent>().Name;
                    userLabel.userLabelState.SetBattleDescription(battleMode, name);
                }
            }
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }

        public class BattleLabelReadyNode : Node {
            public BattleLabelComponent battleLabel;

            public BattleLabelReadyComponent battleLabelReady;
        }

        [Not(typeof(BattleInfoForLabelComponent))]
        public class EmptyBattleLabelNode : BattleLabelReadyNode { }

        public class UserLabelStateNode : Node {
            public BattleGroupComponent battleGroup;

            public UserLabelStateComponent userLabelState;
        }
    }
}