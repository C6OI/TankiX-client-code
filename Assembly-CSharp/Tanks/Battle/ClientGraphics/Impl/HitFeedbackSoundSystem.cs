using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HitFeedbackSoundSystem : ECSSystem {
        [OnEventFire]
        public void InitFreezeFeedbackSounds(NodeAddedEvent e, FreezeWeaponNode weapon, [Context] [JoinByTank] SelfTankNode tank) {
            InitStreamWeaponHitFeedback(weapon, tank, tank.hitFeedbackSounds.FreezeWeaponAttackController);
        }

        [OnEventFire]
        public void InitFlamethrowerFeedbackSounds(NodeAddedEvent e, FlamethrowerWeaponNode weapon, [Context] [JoinByTank] SelfTankNode tank) {
            InitStreamWeaponHitFeedback(weapon, tank, tank.hitFeedbackSounds.FlamethrowerWeaponAttackController);
        }

        [OnEventFire]
        public void StopStreamHitFeedbackSounds(NodeRemoveEvent e, SingleNode<SoundListenerReadyForHitFeedbackComponent> listener,
            [JoinAll] SingleNode<StreamWeaponHitFeedbackReadyComponent> weapon) {
            weapon.component.SoundController.FadeOut();
        }

        [OnEventFire]
        public void StopStreamHitFeedbackSounds(NodeRemoveEvent e, SingleNode<StreamHitEnemyFeedbackComponent> streamHitFeedback,
            [JoinSelf] SingleNode<StreamWeaponHitFeedbackReadyComponent> weapon) {
            weapon.component.SoundController.FadeOut();
        }

        [OnEventFire]
        public void StartStreamHitFeedbackSounds(NodeAddedEvent e, SingleNode<StreamHitEnemyFeedbackComponent> streamHitFeedback,
            [JoinSelf] [Context] SingleNode<StreamWeaponHitFeedbackReadyComponent> weapon) {
            weapon.component.SoundController.FadeIn();
        }

        [OnEventFire]
        public void InitIsisFeedbackSounds(NodeAddedEvent e, IsisWeaponNode isis, [Context] [JoinByTank] SelfTankNode tank) {
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            SoundController isisAttackFeedbackController = tank.hitFeedbackSounds.IsisAttackFeedbackController;
            SoundController isisHealingFeedbackController = tank.hitFeedbackSounds.IsisHealingFeedbackController;

            isis.Entity.AddComponent(new IsisHitFeedbackReadyComponent(AttachStreamSoundController(isisHealingFeedbackController, soundRootTransform),
                AttachStreamSoundController(isisAttackFeedbackController, soundRootTransform)));
        }

        [OnEventFire]
        public void StopIsisFeedbackSounds(NodeRemoveEvent e, SingleNode<SoundListenerReadyForHitFeedbackComponent> listener, [JoinAll] IsisReadyWeaponNode isis) {
            isis.isisHitFeedbackReady.HealingSoundController.FadeOut();
            isis.isisHitFeedbackReady.AttackSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopIsisHealingFeedbackSound(NodeRemoveEvent e, SingleNode<StreamHitTeammateFeedbackComponent> weapon, [JoinSelf] IsisReadyWeaponNode isis) {
            isis.isisHitFeedbackReady.HealingSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopIsisAttackFeedbackSound(NodeRemoveEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon, [JoinSelf] IsisReadyWeaponNode isis) {
            isis.isisHitFeedbackReady.AttackSoundController.FadeOut();
        }

        [OnEventFire]
        public void StartIsisHealingFeedbackSound(NodeAddedEvent e, SingleNode<StreamHitTeammateFeedbackComponent> weapon, [JoinSelf] [Context] IsisReadyWeaponNode isis,
            SoundListenerNode listener) {
            isis.isisHitFeedbackReady.HealingSoundController.FadeIn();
        }

        [OnEventFire]
        public void StartIsisAttackFeedbackSound(NodeAddedEvent e, SingleNode<StreamHitEnemyFeedbackComponent> weapon, [JoinSelf] [Context] IsisReadyWeaponNode isis,
            SoundListenerNode listener) {
            isis.isisHitFeedbackReady.AttackSoundController.FadeIn();
        }

        [OnEventFire]
        public void ClearHitFeedbackSounds(KillTankSoundEffectCreatedEvent e, SingleNode<SoundListenerComponent> listener) {
            WeaponFeedbackSoundBehaviour.ClearHitFeedbackSounds();
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] HammerHitFeedbackSoundNode weapon,
            [JoinAll] SoundListenerNode listener) {
            PlayHitFeedbackSound(tank.component.HammerHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] SmokyHitFeedbackSoundNode weapon,
            [JoinAll] SoundListenerNode listener) {
            PlayHitFeedbackSound(tank.component.SmokyHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] ThunderHitFeedbackSoundNode weapon,
            [JoinAll] SoundListenerNode listener) {
            PlayHitFeedbackSound(tank.component.ThunderHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] RailgunHitFeedbackSoundNode weapon,
            [JoinAll] SoundListenerNode listener) {
            PlayHitFeedbackSound(tank.component.RailgunHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] RicochetHitFeedbackSoundNode weapon,
            [JoinAll] SoundListenerNode listener) {
            PlayHitFeedbackSound(tank.component.RicochetHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        [OnEventFire]
        public void PlayHitFeedbackSound(HitFeedbackEvent e, SingleNode<HitFeedbackSoundsComponent> tank, [JoinByTank] ShaftHitFeedbackSoundNode weapon,
            [JoinAll] SoundListenerNode listener) {
            PlayHitFeedbackSound(tank.component.ShaftHitFeedbackSoundAsset, weapon.hitFeedbackSoundsPlayingSettings);
        }

        void InitStreamWeaponHitFeedback(WeaponNode weapon, SelfTankNode tank, SoundController attackAsset) {
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            weapon.Entity.AddComponent(new StreamWeaponHitFeedbackReadyComponent(AttachStreamSoundController(attackAsset, soundRootTransform)));
        }

        void PlayHitFeedbackSound(WeaponFeedbackSoundBehaviour asset, HitFeedbackSoundsPlayingSettingsComponent settings) {
            WeaponFeedbackSoundBehaviour weaponFeedbackSoundBehaviour = Object.Instantiate(asset);
            weaponFeedbackSoundBehaviour.Play(settings.Delay, settings.Volume, settings.RemoveOnKillSound);
        }

        SoundController AttachStreamSoundController(SoundController asset, Transform root) => Object.Instantiate(asset, root.position, root.rotation, root);

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerBattleStateComponent soundListenerBattleState;

            public SoundListenerReadyForHitFeedbackComponent soundListenerReadyForHitFeedback;
        }

        public class SelfTankNode : Node {
            public HitFeedbackSoundsComponent hitFeedbackSounds;
            public SelfTankComponent selfTank;

            public TankGroupComponent tankGroup;

            public TankSoundRootComponent tankSoundRoot;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class HitFeedbackSoundsPlayingSettingsWeaponNode : WeaponNode {
            public HitFeedbackSoundsPlayingSettingsComponent hitFeedbackSoundsPlayingSettings;
        }

        public class HammerHitFeedbackSoundNode : HitFeedbackSoundsPlayingSettingsWeaponNode {
            public HammerComponent hammer;
        }

        public class SmokyHitFeedbackSoundNode : HitFeedbackSoundsPlayingSettingsWeaponNode {
            public SmokyComponent smoky;
        }

        public class ThunderHitFeedbackSoundNode : HitFeedbackSoundsPlayingSettingsWeaponNode {
            public ThunderComponent thunder;
        }

        public class RailgunHitFeedbackSoundNode : HitFeedbackSoundsPlayingSettingsWeaponNode {
            public RailgunComponent railgun;
        }

        public class RicochetHitFeedbackSoundNode : HitFeedbackSoundsPlayingSettingsWeaponNode {
            public RicochetComponent ricochet;
        }

        public class ShaftHitFeedbackSoundNode : HitFeedbackSoundsPlayingSettingsWeaponNode {
            public ShaftComponent shaft;
        }

        public class IsisWeaponNode : WeaponNode {
            public IsisComponent isis;
        }

        public class FreezeWeaponNode : WeaponNode {
            public FreezeComponent freeze;
        }

        public class FlamethrowerWeaponNode : WeaponNode {
            public FlamethrowerComponent flamethrower;
        }

        public class IsisReadyWeaponNode : IsisWeaponNode {
            public IsisHitFeedbackReadyComponent isisHitFeedbackReady;
        }
    }
}