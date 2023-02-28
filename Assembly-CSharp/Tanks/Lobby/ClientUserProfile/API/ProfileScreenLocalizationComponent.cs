using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class ProfileScreenLocalizationComponent : MonoBehaviour, Component {
        [SerializeField] Text logoutButtonText;

        [SerializeField] Text logoutButtonConfirmText;

        [SerializeField] Text logoutButtonCancelText;

        [SerializeField] Text requestFriendButtonText;

        [SerializeField] Text revokeFriendButtonText;

        [SerializeField] Text acceptFriendButtonText;

        [SerializeField] Text rejectFriendButtonText;

        [SerializeField] Text removeFriendButtonText;

        [SerializeField] Text removeFriendButtonConfirmText;

        [SerializeField] Text removeFriendButtonCancelText;

        [SerializeField] Text goToConfirmEmailScreenButtonText;

        [SerializeField] Text goToChangeUIDScreenButtonText;

        [SerializeField] Text goToPromoCodesScreenButtonText;

        [SerializeField] Text enterAsSpectatorButtonText;

        public string RequestFriendButtonText {
            set => requestFriendButtonText.text = value.ToUpper();
        }

        public string RevokeFriendButtonText {
            set => revokeFriendButtonText.text = value.ToUpper();
        }

        public string AcceptFriendButtonText {
            set => acceptFriendButtonText.text = value.ToUpper();
        }

        public string RejectFriendButtonText {
            set => rejectFriendButtonText.text = value.ToUpper();
        }

        public string RemoveFriendButtonText {
            set => removeFriendButtonText.text = value.ToUpper();
        }

        public string RemoveFriendButtonConfirmText {
            set => removeFriendButtonConfirmText.text = value.ToUpper();
        }

        public string RemoveFriendButtonCancelText {
            set => removeFriendButtonCancelText.text = value.ToUpper();
        }

        public string LogoutButtonText {
            set => logoutButtonText.text = value.ToUpper();
        }

        public string LogoutButtonConfirmText {
            set => logoutButtonConfirmText.text = value.ToUpper();
        }

        public string LogoutButtonCancelText {
            set => logoutButtonCancelText.text = value.ToUpper();
        }

        public string GoToConfirmEmailScreenButtonText {
            set => goToConfirmEmailScreenButtonText.text = value.ToUpper();
        }

        public string GoToChangeUIDScreenButtonText {
            set => goToChangeUIDScreenButtonText.text = value.ToUpper();
        }

        public string GoToPromoCodesScreenButtonText {
            set => goToPromoCodesScreenButtonText.text = value.ToUpper();
        }

        public string EnterAsSpectatorButtonText {
            set => enterAsSpectatorButtonText.text = value.ToUpper();
        }

        public string ProfileHeaderText { get; set; }

        public string MyProfileHeaderText { get; set; }

        public string FriendsProfileHeaderText { get; set; }

        public string OfferFriendShipText { get; set; }

        public string FriendRequestSentText { get; set; }
    }
}