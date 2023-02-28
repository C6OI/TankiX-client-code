using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFrictionSoundSystem : ECSSystem {
        [OnEventFire]
        public void InitTankFrictionSound(NodeAddedEvent evt, [Combine] InitialTankFrictionSoundNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Entity entity = tank.Entity;
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            TankFrictionSoundEffectComponent tankFrictionSoundEffect = tank.tankFrictionSoundEffect;
            SoundController metallFrictionSourcePrefab = tankFrictionSoundEffect.MetallFrictionSourcePrefab;
            SoundController stoneFrictionSourcePrefab = tankFrictionSoundEffect.StoneFrictionSourcePrefab;
            SoundController frictionContactSourcePrefab = tankFrictionSoundEffect.FrictionContactSourcePrefab;
            tankFrictionSoundEffect.MetallFrictionSourceInstance = InitSoundTransforms(metallFrictionSourcePrefab, soundRootTransform);
            tankFrictionSoundEffect.StoneFrictionSourceInstance = InitSoundTransforms(stoneFrictionSourcePrefab, soundRootTransform);
            tankFrictionSoundEffect.FrictionContactSourceInstance = InitSoundTransforms(frictionContactSourcePrefab, soundRootTransform);
            GameObject gameObject = tank.tankFrictionMarker.gameObject;
            TankFrictionSoundBehaviour tankFrictionSoundBehaviour = gameObject.AddComponent<TankFrictionSoundBehaviour>();
            gameObject.layer = Layers.FRICTION;
            tankFrictionSoundBehaviour.Init(entity);
            BoxCollider boundsCollider = tank.tankColliders.BoundsCollider;
            float halfLength = boundsCollider.bounds.size.z * 0.5f;
            TankFrictionCollideSoundBehaviour tankFrictionCollideSoundBehaviour = tank.tankCollision.gameObject.AddComponent<TankFrictionCollideSoundBehaviour>();

            tankFrictionCollideSoundBehaviour.Init(tank.tankFrictionSoundEffect.FrictionContactSourceInstance, tank.rigidbody.Rigidbody, halfLength, tank.tankFallingSoundEffect.MinPower,
                tank.tankFallingSoundEffect.MaxPower);

            TankFrictionSoundEffectReadyComponent tankFrictionSoundEffectReadyComponent = new();
            tankFrictionSoundEffectReadyComponent.TankFrictionCollideSoundBehaviour = tankFrictionCollideSoundBehaviour;
            tankFrictionSoundEffectReadyComponent.TankFrictionSoundBehaviour = tankFrictionSoundBehaviour;
            tank.Entity.AddComponent(tankFrictionSoundEffectReadyComponent);
        }

        [OnEventFire]
        public void DisableTankFrictionSound(NodeAddedEvent evt, ReadyDeadTankFrictionSoundNode tank) {
            StopSounds(tank.tankFrictionSoundEffect);
            tank.tankFrictionSoundEffectReady.TankFrictionCollideSoundBehaviour.enabled = false;
            tank.tankFrictionSoundEffectReady.TankFrictionSoundBehaviour.enabled = false;
        }

        [OnEventFire]
        public void EnableTankFrictionSound(NodeAddedEvent evt, ReadyActiveTankFrictionSoundNode tank) {
            tank.tankFrictionSoundEffectReady.TankFrictionCollideSoundBehaviour.enabled = true;
            tank.tankFrictionSoundEffectReady.TankFrictionSoundBehaviour.enabled = true;
        }

        [OnEventFire]
        public void PlayTankFrictionSound(UpdateEvent evt, ReadyActiveTankFrictionSoundNode tank) {
            TankFrictionSoundBehaviour tankFrictionSoundBehaviour = tank.tankFrictionSoundEffectReady.TankFrictionSoundBehaviour;

            if (!tankFrictionSoundBehaviour.TriggerStay) {
                return;
            }

            TankFrictionSoundEffectComponent tankFrictionSoundEffect = tank.tankFrictionSoundEffect;
            Collider frictionCollider = tankFrictionSoundBehaviour.FrictionCollider;

            if (frictionCollider == null) {
                StopSounds(tankFrictionSoundEffect);
                return;
            }

            Vector3 velocity = tank.rigidbody.Rigidbody.velocity;

            if (frictionCollider.gameObject.layer == Layers.FRICTION) {
                SetMetallFriction(tankFrictionSoundEffect, velocity);
                return;
            }

            CollisionDustBehaviour collisionDustBehaviour = tank.collisionDust.CollisionDustBehaviour;
            DustEffectBehaviour effect = collisionDustBehaviour.Effect;

            if (effect == null) {
                StopSounds(tankFrictionSoundEffect);
                return;
            }

            switch (effect.surface) {
                case DustEffectBehaviour.SurfaceType.Metal:
                    SetMetallFriction(tankFrictionSoundEffect, velocity);
                    break;

                case DustEffectBehaviour.SurfaceType.Concrete:
                    SetStoneFriction(tankFrictionSoundEffect, velocity);
                    break;

                default:
                    StopSounds(tankFrictionSoundEffect);
                    break;
            }
        }

        [OnEventFire]
        public void StopTankFrictionSound(TankFrictionExitEvent evt, ReadyTankFrictionSoundNode tank) {
            TankFrictionSoundEffectComponent tankFrictionSoundEffect = tank.tankFrictionSoundEffect;
            StopSounds(tankFrictionSoundEffect);
        }

        SoundController InitSoundTransforms(SoundController sourcePrefab, Transform root) {
            SoundController soundController = Object.Instantiate(sourcePrefab);
            Transform transform = soundController.transform;
            transform.parent = root;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            return soundController;
        }

        void SetFriction(SoundController actualSource, SoundController stopSource, TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity) {
            SetFrictionVolume(actualSource, tankFrictionSoundEffect, velocity);
            actualSource.FadeIn();
            stopSource.FadeOut();
        }

        void StopSounds(TankFrictionSoundEffectComponent tankFrictionSoundEffect) {
            tankFrictionSoundEffect.MetallFrictionSourceInstance.FadeOut();
            tankFrictionSoundEffect.StoneFrictionSourceInstance.FadeOut();
        }

        void SetMetallFriction(TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity) {
            SetFriction(tankFrictionSoundEffect.MetallFrictionSourceInstance, tankFrictionSoundEffect.StoneFrictionSourceInstance, tankFrictionSoundEffect, velocity);
        }

        void SetStoneFriction(TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity) {
            SetFriction(tankFrictionSoundEffect.StoneFrictionSourceInstance, tankFrictionSoundEffect.MetallFrictionSourceInstance, tankFrictionSoundEffect, velocity);
        }

        void SetFrictionVolume(SoundController sound, TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity) {
            float minValuableFrictionPower = tankFrictionSoundEffect.MinValuableFrictionPower;
            float maxValuableFrictionPower = tankFrictionSoundEffect.MaxValuableFrictionPower;
            SetVolumeByVelocity(sound, velocity, minValuableFrictionPower, maxValuableFrictionPower);
        }

        void SetVolumeByVelocity(SoundController sound, Vector3 velocity, float min, float max) {
            if (!(sound == null)) {
                float sqrMagnitude = velocity.sqrMagnitude;
                float maxVolume = Mathf.Clamp01((sqrMagnitude - min) / (max - min));
                sound.MaxVolume = maxVolume;
            }
        }

        public class InitialTankFrictionSoundNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public CollisionDustComponent collisionDust;

            public RigidbodyComponent rigidbody;

            public TankCollidersComponent tankColliders;

            public TankCollisionComponent tankCollision;

            public TankFallingSoundEffectComponent tankFallingSoundEffect;

            public TankFrictionMarkerComponent tankFrictionMarker;

            public TankFrictionSoundEffectComponent tankFrictionSoundEffect;

            public TankSoundRootComponent tankSoundRoot;
        }

        public class ReadyTankFrictionSoundNode : InitialTankFrictionSoundNode {
            public TankFrictionSoundEffectReadyComponent tankFrictionSoundEffectReady;
        }

        public class ReadyActiveTankFrictionSoundNode : ReadyTankFrictionSoundNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class ReadyDeadTankFrictionSoundNode : ReadyTankFrictionSoundNode {
            public TankDeadStateComponent tankDeadState;
        }
    }
}