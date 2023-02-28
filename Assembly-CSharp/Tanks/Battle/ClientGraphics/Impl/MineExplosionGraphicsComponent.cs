using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MineExplosionGraphicsComponent : MonoBehaviour, Component {
        [SerializeField] GameObject effectPrefab;

        [SerializeField] float explosionLifeTime = 2f;

        [SerializeField] Vector3 origin = Vector3.up;

        public GameObject EffectPrefab {
            get => effectPrefab;
            set => effectPrefab = value;
        }

        public float ExplosionLifeTime => explosionLifeTime;

        public Vector3 Origin => origin;
    }
}