namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingLoopSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController> {
        public override void Play() => soundComponent.SetSoundActive();

        public override void Stop() => soundComponent.FadeOut();

        public void SetDelay(float delayTimeSec) => soundComponent.PlayingDelaySec = delayTimeSec;
    }
}