using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RailgunTrailComponent : MonoBehaviour, Component {
        [SerializeField] GameObject prefab;

        [SerializeField] GameObject tipPrefab;

        public GameObject Prefab => prefab;

        public GameObject TipPrefab => tipPrefab;
    }
}