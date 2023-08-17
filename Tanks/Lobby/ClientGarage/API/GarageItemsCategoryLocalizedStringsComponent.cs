using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class GarageItemsCategoryLocalizedStringsComponent : Component {
        public string WeaponsButtonLabel { get; set; }

        public string HullsButtonLabel { get; set; }

        public string ColorsButtonLabel { get; set; }

        public string SuppliesButtonLabel { get; set; }

        public string GraffitiButtonLabel { get; set; }
    }
}