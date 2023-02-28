using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class TopPanelComponent : MonoBehaviour, Component {
        public GameObject backButton;

        public Image background;

        public Animator screenHeader;

        [SerializeField] Text newHeaderText;

        [SerializeField] Text currentHeaderText;

        bool hasHeader;

        public string NewHeader {
            get => newHeaderText.text;
            set => newHeaderText.text = value.ToUpper();
        }

        public string CurrentHeader {
            get => currentHeaderText.text;
            set => currentHeaderText.text = value.ToUpper();
        }

        public bool HasHeader => hasHeader && screenHeader.isInitialized;

        public void SetHeaderText(string headerText) {
            if (hasHeader) {
                NewHeader = headerText;

                if (!screenHeader.isInitialized || !screenHeader.GetCurrentAnimatorStateInfo(0).IsName("Default")) {
                    CurrentHeader = headerText;
                }
            } else {
                SetHeaderTextImmediately(headerText);
                hasHeader = true;
            }
        }

        public void SetHeaderTextImmediately(string headerText) {
            NewHeader = headerText;
            CurrentHeader = headerText;
        }
    }
}