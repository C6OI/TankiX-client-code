using System.Linq;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class KeyboardSettingsInputComponent : MonoBehaviour, Component {
        [SerializeField] public InputActionContainer[] inputActions;

        [SerializeField] GameObject selectionBorder;

        public int id;

        bool _wrongKeyState;

        InputField inputField;

        bool selected;

        [Inject] public static InputManager InputManager { get; set; }

        bool wrongKeyState {
            get => _wrongKeyState;
            set {
                _wrongKeyState = value;
                GetComponent<Animator>().SetBool("wrongKeyState", value);
            }
        }

        void Start() {
            inputField = GetComponent<InputField>();
            SetText();
            inputField.customCaretColor = true;
            inputField.caretColor = Color.clear;
            inputField.selectionColor = Color.clear;
        }

        void Update() {
            if (!selected) {
                return;
            }

            InputKeyCode currentKeyPressed = InputManager.GetCurrentKeyPressed();

            if (currentKeyPressed == null) {
                return;
            }

            KeyCode keyCode = currentKeyPressed.keyCode;

            switch (keyCode) {
                case KeyCode.Backspace:
                case KeyCode.Delete:
                    DeleteKeyBinding();
                    SetText();
                    return;

                case KeyCode.Escape:
                case KeyCode.Mouse0:
                case KeyCode.Mouse1:
                    OnDeselect();
                    return;
            }

            if (CurrentKeyAllow(keyCode)) {
                AssignKeyText(keyCode);
                AssignNewKey(keyCode);
                OnDeselect();
            } else if (keyCode != KeyCode.Mouse0) {
                GetComponent<Animator>().SetTrigger("wrongKeyPressed");
                SetText();
            }
        }

        public void SetText() {
            KeyCode keyCode = LoadAction();

            if (keyCode != 0) {
                AssignKeyText(keyCode);
            } else {
                inputField.text = string.Empty;
            }
        }

        public void SetInputState(bool wrongKey) {
            wrongKeyState = wrongKey;

            if (!(selectionBorder == null)) {
                if (wrongKey) {
                    selectionBorder.SetActive(true);
                } else if (!selected) {
                    selectionBorder.SetActive(false);
                }
            }
        }

        public KeyCode LoadAction() {
            InputAction action = InputManager.GetAction(inputActions[0].actionId, inputActions[0].contextId);

            if (id >= action.keys.Length) {
                return KeyCode.None;
            }

            return action.keys[id];
        }

        public void OnSelect() {
            selected = true;
            GetComponent<Animator>().SetBool("selected", true);

            if (!(selectionBorder == null)) {
                selectionBorder.SetActive(true);
            }
        }

        public void OnDeselect() {
            selected = false;
            GetComponent<Animator>().SetBool("selected", false);

            if (!(selectionBorder == null) && !wrongKeyState) {
                selectionBorder.SetActive(false);
            }
        }

        void DeselectInputField() {
            EventSystem.current.SetSelectedGameObject(transform.parent.gameObject);
        }

        void AssignNewKey(KeyCode newKey) {
            InputActionContainer[] array = inputActions;

            foreach (InputActionContainer inputActionContainer in array) {
                InputManager.ChangeInputActionKey(inputActionContainer.actionId, inputActionContainer.contextId, id, newKey);
            }

            CheckKeys();
        }

        void DeleteKeyBinding() {
            InputActionContainer[] array = inputActions;

            foreach (InputActionContainer inputActionContainer in array) {
                InputManager.DeleteKeyBinding(inputActionContainer.actionId, inputActionContainer.contextId, id);
            }

            CheckKeys();
        }

        void AssignKeyText(KeyCode keyCode) {
            inputField.text = KeyboardSettingsUtil.KeyCodeToString(keyCode);
        }

        public void ResetWrongKeyTrigger() {
            GetComponent<Animator>().ResetTrigger("wrongKeyPressed");
        }

        bool CurrentKeyAllow(KeyCode code) {
            KeyCode[] source = new KeyCode[16] {
                KeyCode.Mouse0,
                KeyCode.Mouse1,
                KeyCode.Mouse2,
                KeyCode.W,
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
                KeyCode.UpArrow,
                KeyCode.DownArrow,
                KeyCode.LeftArrow,
                KeyCode.RightArrow,
                KeyCode.KeypadEnter,
                KeyCode.Return,
                KeyCode.LeftWindows,
                KeyCode.RightWindows,
                KeyCode.Space
            };

            return !source.Contains(code);
        }

        void CheckKeys() {
            GetComponentInParent<KeyboardSettingsScreenComponent>().CheckForOneKeyOnFewActions();
        }
    }
}