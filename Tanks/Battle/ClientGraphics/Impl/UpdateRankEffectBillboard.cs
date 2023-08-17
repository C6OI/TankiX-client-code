using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectBillboard : MonoBehaviour {
        [SerializeField] Camera camera;

        [SerializeField] bool active;

        [SerializeField] bool autoInitCamera = true;

        Transform cameraTransform;

        Transform containerTransform;

        GameObject myContainer;

        void Awake() {
            if (autoInitCamera) {
                camera = Camera.main;
                active = true;
            }

            Transform parent = transform.parent;
            cameraTransform = camera.transform;

            myContainer = new GameObject {
                name = "Billboard_" + gameObject.name
            };

            containerTransform = myContainer.transform;
            containerTransform.position = transform.position;
            transform.parent = myContainer.transform;
            containerTransform.parent = parent;
        }

        void Update() {
            if (active) {
                containerTransform.LookAt(containerTransform.position + cameraTransform.rotation * Vector3.back,
                    cameraTransform.rotation * Vector3.up);
            }
        }
    }
}