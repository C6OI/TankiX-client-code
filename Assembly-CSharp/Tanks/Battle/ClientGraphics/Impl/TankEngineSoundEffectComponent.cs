using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankEngineSoundEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject enginePrefab;

        public GameObject EnginePrefab {
            get => enginePrefab;
            set => enginePrefab = value;
        }

        public HullSoundEngineController SoundEngineController { get; set; }
    }
}