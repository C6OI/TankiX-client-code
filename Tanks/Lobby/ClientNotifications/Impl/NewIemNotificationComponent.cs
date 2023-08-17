using Lobby.ClientControls.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class NewIemNotificationComponent : BehaviourComponent {
        [SerializeField] Text header;

        [SerializeField] Text text;

        [SerializeField] Image image;

        [SerializeField] ImageSkin skin;

        public string Header {
            get => header.text;
            set => header.text = value;
        }

        public string Text {
            get => text.text;
            set => text.text = value;
        }

        public void SetImage(string spriteUid) {
            skin.SpriteUid = spriteUid;
            image.enabled = true;
            skin.enabled = true;
        }
    }
}