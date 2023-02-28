using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class HitFeedbackSoundsPlayingSettingsComponent : Component {
        public float Delay { get; set; }

        public bool RemoveOnKillSound { get; set; }

        public float Volume { get; set; }
    }
}