using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1439558324520L)]
    public interface RepairPropertyTemplate : Template, SupplyPropertyTemplate {
        [PersistentConfig("repairEffect")]
        [AutoAdded]
        RepairEffectConfigComponent repairEffectConfig();

        [AutoAdded]
        [PersistentConfig("duration")]
        DurationConfigComponent durationConfig();
    }
}