namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftStartAimingSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController> {
        public float StartAimingDurationSec => soundComponent.Source.clip.length;

        public override void Play() => soundComponent.SetSoundActive();

        public override void Stop() => soundComponent.FadeOut();
    }
}