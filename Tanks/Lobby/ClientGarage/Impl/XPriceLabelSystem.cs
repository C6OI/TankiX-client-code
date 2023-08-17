using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class XPriceLabelSystem : AbstractPriceLabelSystem {
        [OnEventFire]
        public void UpdateUserMoney(UserXCrystalsChangedEvent e, UserNode userCrystal,
            [Combine] [JoinAll] SingleNode<PriceLabelComponent> price) => UpdatePriceForUser(price.component.Price,
            price.component,
            userCrystal.userXCrystals.Money);

        [OnEventFire]
        public void SetPriceForUser(SetPriceEvent e, PriceForUserNode priceForUser, [Mandatory] [JoinAll] UserNode user) =>
            UpdatePriceForUser(e.XPrice, priceForUser.xPriceLabel, user.userXCrystals.Money);

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserXCrystalsComponent userXCrystals;
        }

        public class PriceForUserNode : Node {
            public XPriceLabelComponent xPriceLabel;
        }
    }
}