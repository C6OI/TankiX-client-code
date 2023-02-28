using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(1451465612461L)]
    public interface BattleChatHUDTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        BattleChatLocalizedStringsComponent battleChatLocalizedStrings();
    }
}