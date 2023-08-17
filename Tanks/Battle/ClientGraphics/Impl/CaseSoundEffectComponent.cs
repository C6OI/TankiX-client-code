using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class CaseSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject caseSoundAsset;

        public GameObject CaseSoundAsset {
            get => caseSoundAsset;
            set => caseSoundAsset = value;
        }

        public AudioSource Source { get; set; }
    }
}