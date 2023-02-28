using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class FreeCameraController : MonoBehaviour {
        public float xSpeed = 250f;

        public float ySpeed = 120f;

        public float yMinLimit = -20f;

        public float yMaxLimit = 80f;

        public float moveSpeed = 1f;

        float x;

        float y;

        void Start() {
            Init();
        }

        void LateUpdate() {
            if (GUIUtility.hotControl != 0 || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                return;
            }

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            Quaternion localRotation = Quaternion.Euler(y, x, 0f);
            transform.localRotation = localRotation;

            if (Input.GetMouseButton(1)) {
                Vector3 translation = default;
                translation.x = moveSpeed * Input.GetAxis("Horizontal");
                translation.y = moveSpeed * Input.GetAxis("Deep");
                translation.z = moveSpeed * Input.GetAxis("Vertical");

                if (translation.x != 0.0 || translation.y != 0.0 || translation.z != 0.0) {
                    transform.Translate(translation);
                }
            }
        }

        void OnEnable() {
            Init();
        }

        void Init() {
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