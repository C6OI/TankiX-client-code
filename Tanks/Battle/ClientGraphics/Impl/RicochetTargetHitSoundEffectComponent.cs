using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RicochetTargetHitSoundEffectComponent : BaseRicochetSoundEffectComponent {
        public override void Play(AudioSource sourceInstane) => sourceInstane.Play();
    }
}