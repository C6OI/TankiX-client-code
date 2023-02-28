using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class SpecialOfferContentLocalizationComponent : Component {
        public string SpriteUid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}