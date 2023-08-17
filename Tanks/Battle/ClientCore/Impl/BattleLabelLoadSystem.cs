using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BattleLabelLoadSystem : ECSSystem {
        [OnEventFire]
        public void OnEnabledBattleLabel(NodeAddedEvent e, BattleLabelReadyNode battleLabel,
            [JoinAll] SelfUserNode selfUser) {
            long battleId = battleLabel.battleLabel.BattleId;
            ScheduleEvent(new RequestLoadBattleInfoEvent(battleId), selfUser);
        }

        [OnEventFire]
        public void LoadedUser(BattleInfoForLabelLoadedEvent e, SelfUserNode selfUser,
            [JoinAll] ICollection<EmptyBattleLabelNode> battleLabels) {
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

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }

        public class BattleLabelReadyNode : Node {
            public BattleLabelComponent battleLabel;

            public BattleLabelReadyComponent battleLabelReady;
        }

        [Not(typeof(BattleInfoForLabelComponent))]
        public class EmptyBattleLabelNode : BattleLabelReadyNode { }
    }
}