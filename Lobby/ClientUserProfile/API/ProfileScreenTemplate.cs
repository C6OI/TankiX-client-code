using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [SerialVersionUID(1453294740327L)]
    public interface ProfileScreenTemplate : Template {
        [PersistentConfig]
        ProfileScreenLocalizationComponent profileScreenLocalization();
    }
}