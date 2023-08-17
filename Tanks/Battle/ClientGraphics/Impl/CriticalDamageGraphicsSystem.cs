using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CriticalDamageGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void RecieveCriticalEvent(CriticalDamageEvent evt, SingleNode<CriticalEffectComponent> node) {
            CriticalEffectEvent criticalEffectEvent = new();
            criticalEffectEvent.EffectPrefab = node.component.EffectAsset;
            criticalEffectEvent.LocalPosition = evt.LocalPosition;
            ScheduleEvent(criticalEffectEvent, evt.Target);
        }

        [OnEventFire]
        public void CreateEffect(CriticalEffectEvent evt, SingleNode<TankVisualRootComponent> node) {
            GameObject gameObject = Object.Instantiate(evt.EffectPrefab);
            gameObject.transform.parent = node.component.transform;
            gameObject.transform.localPosition = evt.LocalPosition;
            ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
            Object.Destroy(gameObject, component.duration);
        }
    }
}