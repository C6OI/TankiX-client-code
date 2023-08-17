using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanMuzzleFlashComponent : MonoBehaviour, Component {
        [SerializeField] GameObject effectPrefab;

        public GameObject EffectPrefab {
            get => effectPrefab;
            set => effectPrefab = value;
        }

        public ParticleSystem EffectInstance { get; set; }

        public Light LightInstance { get; set; }
    }
}