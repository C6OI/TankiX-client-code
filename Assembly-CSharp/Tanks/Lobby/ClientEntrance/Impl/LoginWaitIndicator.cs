using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class LoginWaitIndicator : MonoBehaviour {
        public float angle = 1f;

        void Update() {
            transform.Rotate(Vector3.back, angle);
        }
    }
}