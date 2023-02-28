using Assets;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class EffectsHUDSystem : ECSSystem {
        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<ArmorEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<SonarEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<InvisibilityEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<DamageEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<TurboSpeedEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<TemperatureEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<InvulnerabilityEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<DroneEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<EMPEffectStartedComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<HealingEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void CreateBuff(NodeAddedEvent e, GenericEffectNode<ForceFieldEffectComponent> effect, [JoinByTank] [Context] HUDNodes.SelfTankNode selfTank,
            SingleNode<EffectsContainerComponent> container) {
            container.component.SpawnBuff(effect.Entity);
        }

        [OnEventFire]
        public void InitView(NodeAddedEvent e, EffectWithHUDNode effect) {
            effect.Entity.AddComponent<EffectIconComponent>();
        }

        [OnEventFire]
        public void InitIcon(NodeAddedEvent e, IconNode effect) {
            effect.effectHUD.InitBuff(effect.effectIcon.SpriteUid);
        }

        [OnEventFire]
        public void InitDuration(NodeAddedEvent e, DurationNode effect) {
            float duration = Mathf.Max(effect.durationConfig.Duration / 1000f - (Date.Now.UnityTime - effect.duration.StartedTime.UnityTime), 0f);
            effect.effectHUD.InitDuration(duration);
        }

        [OnEventFire]
        public void RemoveHUD(NodeRemoveEvent e, EffectNode effect, [JoinSelf] SingleNode<EffectHUDComponent> effectHUD) {
            effectHUD.component.Kill();
        }

        public class EffectNode : Node {
            public EffectComponent effect;

            public TankGroupComponent tankGroup;
        }

        public class GenericEffectNode<T> : EffectNode {
            public T marker;
        }

        public class EffectWithHUDNode : EffectNode {
            public EffectHUDComponent effectHUD;
        }

        public class IconNode : EffectWithHUDNode {
            public EffectIconComponent effectIcon;
        }

        public class DurationNode : EffectWithHUDNode {
            public DurationComponent duration;
            public DurationConfigComponent durationConfig;
        }
    }
}