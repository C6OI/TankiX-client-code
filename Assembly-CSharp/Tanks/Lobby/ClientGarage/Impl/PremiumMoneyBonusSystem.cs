using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumMoneyBonusSystem : ECSSystem {
        [OnEventFire]
        public void RegisterBonus(NodeAddedEvent e, MoneyBonusNode bonus) { }

        public class MoneyBonusNode : Node {
            public MoneyBonusComponent moneyBonus;
            public UserGroupComponent userGroup;
        }
    }
}