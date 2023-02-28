using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ProfileAccountSectionUIComponent : BehaviourComponent {
        public LocalizedField UnconfirmedLocalization;

        [SerializeField] TextMeshProUGUI userChangeNickname;

        [SerializeField] Color emailColor = Color.green;

        [SerializeField] int emailMessageSize = 18;

        [SerializeField] Toggle subscribeCheckbox;

        [SerializeField] UserEmailUIComponent userEmail;

        public Color EmailColor => emailColor;

        public int EmailMessageSize => emailMessageSize;

        public string UserNickname {
            get => userChangeNickname.text;
            set => userChangeNickname.text = value;
        }

        public virtual bool Subscribe {
            get => subscribeCheckbox.isOn;
            set => subscribeCheckbox.isOn = value;
        }

        public void SetEmail(string format, string email, string unconfirmedEmail) {
            userEmail.FormatText = format;
            userEmail.UnconfirmedEmail = unconfirmedEmail;
            userEmail.Email = email;
        }
    }
}