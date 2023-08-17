using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanHitGraphicsEffectComponent : MonoBehaviour, Component {
        [SerializeField] ParticleSystem hitStaticPrefab;

        [SerializeField] ParticleSystem hitTargetPrefab;

        [SerializeField] float hitOffset;

        public ParticleSystem HitStatic { get; set; }

        public ParticleSystem HitTarget { get; set; }

        public Light HitStaticLight { get; set; }

        public Light HitTargetLight { get; set; }

        public float HitOffset {
            get => hitOffset;
            set => hitOffset = value;
        }

        public void Init() {
            HitStatic = Instantiate(hitStaticPrefab);
            HitTarget = Instantiate(hitTargetPrefab);
            HitStaticLight = HitStatic.GetComponent<Light>();
            HitTargetLight = HitTarget.GetComponent<Light>();
            HitStatic.transform.parent = transform;
            HitTarget.transform.parent = transform;
            HitStatic.Stop(true);
            HitTarget.Stop(true);
            HitStaticLight.enabled = false;
            HitTargetLight.enabled = false;
        }
    }
}