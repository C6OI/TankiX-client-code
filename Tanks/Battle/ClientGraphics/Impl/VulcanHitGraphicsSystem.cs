using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanHitGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, VulcanHitGraphicsEffectInitNode weapon) {
            VulcanHitGraphicsEffectComponent vulcanHitGraphicsEffect = weapon.vulcanHitGraphicsEffect;
            vulcanHitGraphicsEffect.Init();
            weapon.Entity.AddComponent<VulcanHitGraphicsEffectReadyComponent>();
        }

        [OnEventFire]
        public void StopHitEffect(NodeRemoveEvent evt, VulcanHitGraphicsNode weapon) {
            VulcanHitGraphicsEffectComponent vulcanHitGraphicsEffect = weapon.vulcanHitGraphicsEffect;
            vulcanHitGraphicsEffect.HitStatic.Stop(true);
            vulcanHitGraphicsEffect.HitTarget.Stop(true);
            vulcanHitGraphicsEffect.HitStaticLight.enabled = false;
            vulcanHitGraphicsEffect.HitTargetLight.enabled = false;
        }

        [OnEventComplete]
        public void UpdateHitEffect(UpdateEvent evt, VulcanHitGraphicsNode weapon) {
            VulcanHitGraphicsEffectComponent vulcanHitGraphicsEffect = weapon.vulcanHitGraphicsEffect;
            StreamHitComponent streamHit = weapon.streamHit;

            if (streamHit.StaticHit != null) {
                vulcanHitGraphicsEffect.HitStatic.transform.position =
                    streamHit.StaticHit.Position + streamHit.StaticHit.Normal * vulcanHitGraphicsEffect.HitOffset;

                vulcanHitGraphicsEffect.HitStatic.transform.rotation = Quaternion.LookRotation(streamHit.StaticHit.Normal);
                vulcanHitGraphicsEffect.HitStatic.Play(true);
                vulcanHitGraphicsEffect.HitStaticLight.enabled = true;
            } else if (streamHit.TankHit != null && weapon.Entity.HasComponent<StreamHitTargetLoadedComponent>()) {
                Entity entity = streamHit.TankHit.Entity;
                UpdateVulcanHitGraphicsByTargetTankEvent updateVulcanHitGraphicsByTargetTankEvent = new();
                updateVulcanHitGraphicsByTargetTankEvent.HitTargetParticleSystem = vulcanHitGraphicsEffect.HitTarget;
                updateVulcanHitGraphicsByTargetTankEvent.HitTargetLight = vulcanHitGraphicsEffect.HitTargetLight;
                updateVulcanHitGraphicsByTargetTankEvent.TankHit = streamHit.TankHit;
                updateVulcanHitGraphicsByTargetTankEvent.HitOffset = vulcanHitGraphicsEffect.HitOffset;
                ScheduleEvent(updateVulcanHitGraphicsByTargetTankEvent, entity);
            }
        }

        [OnEventFire]
        public void UpdateHitEffect(UpdateVulcanHitGraphicsByTargetTankEvent evt, HullNode tank) {
            GameObject hullInstance = tank.hullInstance.HullInstance;

            evt.HitTargetParticleSystem.transform.position =
                hullInstance.transform.TransformPoint(evt.TankHit.LocalHitPoint) - evt.TankHit.HitDirection * evt.HitOffset;

            evt.HitTargetParticleSystem.transform.rotation = Quaternion.LookRotation(evt.TankHit.HitDirection);
            evt.HitTargetParticleSystem.Play(true);
            evt.HitTargetLight.enabled = true;
        }

        public class VulcanHitGraphicsEffectInitNode : Node {
            public VulcanHitGraphicsEffectComponent vulcanHitGraphicsEffect;
        }

        public class VulcanHitGraphicsNode : Node {
            public StreamHitComponent streamHit;

            public VulcanHitGraphicsEffectComponent vulcanHitGraphicsEffect;
            public VulcanHitGraphicsEffectReadyComponent vulcanHitGraphicsEffectReady;
        }

        public class HullNode : Node {
            public HullInstanceComponent hullInstance;
        }
    }
}