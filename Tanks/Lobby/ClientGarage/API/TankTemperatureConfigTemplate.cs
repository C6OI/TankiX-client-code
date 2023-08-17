using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1438840222545L)]
    public interface TankTemperatureConfigTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        TemperatureConfigComponent temperatureConfig();
    }
}