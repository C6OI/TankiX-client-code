using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class EnterNewPasswordScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text newPassword;

        [SerializeField] Text repeatNewPassword;

        [SerializeField] Text continueButton;

        public virtual string NewPassword {
            set => newPassword.text = value;
        }

        public virtual string RepeatNewPassword {
            set => repeatNewPassword.text = value;
        }

        public virtual string ContinueButton {
            set => continueButton.text = value;
        }
    }
}