using System;
using System.Collections;
using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class InputFieldComponent : EventMappingComponent {
        [SerializeField] Animator animator;

        [SerializeField] Text errorMessageLabel;

        [SerializeField] TextMeshProUGUI errorMessageTMPLabel;

        string errorMessage;

        Text hint;
        protected InputField inputField;

        protected TMP_InputField tmpInputField;

        public virtual InputField InputField {
            get {
                if (inputField == null) {
                    inputField = GetComponent<InputField>();
                }

                return inputField;
            }
        }

        public virtual TMP_InputField TMPInputField {
            get {
                if (tmpInputField == null) {
                    tmpInputField = GetComponent<TMP_InputField>();
                }

                return tmpInputField;
            }
        }

        bool defaultInput => InputField != null;

        public GameObject InputFieldGameObject {
            get {
                if (defaultInput) {
                    return InputField.gameObject;
                }

                return TMPInputField.gameObject;
            }
        }

        public Animator Animator => animator;

        public virtual string ErrorMessage {
            get => errorMessage;
            set {
                errorMessage = value;
                bool value2 = !string.IsNullOrEmpty(value);
                Animator.SetBool("HasMessage", value2);

                if (defaultInput) {
                    errorMessageLabel.text = errorMessage;
                } else {
                    errorMessageTMPLabel.text = errorMessage;
                }
            }
        }

        public virtual string Input {
            get {
                if (defaultInput) {
                    return InputField.text;
                }

                return TMPInputField.text;
            }
            set {
                if (defaultInput) {
                    InputField.text = value;
                } else {
                    TMPInputField.text = value;
                }
            }
        }

        public virtual string Hint {
            set {
                if (hint == null) {
                    ExtractHint();
                }

                if (hint != null) {
                    hint.text = value;
                }
            }
        }

        protected override void Awake() {
            base.Awake();

            if (TMPInputField != null) {
                TMPInputField.scrollSensitivity = 0f;
            }

            ExtractHint();
        }

        public static bool IsAnyInputFieldInFocus() {
            GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

            if (currentSelectedGameObject == null) {
                return false;
            }

            InputField component = currentSelectedGameObject.GetComponent<InputField>();

            if (component != null) {
                return component.isFocused;
            }

            TMP_InputField component2 = currentSelectedGameObject.GetComponent<TMP_InputField>();

            if (component2 != null) {
                return component2.isFocused;
            }

            return false;
        }

        protected override void Subscribe() {
            if (defaultInput) {
                InputField.onValueChanged.AddListener(delegate(string s) {
                    SendInputFieldValueChangedEvent(s);
                });

                return;
            }

            TMPInputField.onValueChanged.AddListener(delegate(string s) {
                SendInputFieldValueChangedEvent(s);
            });

            TMPInputField.onSelect.AddListener(delegate(string s) {
                if (!string.IsNullOrEmpty(s)) {
                    SendInputFieldValueChangedEvent(s);
                }
            });
        }

        void SendInputFieldValueChangedEvent(string s) {
            if (hint != null) {
                hint.gameObject.SetActive(string.IsNullOrEmpty(s));
            }

            SendEvent<InputFieldValueChangedEvent>();
        }

        public void SendValueChangedEvent() {
            SendInputFieldValueChangedEvent(Input);
        }

        void ExtractHint() {
            if (defaultInput) {
                Text text = InputField.placeholder as Text;

                if (text != null) {
                    hint = text;
                    InputField.placeholder = null;
                }
            }
        }

        public void ActivateInputField(Action onComplete) {
            StartCoroutine(_ActivateInputField(onComplete));
        }

        IEnumerator _ActivateInputField(Action onComplete) {
            yield return new WaitForEndOfFrame();

            if (tmpInputField != null) {
                tmpInputField.Select();
            } else if (inputField != null) {
                inputField.Select();
            }

            onComplete();
        }
    }
}