using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleItemContentComponent : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler,
        Component {
        const string GEAR_SPEED_MULTIPLIER = "GearTransparencySign";

        const string GEAR_TRANSPARENCY_STATE_NAME = "GearTransparency";

        const string SHOW_PREVIEW_NAME = "ShowPreview";

        const string HIGHLIGHTED_NAME = "Highlighted";

        const string NORMAL_NAME = "Normal";

        const float GEAR_REVERSE_MULTIPLIER = -0.66f;

        const int GEAR_TRANSPARENCY_STATE_LAYER_INDEX = 1;

        [SerializeField] Text battleModeTextField;

        [SerializeField] Text debugInfoTextField;

        [SerializeField] Text timeTextField;

        [SerializeField] Text userCountTextField;

        [SerializeField] Text scoreTextField;

        [SerializeField] RawImage previewImage;

        [SerializeField] Animator animator;

        [SerializeField] Material grayscaleMaterial;

        [SerializeField] RectTransform timeTransform;

        [SerializeField] RectTransform scoreTransform;

        [SerializeField] RectTransform scoreTankIcon;

        [SerializeField] RectTransform scoreFlagIcon;

        bool entranceLocked;

        int gearSpeedMultiplierID;

        int gearTransparencyStateID;

        int highlightedID;

        int normalID;

        int showGearID;

        int showPreviewID;

        public bool EntranceLocked {
            get => entranceLocked;
            set {
                entranceLocked = value;

                if (value) {
                    previewImage.material = grayscaleMaterial;
                    SendMessageUpwards("OnItemDisabled", SendMessageOptions.RequireReceiver);
                } else {
                    previewImage.material = null;
                    SendMessageUpwards("OnItemEnabled", SendMessageOptions.RequireReceiver);
                }
            }
        }

        new void Awake() {
            showPreviewID = Animator.StringToHash("ShowPreview");
            normalID = Animator.StringToHash("Normal");
            highlightedID = Animator.StringToHash("Highlighted");
            gearSpeedMultiplierID = Animator.StringToHash("GearTransparencySign");
            gearTransparencyStateID = Animator.StringToHash("GearTransparency");
        }

        protected override void OnRectTransformDimensionsChange() {
            if (!(previewImage.texture == null)) {
                Rect rect = new(0f, 0f, previewImage.texture.width, previewImage.texture.height);
                float num = rect.width / rect.height;
                RectTransform rectTransform = (RectTransform)transform;
                RectTransform rectTransform2 = (RectTransform)previewImage.transform;
                float num2 = rectTransform.rect.width / rectTransform.rect.height;

                rectTransform2.sizeDelta = !(num2 < num)
                                               ? new Vector2(rectTransform.rect.width, rectTransform.rect.width / num)
                                               : new Vector2(num * rectTransform.rect.height, rectTransform.rect.height);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => animator.SetTrigger(highlightedID);

        public void OnPointerExit(PointerEventData eventData) => animator.SetTrigger(normalID);

        public void SetModeField(string text) => battleModeTextField.text = text;

        public void SetDebugField(string text) => debugInfoTextField.text = text;

        public void SetTimeField(string text) => timeTextField.text = text;

        public void SetUserCountField(string text) => userCountTextField.text = text;

        public void SetScoreField(string text) => scoreTextField.text = text;

        public void SetPreview(Texture2D image) {
            previewImage.texture = image;
            animator.SetTrigger(showPreviewID);
            animator.SetFloat(gearSpeedMultiplierID, -0.66f);
            float normalizedTime = Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(1).normalizedTime);
            animator.Play(gearTransparencyStateID, 1, normalizedTime);
            OnRectTransformDimensionsChange();
        }

        public void HideTime() {
            scoreTransform.anchoredPosition = timeTransform.anchoredPosition;
            timeTransform.gameObject.SetActive(false);
        }

        public void HideScore() => scoreTransform.gameObject.SetActive(false);

        public void SetFlagAsScoreIcon() {
            scoreTankIcon.gameObject.SetActive(false);
            scoreFlagIcon.gameObject.SetActive(true);
        }
    }
}