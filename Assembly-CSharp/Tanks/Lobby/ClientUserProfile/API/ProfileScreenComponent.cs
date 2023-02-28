using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class ProfileScreenComponent : BehaviourComponent, NoScaleScreen {
        [SerializeField] TextMeshProUGUI otherPlayerNickname;

        [SerializeField] GameObject addToFriendRow;

        [SerializeField] GameObject friendRequestRow;

        [SerializeField] GameObject revokeFriendRow;

        [SerializeField] GameObject removeFriendRow;

        [SerializeField] GameObject enterBattleAsSpectatorRow;

        [SerializeField] ImageListSkin leagueBorder;

        [SerializeField] ImageSkin avatar;

        [SerializeField] GameObject _premiumFrame;

        public GameObject selfUserAccountButtonsTab;

        public GameObject otherUserAccountButtonsTab;

        [SerializeField] Color friendColor;

        public ImageListSkin LeagueBorder => leagueBorder;

        public ImageSkin Avatar => avatar;

        public bool IsPremium {
            set => _premiumFrame.SetActive(value);
        }

        public TextMeshProUGUI OtherPlayerNickname => otherPlayerNickname;

        public GameObject AddToFriendRow => addToFriendRow;

        public GameObject FriendRequestRow => friendRequestRow;

        public GameObject RemoveFriendRow => removeFriendRow;

        public GameObject RevokeFriendRow => revokeFriendRow;

        public GameObject EnterBattleAsSpectatorRow => enterBattleAsSpectatorRow;

        void OnEnable() {
            AddToFriendRow.SetActive(false);
            friendRequestRow.SetActive(false);
            revokeFriendRow.SetActive(false);
            removeFriendRow.SetActive(false);
            enterBattleAsSpectatorRow.SetActive(false);
            otherPlayerNickname.gameObject.SetActive(false);
        }

        public void SetPlayerColor(bool playerIsFriend) {
            otherPlayerNickname.color = !playerIsFriend ? Color.white : friendColor;
        }

        public void HideOnNewItemNotificationShow() {
            GetComponent<Animator>().SetBool("newItemNotification", true);
        }

        public void ShowOnNewItemNotificationCLose() {
            GetComponent<Animator>().SetBool("newItemNotification", false);
        }
    }
}