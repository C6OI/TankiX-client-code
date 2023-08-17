using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [SerialVersionUID(1470735489716L)]
    [Shared]
    public class UserCountryComponent : Component {
        public string CountryCode { get; set; }
    }
}