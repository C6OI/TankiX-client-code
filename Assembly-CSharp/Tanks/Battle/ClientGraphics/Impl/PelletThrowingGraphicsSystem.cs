using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PelletThrowingGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, PelletThrowingGraphicsInitNode node) {
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            PelletThrowingGraphicsComponent pelletThrowingGraphics = node.pelletThrowingGraphics;
            pelletThrowingGraphics.Trails = InstantiateAndInherit(muzzlePoint, pelletThrowingGraphics.Trails);
            pelletThrowingGraphics.Hits = InstantiateAndInherit(muzzlePoint, pelletThrowingGraphics.Hits);
            CustomRenderQueue.SetQueue(pelletThrowingGraphics.gameObject, 3150);
            node.Entity.AddComponent<PelletThrowingGraphicsReadyComponent>();
        }

        [OnEventFire]
        public void InstantiatePellets(SelfHammerShotEvent evt, PelletThrowingGraphicsNode weapon) {
            InstantiatePelletsByBaseEvent(evt.ShotDirection, weapon);
        }

        [OnEventFire]
        public void InstantiatePellets(RemoteHammerShotEvent evt, PelletThrowingGraphicsNode weapon) {
            weapon.hammerPelletCone.ShotSeed = evt.RandomSeed;
            InstantiatePelletsByBaseEvent(evt.ShotDirection, weapon);
        }

        static void InstantiatePelletsByBaseEvent(Vector3 shotDirection, PelletThrowingGraphicsNode weapon) {
            MuzzlePointComponent muzzlePoint = weapon.muzzlePoint;
            PelletThrowingGraphicsComponent pelletThrowingGraphics = weapon.pelletThrowingGraphics;
            float radiusOfMinDamage = weapon.damageWeakeningByDistance.RadiusOfMinDamage;
            float constant = pelletThrowingGraphics.Trails.main.startSpeed.constant;
            float startLifetime = radiusOfMinDamage / constant;
            ParticleSystem.EmitParams emitParams = default;
            emitParams.position = pelletThrowingGraphics.Trails.transform.position;
            emitParams.startColor = pelletThrowingGraphics.Trails.main.startColor.color;
            emitParams.startSize = pelletThrowingGraphics.Trails.main.startSizeMultiplier;
            ParticleSystem.EmitParams emitParams2 = emitParams;
            emitParams = default;
            emitParams.startColor = pelletThrowingGraphics.Hits.main.startColor.color;
            emitParams.startSize = pelletThrowingGraphics.Hits.main.startSizeMultiplier;
            ParticleSystem.EmitParams emitParams3 = emitParams;
            Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(shotDirection);
            Vector3[] randomDirections = PelletDirectionsCalculator.GetRandomDirections(weapon.hammerPelletCone, muzzlePoint.Current.rotation, localDirection);
            Vector3[] array = randomDirections;

            foreach (Vector3 vector in array) {
                if (pelletThrowingGraphics.Trails.main.startColor.mode == ParticleSystemGradientMode.RandomColor) {
                    emitParams2.startColor = pelletThrowingGraphics.Trails.main.startColor.Evaluate(Random.Range(0f, 1f));
                }

                emitParams2.randomSeed = (uint)(Random.value * 4.2949673E+09f);
                emitParams2.velocity = vector * constant;
                RaycastHit hitInfo;

                if (Physics.Raycast(pelletThrowingGraphics.Trails.transform.position, vector, out hitInfo, radiusOfMinDamage, LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS)) {
                    emitParams2.startLifetime = Vector3.Distance(pelletThrowingGraphics.Trails.transform.position, hitInfo.point) / constant;
                    emitParams3.startLifetime = Random.Range(pelletThrowingGraphics.SparklesMinLifetime, pelletThrowingGraphics.SparklesMaxLifetime);
                    emitParams3.randomSeed = (uint)(Random.value * 4.2949673E+09f);
                    emitParams3.position = hitInfo.point;
                    emitParams3.velocity = Random.onUnitSphere;
                    emitParams3.velocity *= Mathf.Sign(Vector3.Dot(emitParams3.velocity, hitInfo.normal)) * pelletThrowingGraphics.HitReflectVeolcity;
                    pelletThrowingGraphics.Hits.Emit(emitParams3, 1);
                } else {
                    emitParams2.startLifetime = startLifetime;
                }

                pelletThrowingGraphics.Trails.Emit(emitParams2, 1);
            }
        }

        static ParticleSystem InstantiateAndInherit(MuzzlePointComponent muzzlePoint, ParticleSystem prefab) {
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