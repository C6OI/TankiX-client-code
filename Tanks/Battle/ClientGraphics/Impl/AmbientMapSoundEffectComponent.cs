using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientMapSoundEffectComponent : Component {
        public AmbientMapSoundEffectComponent() { }

        public AmbientMapSoundEffectComponent(AmbientSoundFilter ambientMapSound) => AmbientMapSound = ambientMapSound;

        public AmbientSoundFilter AmbientMapSound { get; set; }
    }
}