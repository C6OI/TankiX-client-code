using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1465192871085L)]
    public class ConfirmUserCountryEvent : Event {
        public string CountryCode { get; set; }
    }
}