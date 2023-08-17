using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SpeedSoundEffectComponent : BaseEffectSoundComponent<AudioSource> {
        public override void BeginEffect() {
            StopSound.Stop();
            StartSound.Play();
        }

        public override void StopEffect() {
            StartSound.Stop();
            StopSound.Play();
        }
    }
}