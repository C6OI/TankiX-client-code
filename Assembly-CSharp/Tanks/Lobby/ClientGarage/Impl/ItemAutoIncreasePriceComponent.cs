using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemAutoIncreasePriceComponent : Component {
        public int StartCount { get; set; }

        public int PriceIncreaseAmount { get; set; }

        public int MaxAdditionalPrice { get; set; }
    }
}