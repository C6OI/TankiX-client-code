using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NewsImageContainerComponent : UIBehaviour, Component {
        [SerializeField] RectTransform imageContainer;

        [SerializeField] AspectRatioFitter imageAspectRatioFitter;

        [SerializeField] float imageAppearTime = 0.3f;

        float alphaBeforeAppear = float.NaN;

        Graphic graphic;

        float setImageTime;

        public bool FitInParent {
            set => imageAspectRatioFitter.aspectMode = !value ? AspectRatioFitter.AspectMode.EnvelopeParent : AspectRatioFitter.AspectMode.FitInParent;
        }

        public Color Color {
            get {
                if (graphic != null) {
                    return graphic.color;
                }

                return Color.black;
            }
            set {
                if (graphic != null) {
                    graphic.color = value;
                }
            }
        }

        void Update() {
            if (setImageTime > 0f) {
                if (float.IsNaN(alphaBeforeAppear)) {
                    alphaBeforeAppear = Color.a;
                }

                float time = Time.time;
                float num = Mathf.Clamp01((time - setImageTime) / imageAppearTime);
                Color color = Color;
                color.a = alphaBeforeAppear * num;
                Color = color;

                if (num == 1f) {
                    setImageTime = 0f;
                }
            }
        }

        public void SetRawImage(Texture texture) {
            setImageTime = Time.time;
            ((RawImage)(graphic = imageContainer.gameObject.AddComponent<RawImage>())).texture = texture;
            ApplyAspectRatio(texture);
        }

        public void SetImage(Sprite sprite) {
            setImageTime = Time.time;
            ((Image)(graphic = imageContainer.gameObject.AddComponent<Image>())).sprite = sprite;
            ApplyAspectRatio(sprite.texture);
        }

        public void SetImageSkin(string spriteUid, float aspectRatio) {
            setImageTime = Time.time;
            Image image = imageContainer.gameObject.AddComponent<Image>();
            graphic = image;
            ImageSkin imageSkin = imageContainer.gameObject.AddComponent<ImageSkin>();
            imageSkin.SpriteUid = spriteUid;
            ApplyAspectRatio(aspectRatio);
        }

        void ApplyAspectRatio(Texture texture) {
            if (texture.height > 0) {
                ApplyAspectRatio(texture.width / (float)texture.height);
            }
        }

        void ApplyAspectRatio(float aspectRatio) {
            imageAspectRatioFitter.aspectRatio = aspectRatio;
        }
    }
}