using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class EnergyBonusUISystem : ECSSystem {
        [OnEventFire]
        public void SetBonusInactive(NodeAddedEvent e, TakenEnergyBonusNode userItem, [JoinAll] SingleNode<EnergyBonusContent> energyBonusContent) {
            energyBonusContent.component.SetBonusInactive();
        }

        [OnEventFire]
        public void SetBonusActive(NodeRemoveEvent e, SingleNode<TakenBonusComponent> userItem, [JoinAll] SingleNode<EnergyBonusContent> energyBonusContent) {
            energyBonusContent.component.SetBonusActive();
        }

        public class TakenEnergyBonusNode : Node {
            public EnergyBonusComponent energyBonus;

            public ExpireDateComponent expireDate;

            public TakenBonusComponent takenBonus;
            public UserGroupComponent userGroup;
        }
    }
}