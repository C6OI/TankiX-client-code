using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientSoundZone : AmbientSoundFilter {
        [SerializeField] AmbientZoneSoundEffect ambientZoneSoundEffect;

        public void FadeIn() {
            ambientZoneSoundEffect.RegisterPlayingAmbientZone(this);
            Play();
        }

        public void FadeOut() => Stop();

        protected override void StopAndDestroy() {
            source.Stop();
            ResetFilter();
            ambientZoneSoundEffect.UnregisterPlayingAmbientZone(this);
            ambientZoneSoundEffect.FinalizeEffect();
        }
    }
}