using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PlayingMelodyRoundRestartListenerComponent : Component {
        public PlayingMelodyRoundRestartListenerComponent(AmbientSoundFilter melody) => Melody = melody;

        public AmbientSoundFilter Melody { get; set; }
    }
}