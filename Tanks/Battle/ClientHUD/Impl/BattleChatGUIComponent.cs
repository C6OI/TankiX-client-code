using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatGUIComponent : MonoBehaviour, Component {
        [SerializeField] GameObject inputPanel;

        [SerializeField] Image inputFieldBackgroundImage;

        [SerializeField] Text inputHintText;

        [SerializeField] Color commonNicknameColor;

        [SerializeField] Color commonTextColor;

        [SerializeField] Color redTeamNicknameColor;

        [SerializeField] Color redTeamTextColor;

        [SerializeField] Color blueTeamNicknameColor;

        [SerializeField] Color blueTeamTextColor;

        [SerializeField] GameObject messagesContainer;

        [SerializeField] LayoutElement scrollViewLayoutElement;

        [SerializeField] RectTransform scrollViewRectTransform;

        [SerializeField] GameObject scrollBarHandle;

        [SerializeField] int maxVisibleMessagesInActiveState = 6;

        [SerializeField] int maxVisibleMessagesInPassiveState = 3;

        public string SavedInputMessage { get; set; } = string.Empty;

        public Color InputFieldColor {
            get => inputFieldBackgroundImage.color;
            set => inputFieldBackgroundImage.color = value;
        }

        public string InputHintText {
            get => inputHintText.text;
            set => inputHintText.text = value;
        }

        public Color InputHintColor {
            get => inputHintText.color;
            set => inputHintText.color = value;
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

        public int MaxVisibleMessagesInActiveState => maxVisibleMessagesInActiveState;

        public int MaxVisibleMessagesInPassiveState => maxVisibleMessagesInPassiveState;
    }
}