using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1456381758988L)]
    public interface EmailConfigTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        EmailConfirmationCodeConfigComponent emailConfirmationCodeConfig();
    }
}