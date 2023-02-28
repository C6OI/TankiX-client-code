using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(ScrollRect))]
    public class ResetScrollOnEnable : MonoBehaviour {
        void Start() {
            GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        }

        void OnEnable() {
            GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        }
    }
}