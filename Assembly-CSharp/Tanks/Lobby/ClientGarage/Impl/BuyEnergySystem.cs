using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BuyEnergySystem : ECSSystem {
        [OnEventFire]
        public void OnPressEnergyContextBuyButton(PressEnergyContextBuyButtonEvent e, Node any, [JoinAll] UserNode selfUser, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            long money = selfUser.userXCrystals.Money;

            if (money < e.XPrice) {
                dialogs.component.Get<BuyConfirmationDialog>().XShow(null, delegate { }, (int)e.XPrice);
            } else {
                NewEvent(new EnergyContextBuyEvent(e.Count, e.XPrice)).Attach(selfUser).Schedule();
            }
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserXCrystalsComponent userXCrystals;
        }
    }
}