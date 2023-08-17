using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PriceLabelSystem : AbstractPriceLabelSystem {
        [OnEventFire]
        public void UpdateUserMoney(UserMoneyChangedEvent e, UserNode userCrystal,
            [JoinAll] [Combine] SingleNode<PriceLabelComponent> price) =>
            UpdatePriceForUser(price.component.Price, price.component, userCrystal.userMoney.Money);

        [OnEventFire]
        public void SetPriceForUser(SetPriceEvent e, PriceForUserNode priceForUser, [Mandatory] [JoinAll] UserNode user) =>
            UpdatePriceForUser(e.Price, priceForUser.priceLabel, user.userMoney.Money);

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserMoneyComponent userMoney;
        }

        public class PriceForUserNode : Node {
            public PriceLabelComponent priceLabel;
        }
    }
}