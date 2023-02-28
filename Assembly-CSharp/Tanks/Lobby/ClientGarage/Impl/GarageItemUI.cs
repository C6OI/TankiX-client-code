using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler {
        [SerializeField] ImageSkin preview;

        [SerializeField] ImageSkin shadow;

        Carousel carousel;

        bool state;

        public RectTransform RectTransform => GetComponent<RectTransform>();

        public GarageItem Item { get; private set; }

        void OnEnable() {
            GetComponent<Animator>().SetBool("Selected", state);
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (!TutorialCanvas.Instance.IsShow) {
                carousel.Select(Item);
            }
        }

        public void Init(GarageItem item, Carousel carousel) {
            Item = item;
            preview.SpriteUid = item.Preview;
            shadow.SpriteUid = item.Preview;
            state = false;
            this.carousel = carousel;
        }

        public void Select() {
            state = true;
            GetComponent<Animator>().SetBool("Selected", state);
            this.SendEvent<ListItemSelectedEvent>(Item.UserItem == null ? Item.MarketItem : Item.UserItem);
        }

        public void Deselect() {
            state = false;

            if (gameObject.activeInHierarchy) {
                GetComponent<Animator>().SetBool("Selected", state);
            }
        }
    }
}