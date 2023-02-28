using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl.Utility {
    public class MouseFollowBehaviour : MonoBehaviour {
        public RectTransform followObject;

        public Vector3 offset;

        public Camera uiCamera;

        public float smoothTime = 0.5f;

        Vector3 currentVelocity;

        float objZ;

        void Start() {
            objZ = followObject.position.z;
        }

        void Update() {
            if (uiCamera == null) {
                GameObject gameObject = GameObject.Find("UICamera");

                if (gameObject == null) {
                    return;
                }

                uiCamera = gameObject.GetComponent<Camera>();
            }

            Vector3 position = Input.mousePosition + offset;
            position.z = objZ;
            Vector3 target = uiCamera.ScreenToWorldPoint(position);
            followObject.position = Vector3.SmoothDamp(followObject.position, target, ref currentVelocity, smoothTime);
        }
    }
}