using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeStars : MonoBehaviour {
        [SerializeField] Image[] stars;

        public void SetPower(float power) {
            if (power < 0f) {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            Image[] array = stars;

            foreach (Image image in array) {
                float num = 1f / stars.Length;
                float num2 = Mathf.Min(num, power);
                power -= num2;
                image.fillAmount = num2 / num;
            }
        }
    }
}