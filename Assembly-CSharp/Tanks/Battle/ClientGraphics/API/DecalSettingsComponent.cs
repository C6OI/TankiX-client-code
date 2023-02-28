using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class DecalSettingsComponent : Component {
        public float MaxDistanceToCamera { get; set; }

        public int MaxCount { get; set; }

        public float LifeTimeMultipler { get; set; }

        public bool EnableDecals { get; set; }

        public int MaxDecalsForHammer { get; set; }
    }
}