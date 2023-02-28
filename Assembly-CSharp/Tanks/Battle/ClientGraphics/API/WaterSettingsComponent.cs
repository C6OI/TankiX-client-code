using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class WaterSettingsComponent : Component {
        public bool HasReflection { get; set; }

        public bool EdgeBlend { get; set; }
    }
}