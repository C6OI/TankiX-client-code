using Lobby.ClientPayment.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace tanks.modules.lobby.ClientPayment.API {
    [SerialVersionUID(1513675638265L)]
    public interface PremiumOfferTemplate : SpecialOfferBaseTemplate, GoodsTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        PremiumOfferComponent premiumOffer();
    }
}