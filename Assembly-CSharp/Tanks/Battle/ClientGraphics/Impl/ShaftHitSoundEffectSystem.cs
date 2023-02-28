using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftHitSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(SelfHitEvent evt, ShaftQuickHitSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            CreateShaftHitSoundEffect(evt, weapon.shaftQuickHitSoundEffect);
        }

        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(RemoteHitEvent evt, ShaftQuickHitSoundNode weapon, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            CreateShaftHitSoundEffect(evt, weapon.shaftQuickHitSoundEffect);
        }

        [OnEventFire]
        public void CreateShaftQuickHitSelfSoundEffect(SelfShaftAimingHitEvent evt, SingleNode<ShaftAimingHitSoundEffectComponent> weapon,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            CreateShaftHitSoundEffect(evt, weapon.component);
        }

        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(RemoteShaftAimingHitEvent evt, SingleNode<ShaftAimingHitSoundEffectComponent> weapon,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            CreateShaftHitSoundEffect(evt, weapon.component);
        }

        void CreateShaftHitSoundEffect(HitEvent evt, ShaftHitSoundEffectComponent effectComponent) {
            if (evt.Targets != null) {
                foreach (HitTarget target in evt.Targets) {
                    CreateShaftHitSoundEffect(target.TargetPosition, effectComponent);
                }
            }

            if (evt.StaticHit != null) {
                CreateShaftHitSoundEffect(evt.StaticHit.Position, effectComponent);
            }
        }

        void CreateShaftHitSoundEffect(Vector3 position, ShaftHitSoundEffectComponent effectComponent) {
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = effectComponent.Asset;
            getInstanceFromPoolEvent.AutoRecycleTime = effectComponent.Duration;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            getInstanceFromPoolEvent2.Instance.position = position;
            getInstanceFromPoolEvent2.Instance.rotation = Quaternion.identity;
        }

        [Not(typeof(ShaftAimingWorkFinishStateComponent))]
        public class ShaftQuickHitSoundNode : Node {
            public ShaftQuickHitSoundEffectComponent shaftQuickHitSoundEffect;
        }
    }
}