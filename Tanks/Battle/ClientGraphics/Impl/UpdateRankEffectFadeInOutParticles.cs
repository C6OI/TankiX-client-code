using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectFadeInOutParticles : MonoBehaviour {
        UpdateRankEffectSettings effectSettings;

        bool oldVisibleStat;

        ParticleSystem[] particles;

        void Start() {
            GetEffectSettingsComponent(transform);
            particles = effectSettings.GetComponentsInChildren<ParticleSystem>();
            oldVisibleStat = effectSettings.IsVisible;
        }

        void Update() {
            if (effectSettings.IsVisible != oldVisibleStat) {
                if (effectSettings.IsVisible) {
                    ParticleSystem[] array = particles;

                    foreach (ParticleSystem particleSystem in array) {
                        if (effectSettings.IsVisible) {
                            particleSystem.Play();
                            particleSystem.enableEmission = true;
                        }
                    }
                } else {
                    ParticleSystem[] array2 = particles;

                    foreach (ParticleSystem particleSystem2 in array2) {
                        if (!effectSettings.IsVisible) {
                            particleSystem2.Stop();
                            particleSystem2.enableEmission = false;
                        }
                    }
                }
            }

            oldVisibleStat = effectSettings.IsVisible;
        }

        void GetEffectSettingsComponent(Transform tr) {
            Transform parent = tr.parent;

            if (parent != null) {
                effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();

                if (effectSettings == null) {
                    GetEffectSettingsComponent(parent.transform);
                }
            }
        }
    }
}