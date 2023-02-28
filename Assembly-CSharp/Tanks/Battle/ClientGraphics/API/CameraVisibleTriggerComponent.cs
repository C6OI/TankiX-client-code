using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class CameraVisibleTriggerComponent : BehaviourComponent {
        public Transform MainCameraTransform { get; set; }

        public bool IsVisible { get; set; }

        public float DistanceToCamera {
            get {
                if (MainCameraTransform != null) {
                    return Vector3.Distance(MainCameraTransform.position, gameObject.transform.position);
                }

                return 0f;
            }
        }

        void OnBecameInvisible() {
            IsVisible = false;
        }

        void OnBecameVisible() {
            IsVisible = true;
        }

        public bool IsVisibleAtRange(float testRange) => IsVisible && DistanceToCamera < testRange;
    }
}