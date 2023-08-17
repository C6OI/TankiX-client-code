using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class MaterialsSettingsComponent : Component {
        public int GlobalShadersMaximumLOD { get; set; }
    }
}