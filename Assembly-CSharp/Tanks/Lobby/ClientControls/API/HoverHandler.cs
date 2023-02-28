using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class HoverHandler : MonoBehaviour {
        Camera camera;

        protected virtual bool pointerIn { get; set; }

        void Update() {
            bool flag = false;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] array = Physics.RaycastAll(ray, 100f);
            RaycastHit[] array2 = array;

            foreach (RaycastHit raycastHit in array2) {
                if (raycastHit.collider.gameObject == gameObject) {
                    flag = true;
                    break;
                }
            }

            if (flag && !pointerIn) {
                pointerIn = true;
            } else if (!flag && pointerIn) {
                pointerIn = false;
            }
        }

        void OnEnable() {
            camera = GetComponentInParent<Canvas>().worldCamera;
        }
    }
}