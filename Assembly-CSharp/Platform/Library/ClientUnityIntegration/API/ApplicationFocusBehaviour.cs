using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public class ApplicationFocusBehaviour : MonoBehaviour {
        public static ApplicationFocusBehaviour INSTANCE;

        public bool Focused { get; private set; } = true;

        void Awake() {
            INSTANCE = this;
        }

        void OnApplicationFocus(bool focused) {
            Focused = focused;
        }
    }
}