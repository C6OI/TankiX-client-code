using Platform.Library.ClientUnityIntegration.API;
using UnityEngine.UI;

namespace Lobby.ClientFriends.Impl {
    public class SearchUserInputFieldComponent : BehaviourComponent {
        InputField inputField;

        public string SearchString {
            get => inputField.text;
            set => inputField.text = value;
        }

        public void Start() => inputField = GetComponent<InputField>();
    }
}