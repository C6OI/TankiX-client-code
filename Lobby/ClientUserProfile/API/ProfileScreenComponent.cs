using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientUserProfile.API {
    public class ProfileScreenComponent : MonoBehaviour, Component {
        [SerializeField] GameObject requestFriendButton;

        [SerializeField] GameObject acceptFriendButton;

        [SerializeField] GameObject removeFriendButton;

        [SerializeField] GameObject rejectFriendButton;

        [SerializeField] GameObject revokeFriendButton;

        [SerializeField] Text additionalMessageForButtonText;

        [SerializeField] GameObject logoutButton;

        [SerializeField] GameObject selfUserExperienceIndicator;

        [SerializeField] GameObject remoteUserExperienceIndicator;

        [SerializeField] GameObject goToConfirmEmailScreenButton;

        [SerializeField] GameObject goToChangeUIDScreenButton;

        public GameObject RequestFriendButton => requestFriendButton;

        public GameObject AcceptFriendButton => acceptFriendButton;

        public GameObject RemoveFriendButton => removeFriendButton;

        public GameObject RejectFriendButton => rejectFriendButton;

        public GameObject RevokeFriendButton => revokeFriendButton;

        public Text AdditionalMessageForButtonText => additionalMessageForButtonText;

        public GameObject LogoutButton => logoutButton;

        public GameObject SelfUserExperienceIndicator => selfUserExperienceIndicator;

        public GameObject RemoteUserExperienceIndicator => remoteUserExperienceIndicator;

        public GameObject GoToConfirmEmailScreenButton => goToConfirmEmailScreenButton;

        public GameObject GoToChangeUidScreenButton => goToChangeUIDScreenButton;

        void OnEnable() {
            requestFriendButton.SetActive(false);
            acceptFriendButton.SetActive(false);
            rejectFriendButton.SetActive(false);
            revokeFriendButton.SetActive(false);
            removeFriendButton.SetActive(false);
            additionalMessageForButtonText.gameObject.SetActive(false);
            logoutButton.SetActive(false);
            selfUserExperienceIndicator.SetActive(false);
            remoteUserExperienceIndicator.SetActive(false);
            GoToConfirmEmailScreenButton.SetActive(false);
            goToChangeUIDScreenButton.SetActive(false);
        }
    }
}