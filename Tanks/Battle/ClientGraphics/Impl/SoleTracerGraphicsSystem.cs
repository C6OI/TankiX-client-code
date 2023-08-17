using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoleTracerGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, SoleTracerGraphicsInitNode node) {
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            SoleTracerGraphicsComponent soleTracerGraphics = node.soleTracerGraphics;
            soleTracerGraphics.Tracer = Object.Instantiate(soleTracerGraphics.Tracer);
            UnityUtil.InheritAndEmplace(soleTracerGraphics.Tracer.transform, muzzlePoint.Current);
            node.Entity.AddComponent<SoleTracerGraphicsReadyComponent>();
        }

        [OnEventFire]
        public void InstantiatePellets(BaseShotEvent evt, SoleTracerGraphicsNode weapon) {
            MuzzlePointComponent muzzlePoint = weapon.muzzlePoint;
            SoleTracerGraphicsComponent soleTracerGraphics = weapon.soleTracerGraphics;
            float radiusOfMinDamage = weapon.damageWeakeningByDistance.RadiusOfMinDamage;
            float startLifetime = radiusOfMinDamage / soleTracerGraphics.Tracer.startSpeed;
            ParticleSystem.Particle particle = default;
            particle.position = soleTracerGraphics.Tracer.transform.position;
            particle.color = soleTracerGraphics.Tracer.startColor;
            particle.size = soleTracerGraphics.Tracer.startSize;
            ParticleSystem.Particle particle2 = particle;
            particle2.randomSeed = (uint)(Random.value * 4.2949673E+09f);
            particle2.velocity = evt.ShotDirection * soleTracerGraphics.Tracer.startSpeed;
            RaycastHit hitInfo;

            if (Physics.Raycast(soleTracerGraphics.Tracer.transform.position,
                    evt.ShotDirection,
                    out hitInfo,
                    radiusOfMinDamage,
                    LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS)) {
                particle2.startLifetime = Vector3.Distance(soleTracerGraphics.Tracer.transform.position, hitInfo.point) /
                                          soleTracerGraphics.Tracer.startSpeed;
            } else {
                particle2.startLifetime = startLifetime;
            }

            if (particle2.startLifetime > soleTracerGraphics.MaxTime) {
                particle2.velocity *= particle2.startLifetime / soleTracerGraphics.MaxTime;
                particle2.startLifetime = soleTracerGraphics.MaxTime;
            }

            particle2.remainingLifetime = particle2.startLifetime;
            soleTracerGraphics.Tracer.Emit(particle2);
        }

        public class SoleTracerGraphicsInitNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public SoleTracerGraphicsComponent soleTracerGraphics;
        }

        public class SoleTracerGraphicsNode : Node {
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;

            public MuzzlePointComponent muzzlePoint;

            public SoleTracerGraphicsComponent soleTracerGraphics;
            public SoleTracerGraphicsReadyComponent soleTracerGraphicsReady;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}