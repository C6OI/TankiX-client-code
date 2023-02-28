using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class StaticBonusUI : LocalizedControl {
        [SerializeField] ImageSkin image;

        [SerializeField] Text valueText;

        [SerializeField] Text sufixText;

        int value;

        public string Icon {
            get => image.SpriteUid;
            set => image.SpriteUid = value;
        }

        public int Value {
            get => value;
            set {
                this.value = value;
                valueText.text = string.Format(BonusText, value);
            }
        }

        public string BonusText { get; set; }

        public string DamageText { get; set; }

        public string ArmorText { get; set; }
    }
}