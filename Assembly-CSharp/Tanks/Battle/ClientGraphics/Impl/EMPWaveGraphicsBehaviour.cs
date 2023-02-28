using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(ParticleSystem))]
    public class EMPWaveGraphicsBehaviour : MonoBehaviour {
        [SerializeField] ParticleSystem waveParticleSystem;

        [SerializeField] AudioSource waveSound;

        public ParticleSystem WaveParticleSystem => waveParticleSystem;

        public AudioSource WaveSound => waveSound;
    }
}