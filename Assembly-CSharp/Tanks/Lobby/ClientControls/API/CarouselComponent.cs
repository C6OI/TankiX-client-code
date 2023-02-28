using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class CarouselComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] CarouselButtonComponent backButton;

        [SerializeField] CarouselButtonComponent frontButton;

        [SerializeField] [HideInInspector] int templateIdLow;

        [SerializeField] [HideInInspector] int templateIdHigh;

        public long ItemTemplateId {
            get => (long)templateIdHigh << 32 | (uint)templateIdLow;
            set {
                templateIdLow = (int)(value & 0xFFFFFFFFu);
                templateIdHigh = (int)(value >> 32);
            }
        }

        public TextMeshProUGUI Text => text;

        public CarouselButtonComponent BackButton => backButton;

        public CarouselButtonComponent FrontButton => frontButton;
    }
}