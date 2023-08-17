using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftStartCooldownSoundEffectComponent : AbstractShaftSoundEffectComponent<AudioSource> {
        public override void Play() => soundComponent.Play();

        public override void Stop() => soundComponent.Stop();
    }
}