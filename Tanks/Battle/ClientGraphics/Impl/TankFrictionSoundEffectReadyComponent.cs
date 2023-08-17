using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFrictionSoundEffectReadyComponent : Component {
        public TankFrictionSoundBehaviour TankFrictionSoundBehaviour { get; set; }

        public TankFrictionCollideSoundBehaviour TankFrictionCollideSoundBehaviour { get; set; }
    }
}