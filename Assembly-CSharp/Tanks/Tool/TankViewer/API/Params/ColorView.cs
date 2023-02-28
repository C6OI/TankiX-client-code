using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tool.TankViewer.API.Params {
    public class ColorView : MonoBehaviour {
        public InputField colorInput;

        void Awake() {
            colorInput.onEndEdit.AddListener(OnEndEdit);
        }

        public void SetColor(Color color) {
            colorInput.text = ColorUtility.ToHtmlStringRGB(color).ToLower();
        }

        public Color GetColor() {
            Color color;

            if (ColorUtility.TryParseHtmlString("#" + colorInput.text, out color)) {
                return color;
            }

            return Color.white;
        }

        public void OnEndEdit(string inputText) {
            Color color;

            if (!ColorUtility.TryParseHtmlString("#" + colorInput.text, out color)) {
                colorInput.text = "ffffff";
            }
        }
    }
}