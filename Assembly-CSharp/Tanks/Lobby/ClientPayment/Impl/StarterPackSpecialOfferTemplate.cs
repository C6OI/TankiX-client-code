using Lobby.ClientPayment.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(636166274132352400L)]
    public interface StarterPackSpecialOfferTemplate : BaseStarterPackSpecialOfferTemplate, SpecialOfferBaseTemplate, GoodsTemplate, Template {
        [PersistentConfig]
        [AutoAdded]
        StarterPackMainElementComponent starterPackMainElement();

        [AutoAdded]
        [PersistentConfig]
        StarterPackVisualConfigComponent starterPackVisualConfig();
    }
}