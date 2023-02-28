using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class TMPLocalize : MonoBehaviour {
        [SerializeField] string uid;

        public string TextUid => uid;

        protected void Awake() {
            if (Application.isPlaying) {
                string text = LocalizationUtils.Localize(uid);
                TextMeshProUGUI component = GetComponent<TextMeshProUGUI>();

                if (!string.IsNullOrEmpty(text) && component != null) {
                    text = text.Replace("\\n", "\n");
                    component.text = text;
                }
            }
        }
    }
}