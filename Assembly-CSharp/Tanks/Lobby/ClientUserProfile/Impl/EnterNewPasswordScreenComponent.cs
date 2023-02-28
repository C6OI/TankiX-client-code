using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class EnterNewPasswordScreenComponent : LocalizedScreenComponent, NoScaleScreen {
        [SerializeField] TextMeshProUGUI newPassword;

        [SerializeField] TextMeshProUGUI repeatNewPassword;

        [SerializeField] TextMeshProUGUI continueButton;

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