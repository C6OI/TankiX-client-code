using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SettingsSlotUIComponent : MonoBehaviour {
        [SerializeField] ImageSkin moduleIconImageSkin;

        [SerializeField] GameObject moduleIsPresent;

        [SerializeField] GameObject whiteBack;

        [SerializeField] Image moduleIconImage;

        [SerializeField] Color activeModuleIconColor;

        [SerializeField] Color inactiveModuleIconColor;

        public void SetIcon(string udid, bool moduleActive = true) {
            whiteBack.SetActive(string.IsNullOrEmpty(udid));
            moduleIsPresent.SetActive(!string.IsNullOrEmpty(udid));
            moduleIconImageSkin.SpriteUid = udid;
            moduleIconImage.color = !moduleActive ? inactiveModuleIconColor : activeModuleIconColor;
        }
    }
}