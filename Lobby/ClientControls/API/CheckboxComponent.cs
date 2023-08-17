using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public class CheckboxComponent : EventMappingComponent {
        [SerializeField] Text label;

        [SerializeField] Toggle toggle;

        bool defaultValue;

        public virtual string LabelText {
            get => label.text;
            set => label.text = value;
        }

        public virtual bool IsChecked {
            get => toggle.isOn;
            set => toggle.isOn = value;
        }

        protected override void Awake() {
            base.Awake();
            defaultValue = toggle.isOn;
        }

        void OnEnable() => toggle.isOn = defaultValue;

        protected override void Subscribe() => toggle.onValueChanged.AddListener(delegate(bool isOn) {
            if (isOn) {
                SendEvent<CheckedCheckboxEvent>();
            } else {
                SendEvent<UncheckedCheckboxEvent>();
            }
        });
    }
}