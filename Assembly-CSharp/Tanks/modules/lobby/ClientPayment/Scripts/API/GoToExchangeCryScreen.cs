using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace tanks.modules.lobby.ClientPayment.Scripts.API {
    public class GoToExchangeCryScreen : Event {
        public GoToExchangeCryScreen() => ExchangingCrystalls = 1000L;

        public GoToExchangeCryScreen(long exchangingCrystalls) => ExchangingCrystalls = exchangingCrystalls;

        public long ExchangingCrystalls { get; set; }
    }
}