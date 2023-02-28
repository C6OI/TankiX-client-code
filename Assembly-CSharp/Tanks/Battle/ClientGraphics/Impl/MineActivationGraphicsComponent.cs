using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineActivationGraphicsComponent : Component {
        public MineActivationGraphicsComponent() { }

        public MineActivationGraphicsComponent(float activationStartTime) => ActivationStartTime = activationStartTime;

        public float ActivationStartTime { get; set; }
    }
}