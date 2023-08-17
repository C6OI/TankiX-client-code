using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RailgunChargingEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject prefab;

        public GameObject Prefab {
            get => prefab;
            set => prefab = value;
        }
    }
}