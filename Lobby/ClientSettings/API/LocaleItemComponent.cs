using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientSettings.API {
    public class LocaleItemComponent : MonoBehaviour, Component {
        [SerializeField] Text caption;

        [SerializeField] Text localizedCaption;

        public void SetText(string caption, string localizedCaption) {
            this.caption.text = caption;
            this.localizedCaption.text = localizedCaption;
        }
    }
}