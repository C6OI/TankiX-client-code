using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LoginRewardItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] TextMeshProUGUI dayLabel;

        [SerializeField] LoginRewardItemTooltip tooltip;

        [SerializeField] ImageSkin imagePrefab;

        [SerializeField] Transform imagesContainer;

        [SerializeField] LoginRewardProgressBar progressBar;

        [SerializeField] LocalizedField dayLocalizedField;

        public int Day {
            set => dayLabel.text = value + " " + dayLocalizedField.Value.ToUpper();
        }

        public LoginRewardProgressBar.FillType fillType {
            set => progressBar.Fill(value);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            GetComponent<Animator>().SetBool("hover", true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            GetComponent<Animator>().SetBool("hover", false);
        }

        public void SetupLines(bool itemIsFirst, bool itemIsLast) {
            if (itemIsFirst) {
                progressBar.LeftLine.SetActive(false);
            }

            if (itemIsLast) {
                progressBar.RightLine.SetActive(false);
            }
        }

        public void AddItem(string imageUID, string name) {
            ImageSkin imageSkin = Instantiate(imagePrefab, imagesContainer);
            imageSkin.SpriteUid = imageUID;
            imageSkin.gameObject.SetActive(true);
            string text = (!string.IsNullOrEmpty(tooltip.Text) ? "\n" : string.Empty) + name;
            tooltip.Text += text;
        }
    }
}