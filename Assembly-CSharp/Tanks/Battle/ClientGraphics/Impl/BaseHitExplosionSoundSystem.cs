using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class BaseHitExplosionSoundSystem : ECSSystem {
        protected void CreateHitExplosionSoundEffect(Vector3 position, GameObject prefab, float duration) {
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = prefab;
            getInstanceFromPoolEvent.AutoRecycleTime = duration;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            getInstanceFromPoolEvent2.Instance.position = position;
            getInstanceFromPoolEvent2.Instance.rotation = Quaternion.identity;
            getInstanceFromPoolEvent2.Instance.gameObject.SetActive(true);
        }
    }
}