using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(6886320463503786587L)]
    public interface RepairEffectTemplate : Template, EffectTemplate {
        [PersistentConfig]
        RepairEffectConfigComponent repairEffect();

        [PersistentConfig]
        DurationConfigComponent duration();
    }
}