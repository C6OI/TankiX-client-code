using UnityEngine;

namespace Lobby.ClientControls.API {
    public class ListItem : MonoBehaviour {
        public const string DISABLE_MESSAGE = "OnItemDisabled";

        public const string ENABLE_MESSAGE = "OnItemEnabled";

        static readonly string SELECTED_STATE = "Selected";

        static readonly string ENABLED_STATE = "Enabled";

        [SerializeField] RectTransform content;

        ListItemContent cachedContent;

        object data;

        public object Data {
            get => data;
            set {
                data = value;

                if (cachedContent != null) {
                    cachedContent.SetDataProvider(data);
                }
            }
        }

        void OnItemDisabled() => GetComponent<Animator>().SetBool(ENABLED_STATE, false);

        void OnItemEnabled() => GetComponent<Animator>().SetBool(ENABLED_STATE, true);

        public void PlaySelectionAnimation() => GetComponent<Animator>().SetBool(SELECTED_STATE, true);

        public void PlayDeselectionAnimation() => GetComponent<Animator>().SetBool(SELECTED_STATE, false);

        public void SetContent(RectTransform content) {
            content.SetParent(this.content, false);
            content.gameObject.SetActive(false);
            content.gameObject.SetActive(true);
            cachedContent = content.GetComponent<ListItemContent>();
        }

        public void Select() {
            SendMessageUpwards("OnItemSelect", this);
            PlaySelectionAnimation();

            if (cachedContent != null) {
                cachedContent.Select();
            }
        }

        public RectTransform GetContent() {
            if (content.childCount == 1) {
                return (RectTransform)content.GetChild(0);
            }

            return null;
        }
    }
}