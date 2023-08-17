using UnityEngine;

namespace Lobby.ClientControls.API {
    public class RotationDisabled : MonoBehaviour {
        public void LateUpdate() => transform.rotation = Quaternion.identity;
    }
}