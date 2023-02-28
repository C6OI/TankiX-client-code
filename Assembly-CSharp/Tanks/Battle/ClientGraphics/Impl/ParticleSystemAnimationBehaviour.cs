using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ParticleSystemAnimationBehaviour : MonoBehaviour {
        public ParticleSystem particleSystem;

        public void PlayParticleSystem() {
            particleSystem.Play();
        }
    }
}