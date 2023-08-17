using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(1442306448169L)]
    public interface PauseServiceMessageTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        PauseMessageComponent pauseMessage();
    }
}