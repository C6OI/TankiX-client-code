using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    public class VegetationSettingsComponent : Component {
        public int Value { get; set; }

        public bool FarFoliageEnabled { get; set; }

        public bool BillboardTreesShadowCasting { get; set; }

        public bool BillboardTreesShadowReceiving { get; set; }
    }
}