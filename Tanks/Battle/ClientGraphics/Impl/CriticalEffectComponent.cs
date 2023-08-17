using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CriticalEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject effectAsset;

        public GameObject EffectAsset {
            get => effectAsset;
            set => effectAsset = value;
        }
    }
}