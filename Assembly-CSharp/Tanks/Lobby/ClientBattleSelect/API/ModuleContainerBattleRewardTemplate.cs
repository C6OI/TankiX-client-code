using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1513922916493L)]
    public interface ModuleContainerBattleRewardTemplate : BattleResultRewardTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleContainerRewardTextConfigComponent moduleContainerRewardTextConfig();
    }
}