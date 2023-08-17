using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineExplosionGraphicsComponent : MonoBehaviour, Component {
        [SerializeField] GameObject effectPrefab;

        public GameObject EffectPrefab {
            get => effectPrefab;
            set => effectPrefab = value;
        }
    }
}