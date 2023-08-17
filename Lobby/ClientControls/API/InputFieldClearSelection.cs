using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(InputField))]
    public class InputFieldClearSelection : MonoBehaviour {
        InputFieldComponent inputField;

        bool needClear;

        void Awake() => inputField = gameObject.GetComponent<InputFieldComponent>();

        void LateUpdate() {
            if (needClear) {
                inputField.InputField.selectionAnchorPosition = inputField.InputField.text.Length;
                inputField.InputField.selectionFocusPosition = inputField.InputField.text.Length;
                needClear = false;
            }
        }

        public void OnSelect() => needClear = true;
    }
}