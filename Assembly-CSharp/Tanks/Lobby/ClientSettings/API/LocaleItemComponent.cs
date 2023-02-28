using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientSettings.API {
    public class LocaleItemComponent : MonoBehaviour, Component {
        [SerializeField] TextMeshProUGUI caption;

        public void SetText(string caption, string localizedCaption) {
            this.caption.text = caption;
        }
    }
}