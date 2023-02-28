using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1484723551013L)]
    public interface GarageModulesScreenTemplate : Template {
        GarageModulesScreenComponent garageModulesScreen();

        [AutoAdded]
        [PersistentConfig]
        ModulesScreenTextComponent modulesScreenText();
    }
}