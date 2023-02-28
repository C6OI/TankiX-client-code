using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636350381077302417L)]
    public interface ModuleUpgradeEffectWithDurationTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleEffectDurationPropertyComponent moduleEffectDurationProperty();
    }
}