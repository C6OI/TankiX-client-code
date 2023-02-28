using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftQuickShotSoundEffectComponent : AbstractShaftSoundEffectComponent<AudioSource> {
        public override void Play() {
            soundComponent.Play();
        }

        public override void Stop() {
            soundComponent.Stop();
        }
    }
}