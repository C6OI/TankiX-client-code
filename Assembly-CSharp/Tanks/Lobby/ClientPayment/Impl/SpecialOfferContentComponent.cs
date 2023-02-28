using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class SpecialOfferContentComponent : Component {
        public int SalePercent { get; set; }

        public bool HighlightTitle { get; set; }

        public bool ShowItemsList { get; set; }
    }
}