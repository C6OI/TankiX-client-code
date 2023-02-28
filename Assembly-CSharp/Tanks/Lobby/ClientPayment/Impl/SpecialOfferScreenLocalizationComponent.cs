using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class SpecialOfferScreenLocalizationComponent : Component {
        public string SpriteUid { get; set; }

        public string Footer { get; set; }

        public string Description { get; set; }
    }
}