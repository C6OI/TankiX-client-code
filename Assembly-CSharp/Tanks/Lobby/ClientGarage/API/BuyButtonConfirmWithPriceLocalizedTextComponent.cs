using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class BuyButtonConfirmWithPriceLocalizedTextComponent : Component {
        public string BuyText { get; set; }

        public string ForText { get; set; }

        public string ConfirmText { get; set; }

        public string CancelText { get; set; }
    }
}