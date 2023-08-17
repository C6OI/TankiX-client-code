using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1447140196293L)]
    public interface GarageItemPropertyTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        ItemPropertyScreenTextComponent itemPropertyScreenText();

        [AutoAdded]
        [PersistentConfig]
        ItemUpgradeScreenTextComponent itemUpgradeScreenText();

        [PersistentConfig]
        WarningOverUpgradeComponent warningOverUpgrade();
    }
}