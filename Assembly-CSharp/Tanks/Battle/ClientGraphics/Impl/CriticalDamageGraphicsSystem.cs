using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CriticalDamageGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void ReceiveCriticalEvent(CriticalDamageEvent evt, SingleNode<CriticalEffectComponent> node) {
            CriticalEffectEvent criticalEffectEvent = new();
            criticalEffectEvent.EffectPrefab = node.component.EffectAsset;
            criticalEffectEvent.LocalPosition = evt.LocalPosition;
            CriticalEffectEvent eventInstance = criticalEffectEvent;
            ScheduleEvent(eventInstance, evt.Target);
        }

        [OnEventFire]
        public void CreateEffect(CriticalEffectEvent evt, SingleNode<TankVisualRootComponent> tank, [JoinByTank] TankIncarnationWithoutCriticalEffectNode tankIncarnation) {
            ParticleSystem component = evt.EffectPrefab.GetComponent<ParticleSystem>();
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = evt.EffectPrefab;
            getInstanceFromPoolEvent.AutoRecycleTime = component.duration;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, tank);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            instance.parent = tank.component.transform;
            instance.localPosition = evt.LocalPosition;
            instance.gameObject.SetActive(true);
        }

        public class TankIncarnationWithoutCriticalEffectNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }
    }
}