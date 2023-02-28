using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DiscreteWeaponShotEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject asset;

        public GameObject Asset {
            get => asset;
            set => asset = value;
        }

        public AudioSource[] AudioSources { get; set; }
    }
}