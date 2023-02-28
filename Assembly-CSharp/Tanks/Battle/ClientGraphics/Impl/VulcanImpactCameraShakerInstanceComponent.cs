using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanImpactCameraShakerInstanceComponent : Component {
        public VulcanImpactCameraShakerInstanceComponent(float fadeOutTime) {
            Init(fadeOutTime);
        }

        public CameraShakeInstance CameraShakeInstance { get; set; }

        public float FadeOutTime { get; set; }

        public Vector3 ImpactDirection { get; set; }

        public float WeakeningCoeff { get; set; }

        public bool ImpactDataChanged { get; set; }

        public void Init(float fadeOutTime) {
            FadeOutTime = fadeOutTime;
            ImpactDataChanged = false;
            ImpactDirection = Vector3.zero;
            WeakeningCoeff = -1f;
        }
    }
}