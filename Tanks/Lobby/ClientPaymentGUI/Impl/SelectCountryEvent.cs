using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class SelectCountryEvent : Event {
        public string CountryName { get; set; }

        public string CountryCode { get; set; }
    }
}