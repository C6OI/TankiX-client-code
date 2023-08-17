using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(ScrollRect))]
    public class ResetScrollOnEnable : MonoBehaviour {
        void OnEnable() => GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
    }
}