using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class ShaftHitSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject asset;

        [SerializeField] float duration;

        public GameObject Asset {
            get => asset;
            set => asset = value;
        }

        public float Duration {
            get => duration;
            set => duration = value;
        }
    }
}