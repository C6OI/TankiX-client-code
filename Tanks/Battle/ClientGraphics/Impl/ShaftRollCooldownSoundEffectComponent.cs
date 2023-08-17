namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftRollCooldownSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController> {
        public override void Play() {
            soundComponent.SetSoundActive();
            soundComponent.FadeOut();
        }

        public override void Stop() => soundComponent.StopImmediately();

        public void SetFadeOutTime(float fadeOutTimeSec) => soundComponent.FadeOutTimeSec = fadeOutTimeSec;
    }
}