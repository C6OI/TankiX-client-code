using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientZoneSoundEffectComponent : Component {
        public AmbientZoneSoundEffectComponent(AmbientZoneSoundEffect ambientZoneSoundEffect) => AmbientZoneSoundEffect = ambientZoneSoundEffect;

        public AmbientZoneSoundEffect AmbientZoneSoundEffect { get; set; }
    }
}