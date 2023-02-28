using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientControls.API {
    [SerialVersionUID(635718872392823203L)]
    public interface LocalizedTextTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        LocalizedTextComponent localizedText();
    }
}