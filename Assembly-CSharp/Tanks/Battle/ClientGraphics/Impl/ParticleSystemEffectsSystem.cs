using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ParticleSystemEffectsSystem : ECSSystem {
        [OnEventFire]
        public void TankActive(NodeAddedEvent e, TankWithParticleActiveStateNode tankActiveStateNode) {
            tankActiveStateNode.particleSystemEffects.StartEmission();
        }

        [OnEventFire]
        public void TankDead(NodeAddedEvent e, TankWithParticleDeadStateNode tankDeadStateNode) {
            tankDeadStateNode.particleSystemEffects.StopEmission();
        }

        [OnEventFire]
        public void TankVisible(DeactivateTankInvisibilityEffectEvent e, TankParticleNode tankWithEffectNode) {
            tankWithEffectNode.particleSystemEffects.StartEmission();
        }

        [OnEventFire]
        public void TankInvisible(ActivateTankInvisibilityEffectEvent e, TankParticleNode tankWithEffectNode) {
            tankWithEffectNode.particleSystemEffects.StopEmission();
        }

        [OnEventFire]
        public void StartAiming(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon, [Combine] TankParticleNode tankWithEffectNode) {
            tankWithEffectNode.particleSystemEffects.StopEmission();
        }

        [OnEventFire]
        public void StopAiming(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon, [Combine] TankParticleNode tankWithEffectNode) {
            tankWithEffectNode.particleSystemEffects.StartEmission();
        }

        [OnEventFire]
        public void TankActive(NodeAddedEvent e, TankActiveStateNode tankActiveStateNode, [JoinByTank] ICollection<SkinParticleNode> skins) {
            skins.ForEach(delegate(SkinParticleNode skin) {
                skin.skinParticleSystemEffects.StartEmission();
            });
        }

        [OnEventFire]
        public void TankDead(NodeAddedEvent e, TankDeadStateNode tankDeadStateNode, [JoinByTank] ICollection<SkinParticleNode> skins) {
            skins.ForEach(delegate(SkinParticleNode skin) {
                skin.skinParticleSystemEffects.StopEmission();
            });
        }

        [OnEventFire]
        public void TankVisible(DeactivateTankInvisibilityEffectEvent e, TankActiveStateNode tank, [JoinByTank] ICollection<SkinParticleNode> skins) {
            skins.ForEach(delegate(SkinParticleNode skin) {
                skin.skinParticleSystemEffects.StartEmission();
            });
        }

        [OnEventFire]
        public void TankInvisible(ActivateTankInvisibilityEffectEvent e, TankActiveStateNode tank, [JoinByTank] ICollection<SkinParticleNode> skins) {
            skins.ForEach(delegate(SkinParticleNode skin) {
                skin.skinParticleSystemEffects.StopEmission();
            });
        }

        [OnEventFire]
        public void StartAiming(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon, [JoinByTank] ICollection<SkinParticleNode> skins) {
            skins.ForEach(delegate(SkinParticleNode skin) {
                skin.skinParticleSystemEffects.StopEmission();
            });
        }

        [OnEventFire]
        public void StopAiming(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon, [JoinByTank] ICollection<SkinParticleNode> skins) {
            skins.ForEach(delegate(SkinParticleNode skin) {
                skin.skinParticleSystemEffects.StartEmission();
            });
        }

        public class TankActiveStateNode : Node {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class TankDeadStateNode : Node {
            public TankDeadStateComponent tankDeadState;
            public TankGroupComponent tankGroup;
        }

        public class TankParticleNode : Node {
            public ParticleSystemEffectsComponent particleSystemEffects;
            public TankGroupComponent tankGroup;
        }

        public class TankWithParticleActiveStateNode : TankParticleNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class TankWithParticleDeadStateNode : TankParticleNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class ShaftAimingWorkingStateNode : Node {
            public SelfComponent self;

            public ShaftAimingAnimationReadyComponent shaftAimingAnimationReady;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class SkinParticleNode : Node {
            public SkinParticleSystemEffectsComponent skinParticleSystemEffects;
            public TankGroupComponent tankGroup;
        }
    }
}