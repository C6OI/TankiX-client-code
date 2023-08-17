using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanTracerEffectComponent : MonoBehaviour, Component {
        [SerializeField] float tracerMaxLength = 222.22f;

        [SerializeField] float startTracerOffset = 0.5f;

        [SerializeField] GameObject tracer;

        public float StartTracerOffset {
            get => startTracerOffset;
            set => startTracerOffset = value;
        }

        public float TracerMaxLength {
            get => tracerMaxLength;
            set => tracerMaxLength = value;
        }

        public GameObject Tracer {
            get => tracer;
            set => tracer = value;
        }
    }
}