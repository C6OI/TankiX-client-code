using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UserLabelStateComponent : BehaviourComponent {
        [SerializeField] Image[] images;

        [SerializeField] CanvasGroup textGroup;

        [SerializeField] TextMeshProUGUI stateText;

        [SerializeField] LocalizedField online;

        [SerializeField] LocalizedField offline;

        [SerializeField] LocalizedField inBattle;

        [SerializeField] Color onlineColor;

        [SerializeField] Color offlineColor;

        [SerializeField] float alpha = 0.6f;

        [SerializeField] bool userInBattle;

        [SerializeField] GameObject userInSquadLabel;

        [SerializeField] Button inviteButton;

        [SerializeField] bool disableInviteOnlyForSquadState;

        public bool CanBeInvited {
            set {
                if (inviteButton != null) {
                    inviteButton.GetComponent<Button>().interactable = value;
                }
            }
        }

        public bool UserInSquad {
            set {
                if (userInSquadLabel != null) {
                    userInSquadLabel.gameObject.SetActive(value);
                }
            }
        }

        public bool DisableInviteOnlyForSquadState => disableInviteOnlyForSquadState;

        void Awake() {
            UserOffline();
        }

        public void UserOnline() {
            SetAlpha(1f);

            if (!userInBattle) {
                stateText.text = online.Value;
                stateText.color = onlineColor;
            }
        }

        public void UserOffline() {
            SetAlpha(alpha);
            stateText.text = offline.Value;
            stateText.color = offlineColor;
        }

        public void UserInBattle() {
            userInBattle = true;
            stateText.text = inBattle.Value;
            stateText.color = onlineColor;
        }

        public void UserOutBattle(bool userOnline) {
            userInBattle = false;

            if (userOnline) {
                UserOnline();
            } else {
                UserOffline();
            }
        }

        public void SetAlpha(float alpha) {
            Image[] array = images;

            foreach (Image image in array) {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                textGroup.alpha = alpha;
            }
        }

        public void SetBattleDescription(string mode, string map) {
            stateText.text = inBattle.Value + " (" + map + ", " + mode + ")";
            stateText.color = onlineColor;
        }
    }
}