using System.Collections.Generic;
using Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.Impl {
    public class CombatEventLogMessageContainter : MonoBehaviour {
        [SerializeField] int maxVisibleMessages = 5;

        [SerializeField] CombatEventLogMessage messagePrefab;

        [SerializeField] CombatEventLogText textPrefab;

        [SerializeField] CombatEventLogUser userPrefab;

        [SerializeField] RectTransform rectTransform;

        [SerializeField] RectTransform rectTransformForMoving;

        [SerializeField] VerticalLayoutGroup verticalLayout;

        public List<CombatEventLogMessage> VisibleChildMessages { get; } = new();

        public int MaxVisibleMessages => maxVisibleMessages;

        public Vector2 AnchoredPosition {
            get => rectTransformForMoving.anchoredPosition;
            set => rectTransformForMoving.anchoredPosition = value;
        }

        public int ChildCount => rectTransform.childCount;

        public VerticalLayoutGroup VerticalLayout => verticalLayout;

        public CombatEventLogUser UserPrefab => userPrefab;

        public CombatEventLogMessage GetMessageInstanceAndAttachToContainer() {
            CombatEventLogMessage combatEventLogMessage = Instantiate(messagePrefab);
            combatEventLogMessage.RectTransform.SetParent(rectTransform, false);
            return combatEventLogMessage;
        }

        public CombatEventLogText GetTextInstance() => Instantiate(textPrefab);

        public CombatEventLogUser GetUserInstance() => Instantiate(userPrefab);

        public void AddMessage(CombatEventLogMessage message) {
            VisibleChildMessages.Add(message);
            message.ShowMessage();
        }

        public void DestroyMessage(CombatEventLogMessage message) {
            VisibleChildMessages.Remove(message);
            Destroy(message.gameObject);
        }

        public void Clear() {
            while (VisibleChildMessages.Count > 0) {
                DestroyMessage(VisibleChildMessages[0]);
            }

            AnchoredPosition = Vector2.zero;
        }
    }
}