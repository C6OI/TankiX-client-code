using UnityEngine;
using UnityEngine.UI;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Platform.Library.ClientUnityIntegration.API {
    [RequireComponent(typeof(Button))]
    public abstract class ButtonMappingComponentBase<T> : EventMappingComponent where T : Event, new() {
        Button button;

        public Button Button {
            get {
                if (button == null) {
                    button = GetComponent<Button>();
                }

                return button;
            }
        }

        protected override void Subscribe() {
            Button.onClick.AddListener(delegate {
                SendEvent<T>();
            });
        }
    }
}