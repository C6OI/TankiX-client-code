using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RailgunShotEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject asset;

        public GameObject Asset {
            get => asset;
            set => asset = value;
        }

        public AudioSource AudioSurce { get; set; }
    }
}