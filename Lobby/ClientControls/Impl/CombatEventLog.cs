using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.Impl {
    public class CombatEventLog : MonoBehaviour, UILog {
        const int INVALID_RANK = -1;

        static readonly UserElementDescription PARSE_FAILED_ELEMENT = new(string.Empty, -1, Color.black);

        [SerializeField] CombatEventLogMessageContainter messagesContainer;

        readonly Queue<CombatEventLogMessage> messagesToDelete = new();

        readonly Regex userPattern = new("{[0-9]{1,2}:[0-9a-fA-F]*:[^}]*}");

        int currentlyVisibleMessages;

        float scrollHeight;

        int spaceBetweenMessages;

        void Awake() => spaceBetweenMessages = (int)messagesContainer.VerticalLayout.spacing;

        void Update() {
            DestroyMessageIfHidden();
            MoveMessage();
        }

        public void AddMessage(string messageText) => StartCoroutine(AddMessageCoroutine(messageText));

        public void Clear() {
            currentlyVisibleMessages = 0;
            scrollHeight = 0f;
            messagesContainer.Clear();
        }

        void DestroyMessageIfHidden() {
            if (messagesToDelete.Count > 0) {
                LayoutElement layoutElement = messagesToDelete.Peek().LayoutElement;
                float num = messagesContainer.AnchoredPosition.y - layoutElement.preferredHeight - spaceBetweenMessages;

                if (num >= 0f) {
                    DestroyMessage(messagesToDelete.Dequeue());
                }
            }
        }

        void MoveMessage() {
            if (messagesContainer.ChildCount > 0) {
                Vector2 anchoredPosition = messagesContainer.AnchoredPosition;

                if (anchoredPosition.y < scrollHeight) {
                    anchoredPosition.y += scrollHeight * Time.deltaTime;
                }

                if (anchoredPosition.y > scrollHeight) {
                    anchoredPosition.y = scrollHeight;
                }

                messagesContainer.AnchoredPosition = anchoredPosition;
            }
        }

        void DestroyMessage(CombatEventLogMessage message) {
            float preferredHeight = message.LayoutElement.preferredHeight;
            scrollHeight -= preferredHeight + spaceBetweenMessages;
            Vector2 anchoredPosition = messagesContainer.AnchoredPosition;
            anchoredPosition.y -= preferredHeight + spaceBetweenMessages;
            messagesContainer.AnchoredPosition = anchoredPosition;

            if (message != null) {
                messagesContainer.DestroyMessage(message);
            }

            currentlyVisibleMessages--;
        }

        void OnScrollLog(float height) => scrollHeight += height + spaceBetweenMessages;

        void OnDeleteMessage(CombatEventLogMessage message) => messagesToDelete.Enqueue(message);

        IEnumerator AddMessageCoroutine(string messageText) {
            yield return new WaitForSeconds(0.1f);

            AddMessageImmediate(messageText);
        }

        public void AddMessageImmediate(string messageText) {
            CombatEventLogMessage messageInstanceAndAttachToContainer =
                messagesContainer.GetMessageInstanceAndAttachToContainer();

            ParseAndConstructMessageLine(messageText, messageInstanceAndAttachToContainer);
            currentlyVisibleMessages++;
            int maxVisibleMessages = messagesContainer.MaxVisibleMessages;
            List<CombatEventLogMessage> visibleChildMessages = messagesContainer.VisibleChildMessages;

            for (int i = 0; i < currentlyVisibleMessages - maxVisibleMessages; i++) {
                visibleChildMessages[i].RequestDelete();
            }

            messagesContainer.AddMessage(messageInstanceAndAttachToContainer);
        }

        void ParseAndConstructMessageLine(string messageText, CombatEventLogMessage message) {
            MatchCollection matchCollection = userPattern.Matches(messageText);
            int count = matchCollection.Count;
            int num = 0;

            for (int i = 0; i < count; i++) {
                Match match = matchCollection[i];

                if (match.Index != num) {
                    AddTextElement(messageText.Substring(num, match.Index - num), message);
                }

                AddUserElement(match.Value, message);
                num = match.Index + match.Length;
            }

            AddTextElement(messageText.Substring(num, messageText.Length - num), message);
        }

        void AddTextElement(string text, CombatEventLogMessage message) {
            if (text.Length > 0) {
                CombatEventLogText textInstance = messagesContainer.GetTextInstance();
                textInstance.Text.text = text;
                message.AttachToRight(textInstance.RectTransform);
            }
        }

        void AddUserElement(string userElementString, CombatEventLogMessage message) {
            UserElementDescription userElementDescription = ParseAndValidateUserElement(userElementString);

            if (userElementDescription != PARSE_FAILED_ELEMENT) {
                CombatEventLogUser userInstance = messagesContainer.GetUserInstance();
                userInstance.RankIcon.SelectSprite(userElementDescription.RankIndex.ToString());
                userInstance.UserName.text = userElementDescription.Uid;
                userInstance.UserName.color = userElementDescription.ElementColor;
                message.AttachToRight(userInstance.RectTransform);
            } else {
                AddTextElement(userElementString, message);
            }
        }

        UserElementDescription ParseAndValidateUserElement(string userElementString) {
            string text = userElementString.Substring(1, userElementString.Length - 2);
            string[] array = text.Split(':');

            if (array.Length == 3) {
                return FinishParsing(array[0], array[1], array[2]);
            }

            return PARSE_FAILED_ELEMENT;
        }

        UserElementDescription FinishParsing(string rankStr, string colorStr, string uidStr) {
            int num = ValidateAndParseRank(rankStr);
            Color color;
            bool flag = ColorUtility.TryParseHtmlString("#" + colorStr.ToLower(), out color);

            if (num != -1 && flag) {
                return new UserElementDescription(uidStr, num, color);
            }

            return PARSE_FAILED_ELEMENT;
        }

        int ValidateAndParseRank(string rankStr) {
            try {
                int num = int.Parse(rankStr) + 1;
                int count = messagesContainer.UserPrefab.RankIcon.Count;

                if (num > 0 && num <= count) {
                    return num;
                }

                return -1;
            } catch (Exception) {
                return -1;
            }
        }

        class UserElementDescription {
            public UserElementDescription(string uid, int rankIndex, Color elementColor) {
                Uid = uid;
                RankIndex = rankIndex;
                ElementColor = elementColor;
            }

            public string Uid { get; }

            public int RankIndex { get; }

            public Color ElementColor { get; }
        }
    }
}