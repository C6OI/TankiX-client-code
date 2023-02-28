using Lobby.ClientPayment.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientPayment.API {
    [SerialVersionUID(1513600712014L)]
    public interface LeagueFirstEntranceSpecialOfferTemplate : SpecialOfferBaseTemplate, GoodsTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        LeagueFirstEntranceSpecialOfferComponent leagueFirstEntranceSpecialOffer();
    }
}