using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PelletThrowingGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, PelletThrowingGraphicsInitNode node) {
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            PelletThrowingGraphicsComponent pelletThrowingGraphics = node.pelletThrowingGraphics;
            pelletThrowingGraphics.Trails = InstantiateAndInherit(muzzlePoint, pelletThrowingGraphics.Trails);
            pelletThrowingGraphics.Hits = InstantiateAndInherit(muzzlePoint, pelletThrowingGraphics.Hits);
            node.Entity.AddComponent<PelletThrowingGraphicsReadyComponent>();
        }

        [OnEventFire]
        public void InstantiatePellets(SelfHammerShotEvent evt, PelletThrowingGraphicsNode weapon) =>
            InstantiatePelletsByBaseEvent(evt.ShotDirection, evt.RandomSeed, weapon);

        [OnEventFire]
        public void InstantiatePellets(RemoteHammerShotEvent evt, PelletThrowingGraphicsNode weapon) =>
            InstantiatePelletsByBaseEvent(evt.ShotDirection, evt.RandomSeed, weapon);

        void InstantiatePelletsByBaseEvent(Vector3 shotDirection, int randomSeed, PelletThrowingGraphicsNode weapon) {
            MuzzlePointComponent muzzlePoint = weapon.muzzlePoint;
            PelletThrowingGraphicsComponent pelletThrowingGraphics = weapon.pelletThrowingGraphics;
            float radiusOfMinDamage = weapon.damageWeakeningByDistance.RadiusOfMinDamage;
            float startLifetime = radiusOfMinDamage / pelletThrowingGraphics.Trails.startSpeed;
            ParticleSystem.Particle particle = default;
            particle.position = pelletThrowingGraphics.Trails.transform.position;
            particle.color = pelletThrowingGraphics.Trails.startColor;
            particle.size = pelletThrowingGraphics.Trails.startSize;
            ParticleSystem.Particle particle2 = particle;
            particle = default;
            particle.color = pelletThrowingGraphics.Hits.startColor;
            particle.size = pelletThrowingGraphics.Hits.startSize;
            ParticleSystem.Particle particle3 = particle;
            Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(shotDirection);

            Vector3[] randomDirections = PelletDirectionsCalculator.GetRandomDirections(weapon.hammerPelletCone,
                muzzlePoint.Current.rotation,
                localDirection,
                randomSeed);

            for (int i = 0; i < randomDirections.Length; i++) {
                particle2.randomSeed = (uint)(Random.value * 4.2949673E+09f);
                particle2.velocity = randomDirections[i] * pelletThrowingGraphics.Trails.startSpeed;
                RaycastHit hitInfo;

                if (Physics.Raycast(pelletThrowingGraphics.Trails.transform.position,
                        randomDirections[i],
                        out hitInfo,
                        radiusOfMinDamage,
                        LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS)) {
                    particle2.startLifetime =
                        Vector3.Distance(pelletThrowingGraphics.Trails.transform.position, hitInfo.point) /
                        pelletThrowingGraphics.Trails.startSpeed;

                    particle3.startLifetime = Random.Range(pelletThrowingGraphics.SparklesMinLifetime,
                        pelletThrowingGraphics.SparklesMaxLifetime);

                    particle3.remainingLifetime = particle3.startLifetime;
                    particle3.randomSeed = (uint)(Random.value * 4.2949673E+09f);
                    particle3.position = hitInfo.point;
                    particle3.velocity = Random.onUnitSphere;

                    particle3.velocity *= Mathf.Sign(Vector3.Dot(particle3.velocity, hitInfo.normal)) *
                                          pelletThrowingGraphics.HitReflectVeolcity;

                    pelletThrowingGraphics.Hits.Emit(particle3);
                } else {
                    particle2.startLifetime = startLifetime;
                }

                particle2.remainingLifetime = particle2.startLifetime;
                pelletThrowingGraphics.Trails.Emit(particle2);
            }
        }

        ParticleSystem InstantiateAndInherit(MuzzlePointComponent muzzlePoint, ParticleSystem prefab) {
            ParticleSystem particleSystem = Object.Instantiate(prefab);
            UnityUtil.InheritAndEmplace(particleSystem.transform, muzzlePoint.Current);
            return particleSystem;
        }

        public class PelletThrowingGraphicsInitNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public PelletThrowingGraphicsComponent pelletThrowingGraphics;
        }

        public class PelletThrowingGraphicsNode : Node {
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;

            public HammerPelletConeComponent hammerPelletCone;

            public MuzzlePointComponent muzzlePoint;

            public PelletThrowingGraphicsComponent pelletThrowingGraphics;
            public PelletThrowingGraphicsReadyComponent pelletThrowingGraphicsReady;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}