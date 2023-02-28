using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftClosingCooldownSoundEffectComponent : AbstractShaftSoundEffectComponent<AudioSource> {
        public override void Play() {
            soundComponent.Play();
        }

        public override void Stop() {
            soundComponent.Stop();
        }
    }
}