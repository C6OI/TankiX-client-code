using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatUserListUIComponent : ConfirmDialogComponent {
        [SerializeField] TextMeshProUGUI emptyFriendsListLabel;

        [SerializeField] LocalizedField noFriendsOnlineText;

        [SerializeField] TMP_InputField searchingInput;

        [SerializeField] float inputDelayInSec;

        [SerializeField] ChatUserListUITableView tableView;

        [SerializeField] ChatUserListShowMode defaultShowMode;

        [SerializeField] Button PartipientsButton;

        [SerializeField] Button FriendsButton;

        readonly List<UserCellData> friends = new();

        bool friendsLoaded;

        bool inputChanged;

        float lastChangeTime;

        readonly List<UserCellData> participants = new();

        bool participantsLoaded;

        readonly List<UserCellData> pending = new();

        bool pendingLoaded;

        ChatUserListShowMode showMode;

        [Inject] public static UnityTime UnityTime { get; set; }

        bool loaded => friendsLoaded && participantsLoaded && pendingLoaded;

        public ChatUserListShowMode ShowMode {
            get => showMode;
            set {
                showMode = value;
                List<UserCellData> list = new();

                switch (showMode) {
                    case ChatUserListShowMode.Participants:
                        PartipientsButton.GetComponent<Animator>().SetBool("activated", true);
                        FriendsButton.GetComponent<Animator>().SetBool("activated", false);
                        list.AddRange(participants);
                        break;

                    case ChatUserListShowMode.Invite:
                        PartipientsButton.GetComponent<Animator>().SetBool("activated", false);
                        FriendsButton.GetComponent<Animator>().SetBool("activated", true);
                        list.AddRange(friends.Where(x => !participants.Exists(p => p.id == x.id) && !pending.Exists(p => p.id == x.id)));
                        break;
                }

                UpdateTable(list);
            }
        }

        void Awake() {
            if (PartipientsButton != null) {
                PartipientsButton.onClick.AddListener(ShowParticipants);
            }

            if (FriendsButton != null) {
                FriendsButton.onClick.AddListener(ShowFriends);
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
            searchingInput.Select();
            searchingInput.onValueChanged.AddListener(OnSearchingInputValueChanged);
            ShowMode = defaultShowMode;
        }

        void OnDisable() {
            pending.Clear();
            participants.Clear();
            friends.Clear();
            friendsLoaded = false;
            searchingInput.onValueChanged.RemoveListener(OnSearchingInputValueChanged);
        }

        public void AddFriends(Dictionary<long, string> FriendsIdsAndNicknames) {
            foreach (long key in FriendsIdsAndNicknames.Keys) {
                UserCellData item = new(key, FriendsIdsAndNicknames[key]);
                friends.Add(item);
            }

            friendsLoaded = true;
            ShowMode = showMode;
        }

        public void AddParticipants() { }

        public void AddFriend(Dictionary<long, string> FriendsIdsAndNicknames) {
            foreach (long key in FriendsIdsAndNicknames.Keys) {
                UserCellData item = new(key, FriendsIdsAndNicknames[key]);
                friends.Add(item);
            }

            friendsLoaded = true;
            ShowMode = showMode;
        }

        public void RemoveFriend(long userId) {
            friends.RemoveAll(x => x.id == userId);
            ShowMode = showMode;
        }

        public void AddFriend(long userId, string userUid) {
            UserCellData item = new(userId, userUid);
            friends.Add(item);
            ShowMode = showMode;
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
                empty = noFriendsOnlineText.Value;

                if (!emptyFriendsListLabel.gameObject.activeSelf || emptyFriendsListLabel.text != empty) {
                    emptyFriendsListLabel.text = empty;
                    emptyFriendsListLabel.gameObject.SetActive(true);
                }
            } else if (emptyFriendsListLabel.gameObject.activeSelf) {
                emptyFriendsListLabel.gameObject.SetActive(false);
            }
        }

        public int GetUserDataIndexById(long userId, List<UserCellData> list) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i].id == userId) {
                    return i;
                }
            }

            return -1;
        }

        public void ShowParticipants() {
            ShowMode = ChatUserListShowMode.Participants;
        }

        public void ShowFriends() {
            ShowMode = ChatUserListShowMode.Invite;
        }
    }
}