using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class CameraVisibleTriggerComponent : MonoBehaviour, Component {
        public Camera MainCamera { get; set; }

        public bool IsVisible { get; set; }

        public float DistanceToCamera {
            get {
                if (MainCamera != null) {
                    return Vector3.Distance(MainCamera.transform.position, gameObject.transform.position);
                }

                return 0f;
            }
        }

        void OnBecameInvisible() => IsVisible = false;

        void OnBecameVisible() => IsVisible = true;

        public bool IsVisibleAtRange(float testRange) => IsVisible && DistanceToCamera < testRange;
    }
}