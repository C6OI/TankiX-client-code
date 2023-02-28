using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(636116988769884989L)]
    public interface UserNotificatorHUDTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        UserNotificatorHUDTextComponent userNotificatorHUDText();
    }
}