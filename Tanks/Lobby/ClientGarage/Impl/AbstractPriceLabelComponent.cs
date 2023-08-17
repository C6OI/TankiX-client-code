using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public abstract class AbstractPriceLabelComponent : BehaviourComponent {
        public Color shortageColor = Color.red;

        public Color DefualtColor { get; set; }

        public long Price { get; set; }

        public Text Text => GetComponent<Text>();

        public Color Color {
            get => Text.color;
            set {
                if (Text.color == DefualtColor) {
                    Text.color = value;
                }

                DefualtColor = value;
            }
        }

        void Awake() => DefualtColor = Text.color;
    }
}