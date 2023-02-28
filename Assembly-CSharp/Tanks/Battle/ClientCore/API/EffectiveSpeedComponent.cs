using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class EffectiveSpeedComponent : Component {
        public float MaxSpeed { get; set; }

        public float MaxTurnSpeed { get; set; }
    }
}