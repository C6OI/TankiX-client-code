using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankExplosionGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void ShowExplosion(NodeAddedEvent evt, TankNode tank) {
            if (tank.cameraVisibleTrigger.IsVisible) {
                PlayEffect(tank.tankDeathExplosionPrefabs.ExplosionPrefab,
                    tank.tankVisualRoot.transform,
                    tank.mountPoint.MountPoint);
            }
        }

        [OnEventFire]
        public void ShowFire(NodeAddedEvent evt, TankNode tank, [JoinByTank] NormalTurretNode turret) {
            if (tank.cameraVisibleTrigger.IsVisible) {
                PlayEffect(tank.tankDeathExplosionPrefabs.FirePrefab,
                    tank.tankVisualRoot.transform,
                    tank.mountPoint.MountPoint);
            }
        }

        void PlayEffect(ParticleSystem prefab, Transform visualRoot, Transform mountPoint) {
            ParticleSystem particleSystem = Object.Instantiate(prefab);
            Transform transform = particleSystem.transform;
            GameObject gameObject = particleSystem.gameObject;
            transform.parent = visualRoot;
            transform.localPosition = mountPoint.localPosition;
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(true);
            particleSystem.Play(true);
            Object.Destroy(gameObject, particleSystem.duration);
        }

        public class TankNode : Node {
            public CameraVisibleTriggerComponent cameraVisibleTrigger;

            public MountPointComponent mountPoint;

            public TankCommonInstanceComponent tankCommonInstance;
            public TankDeadStateComponent tankDeadState;

            public TankDeathExplosionPrefabsComponent tankDeathExplosionPrefabs;

            public TankVisualRootComponent tankVisualRoot;
        }

        [Not(typeof(WeaponUndergroundComponent))]
        public class NormalTurretNode : Node {
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}