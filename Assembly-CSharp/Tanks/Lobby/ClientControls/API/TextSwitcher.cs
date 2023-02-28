using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class TextSwitcher : MonoBehaviour {
        public Text nextText;

        public Text currentText;

        public void Switch() {
            string text = nextText.text;
            nextText.text = currentText.text;
            currentText.text = text;
        }
    }
}