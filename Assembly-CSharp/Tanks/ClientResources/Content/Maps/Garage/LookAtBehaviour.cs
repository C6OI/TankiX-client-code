using UnityEngine;

namespace tanks.ClientResources.Content.Maps.Garage {
    public class LookAtBehaviour : MonoBehaviour {
        [Header("By default - MainCamera")] public Transform Target;

        public bool OnlyY;

        void Awake() {
            if (Target == null) {
                Target = Camera.main.transform;
            }
        }

        void Update() {
            transform.LookAt(Target);

            if (OnlyY) {
                Vector3 eulerAngles = transform.rotation.eulerAngles;
                eulerAngles.x = 0f;
                eulerAngles.z = 0f;
                transform.rotation = Quaternion.Euler(eulerAngles);
            }
        }
    }
}