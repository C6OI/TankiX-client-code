using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectParticleMovement : MonoBehaviour {
        public Transform parent;

        readonly ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

        Vector3 delta;

        ParticleSystem particleSystem;

        Vector3 previousPosition;

        void Start() {
            particleSystem = GetComponent<ParticleSystem>();
            previousPosition = parent.position;
        }

        void Update() { }

        void LateUpdate() {
            int num = particleSystem.GetParticles(particles);

            for (int i = 0; i < num; i++) {
                particles[i].position = particles[i].position + delta;
            }

            particleSystem.SetParticles(particles, num);
            delta = parent.position - previousPosition;
            previousPosition = parent.position;
        }
    }
}