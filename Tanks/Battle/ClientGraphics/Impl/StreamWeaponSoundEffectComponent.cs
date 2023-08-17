using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StreamWeaponSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject asset;

        public GameObject Asset {
            get => asset;
            set => asset = value;
        }

        public SoundController SoundController { get; set; }
    }
}