using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1470735489716L)]
    public class UserCountryComponent : Component {
        public string CountryCode { get; set; }
    }
}