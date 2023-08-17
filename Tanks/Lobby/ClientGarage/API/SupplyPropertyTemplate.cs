using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1439889950545L)]
    public interface SupplyPropertyTemplate : Template {
        [AutoAdded]
        SupplyPropertyComponent supplyProperty();

        SupplyGroupComponent supplyGroup();
    }
}