using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanMuzzleFlashSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, VulcanMuzzleFlashInitNode node) {
            GameObject gameObject = Object.Instantiate(node.vulcanMuzzleFlash.EffectPrefab);
            UnityUtil.InheritAndEmplace(gameObject.transform, node.muzzlePoint.Current);
            node.vulcanMuzzleFlash.EffectInstance = gameObject.GetComponent<ParticleSystem>();
            node.vulcanMuzzleFlash.LightInstance = gameObject.GetComponent<Light>();
            node.Entity.AddComponent<VulcanMuzzleFlashReadyComponent>();
        }

        [OnEventFire]
        public void StartEffect(NodeAddedEvent evt, VulcanMuzzleFlashNode node) {
            node.vulcanMuzzleFlash.EffectInstance.Play(true);
            node.vulcanMuzzleFlash.LightInstance.enabled = true;
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent evt, VulcanMuzzleFlashNode node) {
            node.vulcanMuzzleFlash.EffectInstance.Stop(true);
            node.vulcanMuzzleFlash.LightInstance.enabled = false;
        }

        public class VulcanMuzzleFlashInitNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public VulcanMuzzleFlashComponent vulcanMuzzleFlash;
        }

        public class VulcanMuzzleFlashNode : Node {
            public VulcanMuzzleFlashComponent vulcanMuzzleFlash;
            public VulcanMuzzleFlashReadyComponent vulcanMuzzleFlashReady;

            public VulcanShootingComponent vulcanShooting;
        }
    }
}