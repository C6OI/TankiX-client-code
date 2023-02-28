using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatDialogComponent : BehaviourComponent, MainScreenComponent.IPanelShowListener {
        public enum ChatSectionType {
            Common = 0,
            Group = 1,
            Personal = 2
        }

        const int MAX_MESSAGES = 50;

        [SerializeField] CanvasGroup maximazedCanvasGroup;

        [SerializeField] CanvasGroup minimazedCanvasGroup;

        [SerializeField] GameObject minimizeButton;

        [SerializeField] GameObject maximizeButton;

        [SerializeField] int baseBottomHeight = 60;

        [SerializeField] int bottomHeightLineAdditional = 24;

        [SerializeField] RectTransform bottom;

        [SerializeField] TMP_InputField inputField;

        [SerializeField] GameObject inputFieldInactivePlaceholder;

        [SerializeField] GameObject sendButton;

        [SerializeField] TMP_Text lastMessage;

        [SerializeField] TMP_Text unreadCounter;

        [SerializeField] GameObject unreadBadge;

        [SerializeField] ScrollRect messagesScroll;

        [SerializeField] ImageSkin activePersonalChannelIcon;

        [SerializeField] ImageSkin activeNotPersonalChannelIcon;

        [SerializeField] GameObject chatIcon;

        [SerializeField] GameObject userIcon;

        [SerializeField] TMP_Text activeChannelName;

        [SerializeField] MessageObject firstSelfMessagePrefab;

        [SerializeField] MessageObject secondSelfMessagePrefab;

        [SerializeField] MessageObject firstOpponentMessagePrefab;

        [SerializeField] MessageObject secondOpponentMessagePrefab;

        [SerializeField] Transform messagesRoot;

        [SerializeField] List<ChatUISettings> uiSettings;

        [SerializeField] List<ChannelRoot> channelRoots;

        bool autoScroll = true;

        int caretCrutch;

        int forceNewLine;

        bool hidden;

        float lastScrollPos;

        readonly int MAX_MESSAGE_LENGTH = 200;

        readonly int MAX_NEW_LINES = 5;

        List<MessageObject> messages = new();

        int unread;

        int waitMessagesScroll;

        public int Unread {
            get => unread;
            set {
                unread = value > 0 ? value : 0;
                unreadBadge.SetActive(unread > 0);

                if (unread > 99) {
                    unreadCounter.text = "99+";
                } else {
                    unreadCounter.text = unread.ToString();
                }
            }
        }

        void Awake() {
            SetBadgeOnStart();
        }

        public void Reset() {
            autoScroll = true;
            lastScrollPos = 0f;
            waitMessagesScroll = 3;
        }

        void Update() {
            if (IsOpen()) {
                Carousel.BlockAxisAtCurrentTick();
            }
        }

        public void LateUpdate() {
            ScrollWaitUpdate();
            UpdateCaretAndSize();
            CheckInput();
        }

        public void OnPanelShow(MainScreenComponent.MainScreens screen) {
            switch (screen) {
                case MainScreenComponent.MainScreens.MatchLobby:
                case MainScreenComponent.MainScreens.MatchSearching:
                case MainScreenComponent.MainScreens.Cards:
                case MainScreenComponent.MainScreens.StarterPack:
                case MainScreenComponent.MainScreens.TankRent:
                    Hide();
                    break;

                default:
                    ForceMinimize();
                    break;
            }
        }

        public bool IsHidden() => hidden;

        public bool IsOpen() => maximazedCanvasGroup.interactable;

        public void Minimize(bool memory = false) {
            if (!hidden) {
                ForceMinimize();
            }
        }

        public void ForceMinimize() {
            hidden = false;
            maximizeButton.SetActive(true);
            minimizeButton.SetActive(false);
            maximazedCanvasGroup.alpha = 0f;
            maximazedCanvasGroup.blocksRaycasts = false;
            maximazedCanvasGroup.interactable = false;
            minimazedCanvasGroup.alpha = 1f;
            GetComponent<Animator>().SetBool("show", false);
        }

        public void Maximaze() {
            hidden = false;
            minimazedCanvasGroup.alpha = 0f;
            minimazedCanvasGroup.blocksRaycasts = false;
            minimazedCanvasGroup.interactable = false;
            maximazedCanvasGroup.alpha = 1f;
            maximazedCanvasGroup.blocksRaycasts = true;
            maximazedCanvasGroup.interactable = true;
            maximizeButton.SetActive(false);
            minimizeButton.SetActive(true);
            ScrollToEnd();
            GetComponent<Animator>().SetBool("show", true);
            EngineService.Engine.ScheduleEvent(new ChatMaximazedEvent(), new EntityStub());
        }

        public void Hide() {
            hidden = true;
            maximizeButton.SetActive(false);
            minimizeButton.SetActive(false);
            minimazedCanvasGroup.alpha = 0f;
            minimazedCanvasGroup.blocksRaycasts = false;
            minimazedCanvasGroup.interactable = false;
            maximazedCanvasGroup.alpha = 0f;
            maximazedCanvasGroup.blocksRaycasts = false;
            maximazedCanvasGroup.interactable = false;
            GetComponent<Animator>().SetBool("show", false);
        }

        public void Show() {
            if (MainScreenComponent.Instance.isActiveAndEnabled && !TutorialCanvas.Instance.IsShow) {
                OnPanelShow(MainScreenComponent.Instance.GetCurrentPanel());
            }
        }

        public void OnHide() { }

        public void OnShow() { }

        void CheckInput() {
            if (!hidden && IsOpen() && Input.GetKeyDown(KeyCode.Escape)) {
                Minimize();
            }

            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) {
                if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) {
                    lastMessage.gameObject.SetActive(false);
                } else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) {
                    lastMessage.gameObject.SetActive(true);
                }
            }
        }

        public void OnInputFieldSubmit(string text) {
            if (!Input.GetKey(KeyCode.Return)) {
                return;
            }

            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) {
                if (forceNewLine < MAX_NEW_LINES && inputField.text.Length < MAX_MESSAGE_LENGTH) {
                    inputField.text += "\n";
                    forceNewLine++;
                    caretCrutch = 3;
                } else {
                    caretCrutch = 2;
                }
            } else {
                Send();
            }
        }

        public void Send() {
            SendMessage(inputField.text);
            inputField.ActivateInputField();
            inputField.text = string.Empty;
            ResetBottomSize();
            forceNewLine = 0;
            autoScroll = true;
        }

        public void ValidateInput(string text) {
            caretCrutch = 1;
        }

        public void SetInputInteractable(bool interactable) {
            sendButton.SetActive(interactable);
            inputField.gameObject.SetActive(interactable);
            inputFieldInactivePlaceholder.SetActive(!interactable);
        }

        void UpdateCaretAndSize() {
            if (caretCrutch > 0) {
                caretCrutch--;

                if (caretCrutch == 1) {
                    inputField.ActivateInputField();
                    inputField.MoveTextEnd(false);
                }

                if (caretCrutch == 0) {
                    bottom.sizeDelta = new Vector2(bottom.sizeDelta.x, Math.Max(baseBottomHeight, inputField.textComponent.preferredHeight + 36f));
                }
            }
        }

        void ResetBottomSize() {
            bottom.sizeDelta = new Vector2(bottom.sizeDelta.x, baseBottomHeight);
        }

        void SetBadgeOnStart() {
            unreadBadge.SetActive(unread > 0);
        }

        public void AddUIMessage(ChatMessage message) {
            if (messages.Count == 50) {
                DestroyImmediate(messages[0].gameObject);
                messages.RemoveAt(0);

                if (!messages[0].First) {
                    ChatMessage message2 = messages[0].Message;
                    DestroyImmediate(messages[0].gameObject);
                    messages[0] = CreateMessage(message2, true);
                    messages[0].transform.SetAsFirstSibling();
                }
            }

            bool first = messages.Count == 0 || message.System || messages.Last().Message.Author != message.Author || messages.Last().Message.ChatId != message.ChatId;
            messages.Add(CreateMessage(message, first));
        }

        MessageObject CreateMessage(ChatMessage message, bool first) {
            MessageObject original = secondOpponentMessagePrefab;

            if (message.Self) {
                original = !first ? secondSelfMessagePrefab : firstSelfMessagePrefab;
            } else if (first) {
                original = firstOpponentMessagePrefab;
            }

            ScrollToEnd();
            MessageObject messageObject = Instantiate(original);
            messageObject.Set(message, GetColorByChatType);
            messageObject.transform.SetParent(messagesRoot, false);
            return messageObject;
        }

        public void SetLastMessage(ChatMessage message) {
            lastMessage.text = message.GetEllipsis(GetColorByChatType);
        }

        public new void SendMessage(string message) {
            EngineService.Engine.ScheduleEvent(new SendMessageEvent(message), new EntityStub());
        }

        public void OnScrollRectChanged(Vector2 pos) {
            if (autoScroll) {
                if (waitMessagesScroll == 0 && pos.y > 0.1 && lastScrollPos != pos.y && pos.y <= 0.99) {
                    autoScroll = false;
                }
            } else if (pos.y <= 0.1) {
                autoScroll = true;
            }

            lastScrollPos = pos.y;
        }

        void ScrollToEnd() {
            if (autoScroll) {
                waitMessagesScroll = 3;
            }
        }

        void ScrollWaitUpdate() {
            if (waitMessagesScroll > 0) {
                waitMessagesScroll--;

                if (waitMessagesScroll == 0) {
                    messagesRoot.GetComponent<CanvasGroup>().alpha = 1f;
                    messagesScroll.normalizedPosition = Vector2.zero;
                }
            }
        }

        public ChatSectionType GetSection(ChatType type) {
            switch (type) {
                case ChatType.PERSONAL:
                    return ChatSectionType.Personal;

                case ChatType.SQUAD:
                case ChatType.CUSTOMGROUP:
                    return ChatSectionType.Group;

                default:
                    return ChatSectionType.Common;
            }
        }

        Color GetColorByChatType(ChatType chatType) {
            return uiSettings.Find(x => x.Type == chatType).Color;
        }

        public EntityBehaviour CreateChatChannel(ChatType type) {
            ChatSectionType sectionType = GetSection(type);
            Transform parent = channelRoots.Find(x => x.ChatSection == sectionType).Parent;
            ChatUISettings chatUISettings = uiSettings.Find(x => x.Type == type);
            EntityBehaviour entityBehaviour = Instantiate(chatUISettings.ChatTabPrefab);
            entityBehaviour.transform.SetParent(parent, false);
            ChatChannelUIComponent component = entityBehaviour.gameObject.GetComponent<ChatChannelUIComponent>();
            component.Tab = entityBehaviour.gameObject;

            if (type != ChatType.PERSONAL) {
                component.SetIcon(chatUISettings.IconName);
            }

            component.Name = chatUISettings.DefaultName.Value;
            return entityBehaviour;
        }

        public void ChangeName(GameObject tab, ChatType type, string name) {
            ChatUISettings chatUISettings = uiSettings.Find(x => x.Type == type);
            ChatChannelUIComponent component = tab.GetComponent<ChatChannelUIComponent>();
            activeNotPersonalChannelIcon.SpriteUid = chatUISettings.IconName;

            if (string.IsNullOrEmpty(name)) {
                name = chatUISettings.DefaultName.Value;
            }

            string text = string.Format("<color=#{0}>{1}</color>", chatUISettings.Color.ToHexString(), name);
            component.Name = text;
        }

        public void SetHeader(string spriteUid, string header, bool personal = false) {
            activePersonalChannelIcon.SpriteUid = spriteUid;
            activeNotPersonalChannelIcon.SpriteUid = spriteUid;
            activeChannelName.text = header;
            chatIcon.SetActive(!personal);
            userIcon.SetActive(personal);
        }

        public void SelectChannel(ChatType type, List<ChatMessage> messages) {
            messagesRoot.GetComponent<CanvasGroup>().alpha = 0f;
            Reset();
            inputField.text = string.Empty;
            forceNewLine = 0;
            ResetBottomSize();

            foreach (MessageObject message in this.messages) {
                DestroyImmediate(message.gameObject);
            }

            this.messages = new List<MessageObject>();
            int num = messages.Count - 50;

            if (num < 0) {
                num = 0;
            }

            for (int i = num; i < messages.Count; i++) {
                AddUIMessage(messages[i]);
            }
        }

        [Serializable]
        public class ChatUISettings {
            [SerializeField] ChatType type;

            [SerializeField] Color color;

            [SerializeField] string iconName;

            [SerializeField] LocalizedField defaultName;

            [SerializeField] EntityBehaviour chatTabPrefab;

            public ChatType Type => type;

            public Color Color => color;

            public string IconName => iconName;

            public LocalizedField DefaultName => defaultName;

            public EntityBehaviour ChatTabPrefab => chatTabPrefab;
        }

        [Serializable]
        public class ChannelRoot {
            [SerializeField] Transform parent;

            [SerializeField] ChatSectionType chatSection;

            public Transform Parent => parent;

            public ChatSectionType ChatSection => chatSection;
        }
    }
}