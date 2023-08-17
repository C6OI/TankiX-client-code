using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class VegetationSettingsComponent : Component {
        public float GrassNearDrawDistance { get; set; }

        public float GrassFarDrawDistance { get; set; }

        public float GrassFadeRange { get; set; }

        public float GrassDenstyMultipler { get; set; }

        public bool GrassCastsShadow { get; set; }

        public bool FarFoliageEnabled { get; set; }

        public bool BillboardTreesShadowCasting { get; set; }

        public bool BillboardTreesShadowReceiving { get; set; }
    }
}