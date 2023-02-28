using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636316679633422227L)]
    public interface SlotConfigTemplate : Template {
        [AutoAdded]
        [PersistentConfig("slotsTypes")]
        Slots2ModuleBehaviourTypesConfigComponent slots2ModuleBehaviourTypesConfig();
    }
}