using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class FriendsListUIComponent : ConfirmDialogComponent {
        [SerializeField] TextMeshProUGUI emptyFriendsListLabel;

        [SerializeField] LocalizedField noFriendsOnlineText;

        [SerializeField] LocalizedField noFriendsText;

        [SerializeField] LocalizedField noFriendsIncomingText;

        [SerializeField] GameObject addAllButton;

        [SerializeField] GameObject rejectAllButton;

        [SerializeField] TMP_InputField searchingInput;

        [SerializeField] float inputDelayInSec;

        public FriendsUITableView tableView;

        [SerializeField] FriendsShowMode defaultShowMode;

        [SerializeField] Button AllFriendsButton;

        [SerializeField] Button IncomnigFriendsButton;

        readonly List<UserCellData> accepted = new();

        readonly List<UserCellData> incoming = new();

        bool inputChanged;

        float lastChangeTime;

        bool loaded;

        readonly List<UserCellData> outgoing = new();

        FriendsShowMode showMode;

        [Inject] public static UnityTime UnityTime { get; set; }

        public FriendsShowMode ShowMode {
            get => showMode;
            set {
                showMode = value;
                List<UserCellData> list = new();

                switch (ShowMode) {
                    case FriendsShowMode.AcceptedAndOutgoing:
                        AllFriendsButton.GetComponent<Animator>().SetBool("activated", true);
                        IncomnigFriendsButton.GetComponent<Animator>().SetBool("activated", false);
                        list.AddRange(accepted);
                        list.AddRange(outgoing);
                        break;

                    case FriendsShowMode.Incoming:
                        AllFriendsButton.GetComponent<Animator>().SetBool("activated", false);
                        IncomnigFriendsButton.GetComponent<Animator>().SetBool("activated", true);
                        list.AddRange(incoming);
                        break;

                    default:
                        list.AddRange(accepted);
                        break;
                }

                UpdateTable(list);
                ResetButtons();
            }
        }

        void Awake() {
            if (AllFriendsButton != null) {
                AllFriendsButton.onClick.AddListener(ShowAcceptedAndOutgoingFriends);
            }

            if (IncomnigFriendsButton != null) {
                IncomnigFriendsButton.onClick.AddListener(ShowIncomingFriends);
            }
        }

        void Update() {
            if (loaded) {
                CheckContentVisibility();
                InputUpdate();
            }
        }

        protected override void OnEnable() {
            base.OnEnable();
            searchingInput.text = string.Empty;
            searchingInput.scrollSensitivity = 0f;
            searchingInput.onValueChanged.AddListener(OnSearchingInputValueChanged);
            ShowMode = defaultShowMode;
            Invoke("ActivateInputField", 0.5f);
        }

        void OnDisable() {
            CancelInvoke();
            incoming.Clear();
            accepted.Clear();
            outgoing.Clear();
            loaded = false;
            searchingInput.onValueChanged.RemoveListener(OnSearchingInputValueChanged);
        }

        public void AddFriends(Dictionary<long, string> FriendsIdsAndNicknames, FriendType friendType) {
            foreach (long key in FriendsIdsAndNicknames.Keys) {
                AddItem(key, FriendsIdsAndNicknames[key], friendType);
            }

            loaded = true;
            ShowMode = showMode;
        }

        public void AddItem(long userId, string userUid, FriendType friendType) {
            UserCellData item = new(userId, userUid);

            switch (friendType) {
                case FriendType.Incoming:
                    incoming.Add(item);
                    break;

                case FriendType.Outgoing:
                    outgoing.Add(item);
                    break;

                default:
                    accepted.Add(item);
                    break;
            }
        }

        public void UpdateTable(List<UserCellData> items) {
            tableView.Items = items;
            tableView.FilterString = tableView.FilterString;
        }

        public void InputUpdate() {
            if (inputChanged && UnityTime.time - lastChangeTime > inputDelayInSec) {
                UpdateFilterString();
                inputChanged = false;
            }
        }

        void OnSearchingInputValueChanged(string value) {
            inputChanged = true;
            lastChangeTime = UnityTime.time;
        }

        void UpdateFilterString() {
            tableView.FilterString = searchingInput.text;
        }

        void CheckContentVisibility() {
            if (tableView.Items.Count == 0) {
                string empty = string.Empty;

                switch (ShowMode) {
                    case FriendsShowMode.AcceptedAndOutgoing:
                        empty = noFriendsText.Value;
                        break;

                    case FriendsShowMode.Incoming:
                        empty = noFriendsIncomingText.Value;
                        break;

                    default:
                        empty = noFriendsOnlineText.Value;
                        break;
                }

                if (!emptyFriendsListLabel.gameObject.activeSelf || emptyFriendsListLabel.text != empty) {
                    emptyFriendsListLabel.text = empty;
                    emptyFriendsListLabel.gameObject.SetActive(true);
                }
            } else if (emptyFriendsListLabel.gameObject.activeSelf) {
                emptyFriendsListLabel.gameObject.SetActive(false);
            }
        }

        void ActivateInputField() {
            searchingInput.ActivateInputField();
        }

        public void RemoveItem(long userId, bool toRight) {
            int userDataIndexById = GetUserDataIndexById(userId, incoming);

            if (userDataIndexById != -1) {
                incoming.RemoveAt(userDataIndexById);
            } else {
                userDataIndexById = GetUserDataIndexById(userId, accepted);

                if (userDataIndexById != -1) {
                    accepted.RemoveAt(userDataIndexById);
                } else {
                    userDataIndexById = GetUserDataIndexById(userId, outgoing);

                    if (userDataIndexById != -1) {
                        outgoing.RemoveAt(userDataIndexById);
                    }
                }
            }

            tableView.RemoveUser(userId, toRight);
        }

        public int GetUserDataIndexById(long userId, List<UserCellData> list) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i].id == userId) {
                    return i;
                }
            }

            return -1;
        }

        public void ShowAcceptedAndOutgoingFriends() {
            ShowMode = FriendsShowMode.AcceptedAndOutgoing;
        }

        public void ShowIncomingFriends() {
            ShowMode = FriendsShowMode.Incoming;
        }

        public void ResetButtons() {
            if (!(addAllButton == null) && !(rejectAllButton == null)) {
                addAllButton.SetActive(false);
                rejectAllButton.SetActive(false);
            }
        }

        public void EnableAddAllButton() {
            addAllButton.SetActive(true);
            rejectAllButton.SetActive(false);
        }

        public void DisableAddAllButton() {
            addAllButton.SetActive(true);
        }

        public void EnableRejectAllButton() {
            addAllButton.SetActive(false);
            rejectAllButton.SetActive(true);
        }

        public void DisableRejectAllButton() {
            rejectAllButton.SetActive(true);
        }

        public void ClearIncoming(bool moveToAccepted) {
            List<UserCellData> list = new(incoming);
            incoming.Clear();

            foreach (UserCellData item in list) {
                tableView.RemoveUser(item.id, !moveToAccepted);

                if (moveToAccepted) {
                    AddItem(item.id, item.uid, FriendType.Accepted);
                }
            }
        }

        void ItemClearDone(UserListItemComponent item) {
            item.transform.SetParent(null, false);
            Destroy(item.gameObject);
        }
    }
}