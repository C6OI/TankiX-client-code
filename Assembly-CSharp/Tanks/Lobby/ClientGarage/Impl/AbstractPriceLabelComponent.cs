using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public abstract class AbstractPriceLabelComponent : BehaviourComponent {
        public Color shortageColor = Color.red;

        [SerializeField] GameObject oldPrice;

        [SerializeField] Text oldPriceText;

        public Color DefaultColor { get; set; }

        public long Price { get; set; }

        public long OldPrice { get; set; }

        public Text Text => GetComponent<Text>();

        public bool OldPriceVisible {
            set {
                if (oldPrice != null) {
                    oldPrice.SetActive(value);
                }
            }
        }

        public string OldPriceText {
            set {
                if (oldPriceText != null) {
                    oldPriceText.text = value;
                }
            }
        }

        public Color Color {
            get => Text.color;
            set {
                if (Text.color == DefaultColor) {
                    Text.color = value;
                }

                DefaultColor = value;
            }
        }

        void Awake() {
            DefaultColor = Text.color;
        }
    }
}