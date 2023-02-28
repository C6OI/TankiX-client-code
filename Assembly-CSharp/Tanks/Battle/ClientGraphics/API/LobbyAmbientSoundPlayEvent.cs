using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class LobbyAmbientSoundPlayEvent : Event {
        public LobbyAmbientSoundPlayEvent(bool hymnMode) => HymnMode = hymnMode;

        public bool HymnMode { get; set; }
    }
}