using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatUIComponent : MonoBehaviour, Component {
        [SerializeField] GameObject inputPanel;

        [SerializeField] Image bottomLineImage;

        [SerializeField] TextMeshProUGUI inputHintText;

        [SerializeField] Color commonNicknameColor;

        [SerializeField] Color commonTextColor;

        [SerializeField] Color redTeamNicknameColor;

        [SerializeField] Color redTeamTextColor;

        [SerializeField] Color blueTeamNicknameColor;

        [SerializeField] Color blueTeamTextColor;

        [SerializeField] PaletteColorField systemMessageColor;

        [SerializeField] GameObject messagesContainer;

        [SerializeField] LayoutElement scrollViewLayoutElement;

        [SerializeField] RectTransform scrollViewRectTransform;

        [SerializeField] RectTransform inputFieldRectTransform;

        [SerializeField] GameObject scrollBarHandle;

        [SerializeField] int maxVisibleMessagesInActiveState = 6;

        [SerializeField] int maxVisibleMessagesInPassiveState = 3;

        public string SavedInputMessage { get; set; } = string.Empty;

        public Color BottomLineColor {
            get => bottomLineImage.color;
            set {
                if (!(bottomLineImage == null)) {
                    value.a = 0.4f;
                    bottomLineImage.color = value;
                }
            }
        }

        public string InputHintText {
            get => inputHintText.text;
            set => inputHintText.text = value;
        }

        public Color InputHintColor {
            get => inputHintText.color;
            set => inputHintText.color = value;
        }

        public Color InputTextColor {
            get => inputPanel.GetComponentInChildren<InputField>().textComponent.color;
            set => inputPanel.GetComponentInChildren<InputField>().textComponent.color = value;
        }

        public bool InputPanelActivity {
            get => inputPanel.activeSelf;
            set => inputPanel.SetActive(value);
        }

        public GameObject MessagesContainer => messagesContainer;

        public float ScrollViewHeight {
            get => scrollViewLayoutElement.preferredHeight;
            set => scrollViewLayoutElement.preferredHeight = value;
        }

        public float ScrollViewPosY => scrollViewRectTransform.anchoredPosition.y;

        public bool ScrollBarActivity {
            get => scrollBarHandle.activeSelf;
            set => scrollBarHandle.SetActive(value);
        }

        public Color CommonNicknameColor => commonNicknameColor;

        public Color CommonTextColor => commonTextColor;

        public Color RedTeamNicknameColor => redTeamNicknameColor;

        public Color RedTeamTextColor => redTeamTextColor;

        public Color BlueTeamNicknameColor => blueTeamNicknameColor;

        public Color BlueTeamTextColor => blueTeamTextColor;

        public Color SystemMessageColor => systemMessageColor;

        public int MaxVisibleMessagesInActiveState => maxVisibleMessagesInActiveState;

        public int MaxVisibleMessagesInPassiveState => maxVisibleMessagesInPassiveState;

        public void SetHintSize(bool teamMode) {
            inputHintText.rectTransform.sizeDelta = new Vector2(!teamMode ? 56f : 86f, inputHintText.rectTransform.sizeDelta.y);
            inputFieldRectTransform.sizeDelta = new Vector2(!teamMode ? 340f : 310f, inputHintText.rectTransform.sizeDelta.y);
        }
    }
}