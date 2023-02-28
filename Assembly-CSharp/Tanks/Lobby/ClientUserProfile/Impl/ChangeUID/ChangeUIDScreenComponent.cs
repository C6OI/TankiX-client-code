using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.Impl.ChangeUID {
    public class ChangeUIDScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text inputHint;

        public string InputHint {
            set => inputHint.text = value;
        }
    }
}