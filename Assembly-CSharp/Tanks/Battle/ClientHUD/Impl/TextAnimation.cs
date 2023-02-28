using TMPro;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(NormalizedAnimatedValue))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextAnimation : MonoBehaviour {
        bool inFadeMode;
        string targetText;

        public string Text {
            set {
                if (targetText != value) {
                    GetComponent<TextMeshProUGUI>().text = value;
                    GetComponent<Animator>().SetTrigger("Start");
                }
            }
        }

        void OnDisable() {
            targetText = string.Empty;
            GetComponent<TextMeshProUGUI>().text = string.Empty;
        }

        void SwitchMode() { }
    }
}