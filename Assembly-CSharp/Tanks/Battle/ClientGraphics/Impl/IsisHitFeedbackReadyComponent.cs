using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IsisHitFeedbackReadyComponent : Component {
        public IsisHitFeedbackReadyComponent(SoundController healingSoundController, SoundController attackSoundController) {
            HealingSoundController = healingSoundController;
            AttackSoundController = attackSoundController;
        }

        public SoundController HealingSoundController { get; set; }

        public SoundController AttackSoundController { get; set; }
    }
}