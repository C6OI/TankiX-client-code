using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankExplosionGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void ShowExplosion(NodeAddedEvent evt, TankNode tank) {
            if (tank.cameraVisibleTrigger.IsVisible) {
                float timeToPlay = tank.tankDeadState.EndDate.UnityTime - Time.time;
                PlayEffect(tank.tankDeathExplosionPrefabs.ExplosionPrefab, tank.tankVisualRoot.transform, tank.mountPoint.MountPoint, timeToPlay, tank);
            }
        }

        [OnEventFire]
        public void ShowFire(NodeAddedEvent evt, TankNode tank, [JoinByTank] NormalTurretNode turret) {
            if (tank.cameraVisibleTrigger.IsVisible) {
                float timeToPlay = tank.tankDeadState.EndDate - Date.Now;
                PlayEffect(tank.tankDeathExplosionPrefabs.FirePrefab, tank.tankVisualRoot.transform, tank.mountPoint.MountPoint, timeToPlay, tank);
            }
        }

        void PlayEffect(ParticleSystem prefab, Transform visualRoot, Transform mountPoint, float timeToPlay, Node entity) {
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = prefab.gameObject;
            getInstanceFromPoolEvent.AutoRecycleTime = Mathf.Min(timeToPlay, prefab.main.duration);
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, entity);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            GameObject gameObject = instance.gameObject;
            instance.parent = visualRoot;
            instance.localPosition = mountPoint.localPosition;
            instance.rotation = Quaternion.identity;
            gameObject.SetActive(true);
            gameObject.GetComponent<ParticleSystem>().Play(true);
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