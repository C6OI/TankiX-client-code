using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class DefaultTooltipContent : MonoBehaviour, ITooltipContent {
        [SerializeField] TextMeshProUGUI text;

        public void Init(object data) {
            string text = data as string;
            text = text.Replace("\\n", "\n");
            this.text.SetText(text);
        }
    }
}