using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class LobbyUserListItemComponent : BehaviourComponent {
        public bool selfUser;

        public GameObject userInfo;

        public GameObject userSearchingText;

        public GameObject userLabelPrefab;

        [SerializeField] TextMeshProUGUI turretName;

        [SerializeField] TextMeshProUGUI hullName;

        [SerializeField] TextMeshProUGUI turretIcon;

        [SerializeField] TextMeshProUGUI hullIcon;

        [SerializeField] GameObject readyButton;

        [SerializeField] TextMeshProUGUI statusLabel;

        [SerializeField] LocalizedField notReadyText;

        [SerializeField] LocalizedField readyText;

        [SerializeField] LocalizedField inBattleText;

        [SerializeField] Color notReadyColor;

        [SerializeField] Color readyColor;

        [SerializeField] GameObject lobbyMasterIcon;

        [SerializeField] Button buttonScript;

        TeamListUserData currentUserData;

        bool showSearchingText = true;
        public Entity userEntity;

        GameObject userLabelInstance;

        public bool Empty => userEntity == null;

        public bool ShowSearchingText {
            get => showSearchingText;
            set {
                showSearchingText = value;
                userSearchingText.SetActive(Empty && showSearchingText);
            }
        }

        public bool Master {
            set => lobbyMasterIcon.SetActive(value);
        }

        void OnEnable() {
            Init();
        }

        void OnDisable() {
            if (!Empty && ClientUnityIntegrationUtils.HasEngine()) {
                if (userEntity.HasComponent<LobbyUserListItemComponent>()) {
                    userEntity.RemoveComponent<LobbyUserListItemComponent>();
                }

                if (userEntity.HasComponent<UserSquadColorComponent>()) {
                    userEntity.RemoveComponent<UserSquadColorComponent>();
                }
            }
        }

        public void SetNotReady() {
            if (selfUser) {
                ShowReadyButton();
                return;
            }

            readyButton.SetActive(false);
            statusLabel.gameObject.SetActive(true);
            statusLabel.text = notReadyText;
            statusLabel.color = notReadyColor;
        }

        public void ShowReadyButton() {
            statusLabel.gameObject.SetActive(false);
            readyButton.SetActive(true);
            statusLabel.text = string.Empty;
            statusLabel.color = notReadyColor;
        }

        public void SetReady() {
            readyButton.SetActive(false);
            statusLabel.gameObject.SetActive(true);
            statusLabel.text = readyText;
            statusLabel.color = readyColor;
        }

        public void SetInBattle() {
            readyButton.SetActive(false);
            statusLabel.gameObject.SetActive(true);
            statusLabel.text = inBattleText;
            statusLabel.color = readyColor;
        }

        void Init() {
            SetNotReady();

            if (!Empty) {
                if (!userEntity.HasComponent<LobbyUserListItemComponent>()) {
                    userEntity.AddComponent(this);
                }

                if (!userEntity.HasComponent<UserSquadColorComponent>()) {
                    UserSquadColorComponent component = GetComponent<UserSquadColorComponent>();
                    userEntity.AddComponent(component);
                }

                if (userLabelInstance != null) {
                    Destroy(userLabelInstance);
                }

                userLabelInstance = Instantiate(userLabelPrefab);
                bool premium = userEntity.HasComponent<PremiumAccountBoostComponent>();
                userLabelInstance = new UserLabelBuilder(userEntity.Id, userLabelInstance, userEntity.GetComponent<UserAvatarComponent>().Id, premium).SkipLoadUserFromServer().Build();
                userLabelInstance.transform.SetParent(turretName.transform.parent, false);
                userLabelInstance.transform.SetSiblingIndex(0);
            } else {
                userSearchingText.SetActive(showSearchingText);
                userInfo.SetActive(false);
            }

            bool flag = !Empty && !selfUser;

            if (buttonScript != null) {
                buttonScript.interactable = flag;
            }

            RightMouseButtonClickSender component2 = GetComponent<RightMouseButtonClickSender>();

            if (component2 != null && !flag) {
                component2.enabled = false;
            }
        }

        public void UpdateEquipment(string hullName, long hullIconId, string turretName, long turretIconId) {
            this.turretName.text = turretName;
            turretIcon.text = "<sprite name=\"" + turretIconId + "\">";
            this.hullName.text = hullName;
            hullIcon.text = "<sprite name=\"" + hullIconId + "\">";
        }

        public void Select() { }
    }
}