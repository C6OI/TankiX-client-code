using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientFriends.Impl {
    public class FriendsScreenLocalizationComponent : MonoBehaviour, Component {
        [SerializeField] Text emptyListNotificationText;

        [SerializeField] Text acceptedFriendHeader;

        [SerializeField] Text possibleFriendHeader;

        [SerializeField] Text searchButtonText;

        [SerializeField] Text searchUserHint;

        [SerializeField] Text searchUserError;

        [SerializeField] Text profileButtonText;

        [SerializeField] Text battleButtonText;

        [SerializeField] Text acceptButtonText;

        [SerializeField] Text rejectButtonText;

        [SerializeField] Text revokeButtonText;

        [SerializeField] Text removeButtonText;

        [SerializeField] Text removeButtonConfirmText;

        [SerializeField] Text removeButtonCancelText;

        public string EmptyListNotificationText {
            set => emptyListNotificationText.text = value;
        }

        public string AcceptedFriendHeader {
            set => acceptedFriendHeader.text = value;
        }

        public string PossibleFriendHeader {
            set => possibleFriendHeader.text = value;
        }

        public string SearchButtonText {
            set => searchButtonText.text = value;
        }

        public string SearchUserHint {
            set => searchUserHint.text = value;
        }

        public string SearchUserError {
            set => searchUserError.text = value;
        }

        public string ProfileButtonText {
            set => profileButtonText.text = value;
        }

        public string BattleButtonText {
            set => battleButtonText.text = value;
        }

        public string AcceptButtonText {
            set => acceptButtonText.text = value;
        }

        public string RejectButtonText {
            set => rejectButtonText.text = value;
        }

        public string RevokeButtonText {
            set => revokeButtonText.text = value;
        }

        public string RemoveButtonText {
            set => removeButtonText.text = value;
        }

        public string RemoveButtonConfirmText {
            set => removeButtonConfirmText.text = value;
        }

        public string RemoveButtonCancelText {
            set => removeButtonCancelText.text = value;
        }
    }
}