using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1453796862447L)]
    public class ClientLocaleComponent : Component {
        public ClientLocaleComponent() { }

        public ClientLocaleComponent(string localeCode) => LocaleCode = localeCode;

        public string LocaleCode { get; set; }
    }
}