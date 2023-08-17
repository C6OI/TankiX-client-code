using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingColorEffectComponent : MonoBehaviour, Component {
        [SerializeField] Color redColor = new(255f, 0f, 0f);

        [SerializeField] Color blueColor = new(0f, 187f, 255f);

        public Color RedColor {
            get => redColor;
            set => redColor = value;
        }

        public Color BlueColor {
            get => blueColor;
            set => blueColor = value;
        }

        public Color ChoosenColor { get; set; }
    }
}