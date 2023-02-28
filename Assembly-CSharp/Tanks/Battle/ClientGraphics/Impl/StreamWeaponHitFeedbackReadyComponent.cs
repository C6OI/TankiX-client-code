using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StreamWeaponHitFeedbackReadyComponent : Component {
        public StreamWeaponHitFeedbackReadyComponent(SoundController soundController) => SoundController = soundController;

        public SoundController SoundController { get; set; }
    }
}