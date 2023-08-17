using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftHitSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(SelfHitEvent evt, SingleNode<ShaftQuickHitSoundEffectComponent> weapon,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) =>
            CreateShaftHitSoundEffect(evt, weapon.component);

        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(RemoteHitEvent evt, SingleNode<ShaftQuickHitSoundEffectComponent> weapon,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) =>
            CreateShaftHitSoundEffect(evt, weapon.component);

        [OnEventFire]
        public void CreateShaftQuickHitSelfSoundEffect(SelfShaftAimingHitEvent evt,
            SingleNode<ShaftAimingHitSoundEffectComponent> weapon,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) =>
            CreateShaftHitSoundEffect(evt, weapon.component);

        [OnEventFire]
        public void CreateShaftQuickHitSoundEffect(RemoteShaftAimingHitEvent evt,
            SingleNode<ShaftAimingHitSoundEffectComponent> weapon,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) =>
            CreateShaftHitSoundEffect(evt, weapon.component);

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
            Object obj = Object.Instantiate(effectComponent.Asset, position, Quaternion.identity);
            Object.Destroy(obj, effectComponent.Duration);
        }
    }
}