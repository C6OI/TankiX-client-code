using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [TemplatePart]
    public interface ClientSessionTemplatePart : ClientSessionTemplate, Template {
        ClientLocaleComponent clientLocale();
    }
}