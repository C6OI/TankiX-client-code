using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.Impl {
    public class TextToUppercase : MonoBehaviour {
        Text inputField;

        void Start() {
            inputField = GetComponent<Text>();
            ToUpperCase();
        }

        public void ToUpperCase() => inputField.text = inputField.text.ToUpper();
    }
}