using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class EnterUserEmailScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text rightPanelHint;

        [SerializeField] Text continueButton;

        public virtual string RightPanelHint {
            set => rightPanelHint.text = value;
        }

        public virtual string ContinueButton {
            set => continueButton.text = value.ToUpper();
        }
    }
}