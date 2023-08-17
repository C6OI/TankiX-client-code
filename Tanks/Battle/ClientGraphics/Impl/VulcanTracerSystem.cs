using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanTracerSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, VulcanTracerInitNode weapon) {
            GameObject gameObject = Object.Instantiate(weapon.vulcanTracerEffect.Tracer);
            UnityUtil.InheritAndEmplace(gameObject.transform, weapon.muzzlePoint.Current);
            gameObject.SetActive(false);
            gameObject.transform.localPosition += Vector3.forward * weapon.vulcanTracerEffect.StartTracerOffset;
            weapon.vulcanTracerEffect.Tracer = gameObject;
            weapon.Entity.AddComponent<VulcanTracerEffectReadyComponent>();
        }

        [OnEventFire]
        public void StartTracer(NodeAddedEvent evt, VulcanTracerShootingEffectNode weapon) =>
            weapon.vulcanTracerEffect.Tracer.SetActive(true);

        [OnEventComplete]
        public void UpdateTracer(UpdateEvent evt, VulcanTracerShootingEffectNode weapon) {
            VulcanTracerBehaviour component = weapon.vulcanTracerEffect.Tracer.GetComponent<VulcanTracerBehaviour>();

            if (!weapon.Entity.HasComponent<StreamHitComponent>()) {
                float tracerMaxLength = weapon.vulcanTracerEffect.TracerMaxLength;
                component.TargetPosition = new Vector3(0f, 0f, tracerMaxLength);
            } else {
                ScheduleEvent<UpdateVulcanTracerByStreamHitEvent>(weapon);
            }
        }

        [OnEventFire]
        public void UpdateTracer(UpdateVulcanTracerByStreamHitEvent evt, VulcanTracerStreamHitNode weapon) {
            StreamHitComponent streamHit = weapon.streamHit;
            GameObject tracer = weapon.vulcanTracerEffect.Tracer;
            VulcanTracerBehaviour component = tracer.GetComponent<VulcanTracerBehaviour>();

            if (streamHit.StaticHit != null) {
                Vector3 position = streamHit.StaticHit.Position;
                component.TargetPosition = MathUtil.WorldPositionToLocalPosition(position, tracer);
            } else if (streamHit.TankHit != null && weapon.Entity.HasComponent<StreamHitTargetLoadedComponent>()) {
                HitTarget tankHit = streamHit.TankHit;
                Entity entity = tankHit.Entity;
                UpdateVulcanTracerByTargetTankEvent updateVulcanTracerByTargetTankEvent = new();
                updateVulcanTracerByTargetTankEvent.VulcanTracerBehaviour = component;
                updateVulcanTracerByTargetTankEvent.Hit = streamHit.TankHit;
                updateVulcanTracerByTargetTankEvent.VulcanTracerInstance = tracer;
                ScheduleEvent(updateVulcanTracerByTargetTankEvent, entity);
            }
        }

        [OnEventFire]
        public void UpdateTracer(UpdateVulcanTracerByTargetTankEvent evt, HullNode tank) {
            GameObject hullInstance = tank.hullInstance.HullInstance;
            Vector3 position = MathUtil.LocalPositionToWorldPosition(evt.Hit.LocalHitPoint, hullInstance);

            evt.VulcanTracerBehaviour.TargetPosition =
                MathUtil.WorldPositionToLocalPosition(position, evt.VulcanTracerInstance);
        }

        [OnEventFire]
        public void StopTracer(NodeRemoveEvent evt, VulcanTracerShootingEffectNode node) =>
            node.vulcanTracerEffect.Tracer.SetActive(false);

        public class VulcanTracerInitNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public VulcanTracerEffectComponent vulcanTracerEffect;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class VulcanTracerShootingEffectNode : Node {
            public VulcanShootingComponent vulcanShooting;

            public VulcanTracerEffectComponent vulcanTracerEffect;
            public VulcanTracerEffectReadyComponent vulcanTracerEffectReady;

            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class VulcanTracerStreamHitNode : Node {
            public StreamHitComponent streamHit;

            public VulcanTracerEffectComponent vulcanTracerEffect;
            public VulcanTracerEffectReadyComponent vulcanTracerEffectReady;
        }

        public class HullNode : Node {
            public HullInstanceComponent hullInstance;
        }
    }
}