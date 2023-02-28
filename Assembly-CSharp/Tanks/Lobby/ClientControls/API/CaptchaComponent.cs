using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class CaptchaComponent : BehaviourComponent {
        [SerializeField] Image captchaImage;

        [SerializeField] Animator animator;

        public Sprite CaptchaSprite {
            get => captchaImage.sprite;
            set => captchaImage.sprite = value;
        }

        public Animator Animator => animator;
    }
}