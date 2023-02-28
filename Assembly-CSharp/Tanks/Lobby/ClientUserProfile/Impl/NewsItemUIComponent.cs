using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NewsItemUIComponent : UIBehaviour, Component {
        [SerializeField] Text headerText;

        [SerializeField] Text dateText;

        [SerializeField] NewsImageContainerComponent imageContainer;

        [SerializeField] RectTransform centralIconTransform;

        [SerializeField] RectTransform saleIconTransform;

        [SerializeField] Text saleIconText;

        [SerializeField] GameObject border;

        public bool noSwap;

        string tooltip = string.Empty;

        public string HeaderText {
            get => headerText.text;
            set => headerText.text = value;
        }

        public string DateText {
            get => dateText.text;
            set => dateText.text = value;
        }

        public bool SaleIconVisible {
            get => saleIconTransform.gameObject.activeSelf;
            set => saleIconTransform.gameObject.SetActive(value);
        }

        public string SaleIconText {
            get => saleIconText.text;
            set => saleIconText.text = value;
        }

        public string Tooltip {
            get => tooltip;
            set {
                tooltip = value;
                TooltipShowBehaviour component = GetComponent<TooltipShowBehaviour>();

                if (component != null) {
                    component.TipText = tooltip;
                }
            }
        }

        public NewsImageContainerComponent ImageContainer => imageContainer;

        public GameObject Border => border;

        public void SetCentralIcon(Texture2D texture) {
            RawImage rawImage = centralIconTransform.gameObject.AddComponent<RawImage>();
            rawImage.texture = texture;
            rawImage.SetNativeSize();
        }
    }
}