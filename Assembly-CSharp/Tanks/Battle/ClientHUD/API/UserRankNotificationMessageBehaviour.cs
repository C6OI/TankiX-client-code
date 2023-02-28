using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.API {
    public class UserRankNotificationMessageBehaviour : BaseUserNotificationMessageBehaviour {
        [SerializeField] Image iconImage;

        [SerializeField] ImageListSkin icon;

        [SerializeField] Text message;

        public ImageListSkin Icon => icon;

        public Image IconImage => iconImage;

        public Text Message => message;

        void OnIconFlyReady() {
            animator.SetTrigger("TextFadeIn");
        }
    }
}