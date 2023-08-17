using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientUserProfile.Impl {
    public class ChangeUserEmailScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text sendButtonText;

        public string SendButton {
            set => sendButtonText.text = value.ToUpper();
        }
    }
}