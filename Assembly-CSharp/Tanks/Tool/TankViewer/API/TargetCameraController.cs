using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class TargetCameraController : MonoBehaviour {
        public Transform target;

        public float distance = 10f;

        public float xSpeed = 250f;

        public float ySpeed = 120f;

        public float yMinLimit = -20f;

        public float yMaxLimit = 80f;

        public float moveSpeed = 1f;

        public float autoRotateSpeed = 1f;

        public Transform defaultCameraTransform;

        int rotationMode;

        float x;

        float y;

        public bool AutoRotate { get; set; }

        void Start() {
            SetDefaultTransform();
        }

        void LateUpdate() {
            if (target == null) {
                return;
            }

            if (AutoRotate) {
                transform.RotateAround(target.position, Vector3.up, Time.deltaTime * autoRotateSpeed);
                Vector3 eulerAngles = transform.localRotation.eulerAngles;
                x = eulerAngles.y;
                y = eulerAngles.x;
                return;
            }

            distance -= Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            Quaternion quaternion = Quaternion.Euler(y, x, 0f);
            transform.localRotation = quaternion;
            Vector3 localPosition = target.position + quaternion * new Vector3(0f, 0f, 0f - distance);
            transform.localPosition = localPosition;
        }

        public void SetDefaultTransform() {
            if (defaultCameraTransform != null) {
                transform.rotation = defaultCameraTransform.rotation;
                transform.position = defaultCameraTransform.position;
                distance = Vector3.Distance(transform.position, target.position);
            }

            Vector3 eulerAngles = transform.localRotation.eulerAngles;
            x = eulerAngles.y;
            y = eulerAngles.x;
        }

        static float ClampAngle(float angle, float min, float max) {
            if (angle < -360f) {
                angle += 360f;
            }

            if (angle > 360f) {
                angle -= 360f;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}