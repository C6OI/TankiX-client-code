using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientUserProfile.API {
    [TemplatePart]
    public interface ClientSessionTemplatePart : Template, ClientSessionTemplate {
        ClientLocaleComponent clientLocale();
    }
}