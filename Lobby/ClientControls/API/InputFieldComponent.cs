using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(InputField))]
    public class InputFieldComponent : EventMappingComponent {
        [SerializeField] Animator animator;

        [SerializeField] Text errorMessage;

        Text hint;
        protected InputField inputField;

        public virtual InputField InputField {
            get {
                if (inputField == null) {
                    inputField = GetComponent<InputField>();
                }

                return inputField;
            }
        }

        public Animator Animator => animator;

        public virtual string ErrorMessage {
            get => errorMessage.text;
            set {
                errorMessage.text = value;
                Animator.SetBool("HasMessage", !string.IsNullOrEmpty(value));
            }
        }

        public virtual string Input {
            get => InputField.text;
            set => InputField.text = value;
        }

        public virtual string Hint {
            set {
                if (hint == null) {
                    ExtractHint();
                }

                hint.text = value;
            }
        }

        protected override void Awake() {
            base.Awake();
            ExtractHint();
        }

        protected override void Subscribe() => GetComponent<InputField>().onValueChange.AddListener(delegate(string s) {
            if (hint != null) {
                hint.gameObject.SetActive(string.IsNullOrEmpty(s));
            }

            SendEvent<InputFieldValueChangedEvent>();
        });

        void ExtractHint() {
            Text text = InputField.placeholder as Text;

            if (text != null) {
                hint = text;
                InputField.placeholder = null;
            }
        }
    }
}