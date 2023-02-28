using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HealthFeedbackMapEffectMaterialComponent : BehaviourComponent {
        [SerializeField] Material sourceMaterial;

        [SerializeField] float intensitySpeed = 2f;

        public float IntensitySpeed => intensitySpeed;

        public Material SourceMaterial => sourceMaterial;
    }
}