using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class ImageSkin : MonoBehaviour, SpriteRequest {
        [SerializeField] string structureUid;

        [SerializeField] string spriteUid;

        public Image.Type type;

        Image image;

        BaseElementScaleController requestHolder;

        bool requestRegistered;

        Image Image {
            get {
                if (image == null && this != null && gameObject != null) {
                    image = GetComponent<Image>();
                }

                return image;
            }
        }

        public string SpriteUid {
            get => spriteUid;
            set {
                if (spriteUid != value) {
                    CancelRequest();
                    spriteUid = value;
                    RegisterRequest();
                }
            }
        }

        protected void Reset() {
            ResetSkin();
        }

        protected virtual void OnEnable() {
            RegisterRequest();
        }

        protected void OnDestroy() {
            CancelRequest();
            Cancel();
        }

        protected void OnBeforeTransformParentChanged() {
            CancelRequest();
        }

        protected void OnTransformParentChanged() {
            RegisterRequest();
        }

        public string Uid => SpriteUid;

        public void Resolve(Sprite sprite) {
            if (Image == null) {
                CancelRequest();
            } else if (Application.isPlaying) {
                Image.sprite = sprite;
                Image.enabled = true;
            } else {
                Image.overrideSprite = sprite;
            }
        }

        public void Cancel() {
            if (Image != null) {
                Image.sprite = null;
                Image.overrideSprite = null;
            }
        }

        void RegisterRequest() {
            if (!string.IsNullOrEmpty(spriteUid)) {
                if (Application.isPlaying && Image.overrideSprite == null) {
                    Image.enabled = false;
                }

                BaseElementScaleControllerProvider componentInParent = GetComponentInParent<BaseElementScaleControllerProvider>();

                if (componentInParent != null && componentInParent.BaseElementScaleController != null) {
                    requestHolder = componentInParent.BaseElementScaleController;
                    componentInParent.BaseElementScaleController.RegisterSpriteRequest(this);
                    requestRegistered = true;
                }
            } else if (Application.isPlaying) {
                Image.sprite = null;
                Image.enabled = false;
            }
        }

        void CancelRequest() {
            if (requestRegistered && requestHolder != null) {
                requestHolder.UnregisterSpriteRequest(this);
                requestRegistered = false;
            }
        }

        public void ResetSkin() {
            Image.sprite = null;
        }
    }
}