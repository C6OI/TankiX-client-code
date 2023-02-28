using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class WeaponStreamTracerEffectComponent : BehaviourComponent {
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