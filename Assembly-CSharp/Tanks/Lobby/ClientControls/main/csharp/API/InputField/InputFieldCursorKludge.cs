using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.main.csharp.API.InputField {
    public class InputFieldCursorKludge : MonoBehaviour {
        public InputFieldComponent inputField;

        public float yPivotOffset;

        void Start() {
            RectTransform component = inputField.InputField.gameObject.GetComponent<RectTransform>();
            Vector2 pivot = component.pivot;
            pivot.y += yPivotOffset;
            component.pivot = pivot;
        }
    }
}