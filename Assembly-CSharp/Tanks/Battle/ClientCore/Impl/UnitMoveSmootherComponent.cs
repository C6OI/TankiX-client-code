using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class UnitMoveSmootherComponent : MonoBehaviour, Component {
        [SerializeField] float smoothingSpeed = 5f;

        Rigidbody body;

        Vector3 lastPosition;

        Quaternion lastRotation;

        Vector3 smoothPositionDelta;

        Quaternion smoothRotationDelta;

        void Start() {
            body = GetComponentInParent<Rigidbody>();
        }

        void LateUpdate() {
            if ((bool)body) {
                smoothPositionDelta = Vector3.Lerp(smoothPositionDelta, Vector3.zero, smoothingSpeed * Time.deltaTime);
                smoothRotationDelta = Quaternion.Slerp(smoothRotationDelta, Quaternion.identity, smoothingSpeed * Time.smoothDeltaTime);
                transform.SetLocalPositionSafe(smoothPositionDelta);
                transform.SetLocalRotationSafe(smoothRotationDelta);
                body.centerOfMass = smoothPositionDelta;
            }
        }

        public void BeforeSetMovement() {
            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }

        public void AfterSetMovement() {
            transform.position = lastPosition;
            transform.rotation = lastRotation;
            smoothPositionDelta = transform.localPosition;
            smoothRotationDelta = transform.localRotation;
            LateUpdate();
        }
    }
}