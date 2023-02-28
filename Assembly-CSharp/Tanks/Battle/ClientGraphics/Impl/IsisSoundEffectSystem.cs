using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IsisSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitIsisSounds(NodeAddedEvent evt, [Combine] InitialIsisSoundEffectNode weapon, SingleNode<SoundListenerBattleStateComponent> soundListener) {
            InitIsisSounds(weapon);
        }

        [OnEventFire]
        public void StartWorking(NodeAddedEvent evt, ReadyIsisSoundEffectWorkingNode weapon) {
            IsisCurrentSoundEffectComponent isisCurrentSoundEffectComponent = new();
            SoundController soundController = weapon.isisHealingSoundEffect.SoundController;
            isisCurrentSoundEffectComponent.SoundController = soundController;
            weapon.Entity.AddComponent(isisCurrentSoundEffectComponent);
        }

        [OnEventFire]
        public void StopWorking(NodeRemoveEvent evt, ReadyIsisSoundEffectWorkingNode weapon) {
            if (weapon.Entity.HasComponent<IsisCurrentSoundEffectComponent>()) {
                weapon.Entity.RemoveComponent<IsisCurrentSoundEffectComponent>();
            }
        }

        [OnEventFire]
        public void EnableIsisTargetSound(NodeAddedEvent e, TargetEffectNode weapon, [Context] [JoinByTank] IsisCurrentSoundEffectNode isisSound, [Context] [JoinByBattle] DMNode dm) {
            IsisCurrentSoundEffectComponent isisCurrentSoundEffect = isisSound.isisCurrentSoundEffect;
            SoundController soundController = weapon.isisDamagingSoundEffect.SoundController;
            UpdateIsisSoundEffect(isisCurrentSoundEffect, soundController);
        }

        [OnEventFire]
        public void EnableIsisTargetSound(NodeAddedEvent e, [Combine] TargetEffectTeamNode weapon, [Combine] [Context] [JoinByTank] IsisCurrentSoundEffectNode isisSound,
            [Context] [JoinByTeam] TeamNode weaponTeam) {
            Entity entity = weapon.streamHit.TankHit.Entity;
            NewEvent<UpdateIsisSoundModeEvent>().Attach(weapon).Attach(entity).Schedule();
        }

        [OnEventFire]
        public void UpdateIsisSoundMode(UpdateIsisSoundModeEvent evt, IsisCurrentSoundTeamEffectNode weapon, TankTeamNode target) {
            SoundController soundController =
                weapon.teamGroup.Key != target.teamGroup.Key ? weapon.isisDamagingSoundEffect.SoundController : weapon.isisHealingSoundEffect.SoundController;

            UpdateIsisSoundEffect(weapon.isisCurrentSoundEffect, soundController);
        }

        [OnEventFire]
        public void DisableIsisTargetSound(NodeRemoveEvent e, TargetEffectNode weapon, [JoinByTank] IsisCurrentSoundEffectNode isisSound) {
            IsisCurrentSoundEffectComponent isisCurrentSoundEffect = isisSound.isisCurrentSoundEffect;
            SoundController soundController = weapon.isisHealingSoundEffect.SoundController;
            UpdateIsisSoundEffect(isisCurrentSoundEffect, soundController);
        }

        [OnEventFire]
        public void PlayIsisSound(NodeAddedEvent evt, SingleNode<IsisCurrentSoundEffectComponent> weapon) {
            PlayIsisSoundEffect(weapon.component);
        }

        [OnEventFire]
        public void StopIsisSound(NodeRemoveEvent evt, SingleNode<IsisCurrentSoundEffectComponent> weapon) {
            if ((bool)weapon.component.SoundController) {
                StopIsisSoundEffect(weapon.component);
            }
        }

        void PlayIsisSoundEffect(IsisCurrentSoundEffectComponent currentSoundEffect) {
            if (!currentSoundEffect.WasStarted) {
                currentSoundEffect.WasStarted = true;
                currentSoundEffect.SoundController.FadeIn();
            }
        }

        void StopIsisSoundEffect(IsisCurrentSoundEffectComponent currentSoundEffect) {
            currentSoundEffect.WasStopped = true;
            currentSoundEffect.SoundController.FadeOut();
        }

        void UpdateIsisSoundEffect(IsisCurrentSoundEffectComponent isisCurrentSoundEffect, SoundController soundController) {
            if (isisCurrentSoundEffect.WasStopped) {
                return;
            }

            if (isisCurrentSoundEffect.WasStarted) {
                if (isisCurrentSoundEffect.SoundController != soundController) {
                    isisCurrentSoundEffect.SoundController.FadeOut();
                    isisCurrentSoundEffect.SoundController = soundController;
                    soundController.FadeIn();
                }
            } else {
                isisCurrentSoundEffect.SoundController = soundController;
                isisCurrentSoundEffect.WasStarted = true;
                soundController.FadeIn();
            }
        }

        void InitIsisSounds(InitialIsisSoundEffectNode weapon) {
            Transform transform = weapon.weaponSoundRoot.transform;
            InitIsisSoundEffect(weapon.isisDamagingSoundEffect, transform);
            InitIsisSoundEffect(weapon.isisHealingSoundEffect, transform);
            weapon.Entity.AddComponent<IsisSoundEffectReadyComponent>();
        }

        void InitIsisSoundEffect(AbstractIsisSoundEffectComponent isisSoundEffect, Transform root) {
            GameObject gameObject = Object.Instantiate(isisSoundEffect.Asset);
            gameObject.transform.parent = root;
            gameObject.transform.localPosition = Vector3.zero;
            isisSoundEffect.SoundController = gameObject.GetComponent<SoundController>();
        }

        public class InitialIsisSoundEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;

            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;

            public IsisHealingSoundEffectComponent isisHealingSoundEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class ReadyIsisSoundEffectNode : Node {
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;

            public IsisHealingSoundEffectComponent isisHealingSoundEffect;

            public IsisSoundEffectReadyComponent isisSoundEffectReady;
        }

        public class ReadyIsisSoundEffectWorkingNode : Node {
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;

            public IsisHealingSoundEffectComponent isisHealingSoundEffect;

            public IsisSoundEffectReadyComponent isisSoundEffectReady;
            public StreamWeaponWorkingComponent streamWeaponWorking;
        }

        public class TargetEffectNode : Node {
            public BattleGroupComponent battleGroup;
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;

            public IsisHealingSoundEffectComponent isisHealingSoundEffect;

            public IsisSoundEffectReadyComponent isisSoundEffectReady;

            public StreamHitComponent streamHit;

            public StreamHitTargetLoadedComponent streamHitTargetLoaded;

            public TankGroupComponent tankGroup;
        }

        public class TargetEffectTeamNode : Node {
            public BattleGroupComponent battleGroup;
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;

            public IsisHealingSoundEffectComponent isisHealingSoundEffect;

            public IsisSoundEffectReadyComponent isisSoundEffectReady;

            public StreamHitComponent streamHit;

            public StreamHitTargetLoadedComponent streamHitTargetLoaded;

            public TankGroupComponent tankGroup;

            public TeamGroupComponent teamGroup;
        }

        public class IsisCurrentSoundEffectNode : Node {
            public IsisCurrentSoundEffectComponent isisCurrentSoundEffect;

            public TankGroupComponent tankGroup;
        }

        public class IsisCurrentSoundTeamEffectNode : Node {
            public IsisCurrentSoundEffectComponent isisCurrentSoundEffect;
            public IsisDamagingSoundEffectComponent isisDamagingSoundEffect;

            public IsisHealingSoundEffectComponent isisHealingSoundEffect;

            public IsisSoundEffectReadyComponent isisSoundEffectReady;

            public TeamGroupComponent teamGroup;
        }

        public class DMNode : Node {
            public BattleGroupComponent battleGroup;
            public DMComponent dm;
        }

        public class TeamNode : Node {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
        }

        public class TankTeamNode : Node {
            public TankComponent tank;
            public TeamGroupComponent teamGroup;
        }
    }
}