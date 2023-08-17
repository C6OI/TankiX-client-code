using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics {
    public class TankDeathExplosionPrefabsComponent : MonoBehaviour, Component {
        [SerializeField] GameObject soundPrefab;

        [SerializeField] ParticleSystem explosionPrefab;

        [SerializeField] ParticleSystem firePrefab;

        public GameObject SoundPrefab => soundPrefab;

        public ParticleSystem ExplosionPrefab => explosionPrefab;

        public ParticleSystem FirePrefab => firePrefab;
    }
}