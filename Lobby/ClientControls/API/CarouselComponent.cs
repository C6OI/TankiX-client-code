using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    public class CarouselComponent : BehaviourComponent {
        [SerializeField] Text text;

        [SerializeField] CarouselButtonComponent backButton;

        [SerializeField] CarouselButtonComponent frontButton;

        [SerializeField] [HideInInspector] int templateIdLow;

        [HideInInspector] [SerializeField] int templateIdHigh;

        public long ItemTemplateId {
            get => (long)templateIdHigh << 32 | (uint)templateIdLow;
            set {
                templateIdLow = (int)(value & 0xFFFFFFFFu);
                templateIdHigh = (int)(value >> 32);
            }
        }

        public Text Text => text;

        public CarouselButtonComponent BackButton => backButton;

        public CarouselButtonComponent FrontButton => frontButton;
    }
}