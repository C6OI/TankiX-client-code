using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapNativeSoundsComponent : Component {
        public MapNativeSoundsComponent(MapNativeSoundsBehaviour mapNativeSounds) => MapNativeSounds = mapNativeSounds;

        public MapNativeSoundsBehaviour MapNativeSounds { get; set; }
    }
}