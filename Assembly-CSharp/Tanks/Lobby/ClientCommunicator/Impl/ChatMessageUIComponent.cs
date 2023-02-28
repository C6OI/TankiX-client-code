using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatMessageUIComponent : MonoBehaviour, Component {
        [SerializeField] TextMeshProUGUI firstPartText;

        [SerializeField] TextMeshProUGUI secondPartText;

        public bool showed;

        public string FirstPartText {
            get => firstPartText.text;
            set {
                firstPartText.text = value;
                Canvas.ForceUpdateCanvases();
                firstPartText.GetComponent<LayoutElement>().minWidth = firstPartText.rectTransform.rect.width;
            }
        }

        public Color FirstPartTextColor {
            set => firstPartText.color = value;
        }

        public string SecondPartText {
            get => secondPartText.text;
            set => secondPartText.text = value;
        }

        public Color SecondPartTextColor {
            set => secondPartText.color = value;
        }
    }
}