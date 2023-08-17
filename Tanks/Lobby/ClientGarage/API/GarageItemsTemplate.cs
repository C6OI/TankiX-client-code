using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(635847310293843430L)]
    public interface GarageItemsTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        GarageItemsScreenTextComponent garageItemsScreenText();
    }
}