using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [SerialVersionUID(1453796862447L)]
    [Shared]
    public class ClientLocaleComponent : Component {
        public ClientLocaleComponent() { }

        public ClientLocaleComponent(string localeCode) => LocaleCode = localeCode;

        public string LocaleCode { get; set; }
    }
}