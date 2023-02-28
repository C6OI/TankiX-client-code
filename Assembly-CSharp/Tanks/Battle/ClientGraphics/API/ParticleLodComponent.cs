using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class ParticleLodComponent : MonoBehaviour {
        public float[] coefficient;

        void Start() {
            int currentParticleQuality = GraphicsSettings.INSTANCE.CurrentParticleQuality;
            float num = coefficient[currentParticleQuality];
            ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
            ParticleSystem[] array = componentsInChildren;

            foreach (ParticleSystem particleSystem in array) {
                ParticleSystem.MainModule main = particleSystem.main;
                main.maxParticles = Mathf.Max(Mathf.Min(main.maxParticles, 1), (int)(main.maxParticles * num));
                ParticleSystem.EmissionModule emission = particleSystem.emission;
                emission.rateOverTimeMultiplier = Mathf.Max(Mathf.Min(emission.rateOverTimeMultiplier, 1f), (int)(emission.rateOverTimeMultiplier * num));
                emission.rateOverDistanceMultiplier = Mathf.Max(Mathf.Min(emission.rateOverDistanceMultiplier, 1f), (int)(emission.rateOverDistanceMultiplier * num));
                ParticleSystem.Burst[] array2 = new ParticleSystem.Burst[emission.burstCount];
                emission.GetBursts(array2);

                for (int j = 0; j < emission.burstCount; j++) {
                    array2[j].minCount = (short)Mathf.Max(Mathf.Min(array2[j].minCount, 1), array2[j].minCount * num);
                    array2[j].maxCount = (short)Mathf.Max(Mathf.Min(array2[j].maxCount, 1), array2[j].maxCount * num);
                }

                emission.SetBursts(array2);
            }
        }
    }
}