using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoleTracerGraphicsComponent : MonoBehaviour, Component {
        [SerializeField] ParticleSystem tracer;

        [SerializeField] float maxTime = 0.2f;

        public ParticleSystem Tracer {
            get => tracer;
            set => tracer = value;
        }

        public float MaxTime {
            get => maxTime;
            set => maxTime = value;
        }
    }
}