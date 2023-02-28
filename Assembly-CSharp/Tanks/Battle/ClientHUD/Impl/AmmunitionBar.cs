using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class AmmunitionBar : MonoBehaviour {
        [SerializeField] Image fillImage;

        [SerializeField] Image light;

        public float FillValue {
            set => fillImage.rectTransform.anchorMax = new Vector2(value, 1f);
        }

        public void Activate() {
            light.gameObject.SetActive(true);
            FillValue = 1f;
        }

        public void Deactivate() {
            light.gameObject.SetActive(false);
            FillValue = 0f;
        }
    }
}