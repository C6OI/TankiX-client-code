using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ParticleSystemEffectsComponent : BehaviourComponent {
        [SerializeField] List<ParticleSystem> particleSystems;

        public void StartEmission() {
            foreach (ParticleSystem particleSystem in particleSystems) {
                particleSystem.Play();
            }
        }

        public void StopEmission() {
            foreach (ParticleSystem particleSystem in particleSystems) {
                particleSystem.Stop();
            }
        }
    }
}