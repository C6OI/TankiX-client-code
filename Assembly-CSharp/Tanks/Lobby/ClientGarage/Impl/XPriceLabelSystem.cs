using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class XPriceLabelSystem : AbstractPriceLabelSystem {
        [OnEventFire]
        public void UpdateUserMoney(UserXCrystalsChangedEvent e, UserNode userCrystal, [JoinAll] [Combine] SingleNode<PriceLabelComponent> price) {
            UpdatePriceForUser(price.component.Price, price.component.OldPrice, price.component, userCrystal.userXCrystals.Money);
        }

        [OnEventFire]
        public void SetPriceForUser(SetPriceEvent e, PriceForUserNode priceForUser, [JoinAll] [Mandatory] UserNode user) {
            UpdatePriceForUser(e.XPrice, e.OldXPrice, priceForUser.xPriceLabel, user.userXCrystals.Money);
        }

        [OnEventFire]
        public void PriceChanged(PriceChangedEvent e, PriceForUserNode priceForUser, [JoinAll] [Mandatory] UserNode user) {
            UpdatePriceForUser(e.XPrice, e.OldXPrice, priceForUser.xPriceLabel, user.userXCrystals.Money);
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserXCrystalsComponent userXCrystals;
        }

        public class PriceForUserNode : Node {
            public XPriceLabelComponent xPriceLabel;
        }
    }
}