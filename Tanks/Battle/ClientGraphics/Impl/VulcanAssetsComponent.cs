using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanAssetsComponent : MonoBehaviour, Component {
        [SerializeField] GameObject hitPrefab;

        [SerializeField] GameObject smokePrefab;

        [SerializeField] GameObject tracerPrefab;

        public GameObject HitPrefab {
            get => hitPrefab;
            set => hitPrefab = value;
        }

        public GameObject SmokePrefab {
            get => smokePrefab;
            set => smokePrefab = value;
        }

        public GameObject TracerPrefab {
            get => tracerPrefab;
            set => tracerPrefab = value;
        }
    }
}