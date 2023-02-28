using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BasePointerComponent : CTFPointerComponent {
        public Sprite FlagOnBaseImage;

        public Sprite FlagStolenImage;

        public LocalizedField BaseText;

        public Image Image;

        public void OnEnable() {
            text.text = BaseText.Value;
        }

        public void SetFlagStolenState() {
            Image.sprite = FlagStolenImage;
        }

        public void SetFlagAtHomeState() {
            Image.sprite = FlagOnBaseImage;
        }
    }
}