using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientLoading.API {
    [SerialVersionUID(5421564863136L)]
    public interface BattleLoadScreenTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        LoadScreenLocalizedTextComponent loadScreenLocalizedText();
    }
}