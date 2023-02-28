using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PelletThrowingGraphicsComponent : BehaviourComponent {
        [SerializeField] ParticleSystem trails;

        [SerializeField] ParticleSystem hits;

        [SerializeField] float sparklesMinLifetime = 0.25f;

        [SerializeField] float sparklesMaxLifetime = 0.5f;

        [SerializeField] float hitReflectVeolcity = 0.05f;

        public ParticleSystem Trails {
            get => trails;
            set => trails = value;
        }

        public ParticleSystem Hits {
            get => hits;
            set => hits = value;
        }

        public float SparklesMinLifetime {
            get => sparklesMinLifetime;
            set => sparklesMinLifetime = value;
        }

        public float SparklesMaxLifetime {
            get => sparklesMaxLifetime;
            set => sparklesMaxLifetime = value;
        }

        public float HitReflectVeolcity {
            get => hitReflectVeolcity;
            set => hitReflectVeolcity = value;
        }
    }
}