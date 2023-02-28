using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class RandomImageSelector : MonoBehaviour {
        public Image TargetImage;

        public Sprite[] AvailableSprites;

        void OnEnable() {
            ChangeImage();
        }

        public void ChangeImage() {
            if (AvailableSprites.Length > 0) {
                int num = Random.Range(0, AvailableSprites.Length);
                TargetImage.sprite = AvailableSprites[num];
            }
        }
    }
}