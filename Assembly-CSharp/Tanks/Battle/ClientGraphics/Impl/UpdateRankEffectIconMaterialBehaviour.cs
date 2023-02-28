using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectIconMaterialBehaviour : MonoBehaviour {
        [SerializeField] Image img;

        [SerializeField] Image sourceImage;

        public float opacity;

        Material mat;

        bool materialUpdate;

        void Awake() {
            CopyFromSkinnedImage();
            mat = Instantiate(img.material);
            img.material = mat;
            materialUpdate = true;
            img.preserveAspect = true;
        }

        void Update() {
            CopyFromSkinnedImage();

            if (materialUpdate) {
                if (opacity >= 1f) {
                    mat.SetFloat("_Opacity", 1f);
                    materialUpdate = false;
                } else {
                    mat.SetFloat("_Opacity", opacity);
                }
            }
        }

        void CopyFromSkinnedImage() {
            if (img.sprite != sourceImage.sprite) {
                img.sprite = sourceImage.sprite;
            }

            if (img.overrideSprite != sourceImage.overrideSprite) {
                img.overrideSprite = sourceImage.overrideSprite;
            }

            if (img.type != sourceImage.type) {
                img.type = sourceImage.type;
            }
        }
    }
}