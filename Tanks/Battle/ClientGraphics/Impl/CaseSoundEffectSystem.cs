using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CaseSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitCaseEjectionSound(NodeAddedEvent evt, [Combine] InitialCaseEjectionSoundEffectNode weapon,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.gameObject.transform;
            PrepareCaseSoundEffect(weapon.caseEjectionSoundEffect, transform);
            weapon.Entity.AddComponent<CaseEjectionSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void PlayCaseEjectionSound(CartridgeCaseEjectionEvent evt, ReadyCaseEjectionSoundEffectNode weapon) =>
            weapon.caseEjectionSoundEffect.Source.Play();

        [OnEventFire]
        public void InitCaseEjectorMovementEffects(NodeAddedEvent evt,
            [Combine] InitialCaseEjectorMovementSoundEffectNode weapon,
            SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = weapon.weaponSoundRoot.gameObject.transform;
            PrepareCaseSoundEffect(weapon.caseEjectorOpeningSoundEffect, transform);
            PrepareCaseSoundEffect(weapon.caseEjectorClosingSoundEffect, transform);
            Entity entity = weapon.Entity;
            weapon.caseEjectorMovementTrigger.Entity = entity;
            entity.AddComponent<CaseEjectorMovementSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void PlayEjectorOpeningEffect(CaseEjectorOpenEvent evt, ReadyCaseEjectorMovementSoundEffectNode weapon) {
            weapon.caseEjectorClosingSoundEffect.Source.Stop();
            weapon.caseEjectorOpeningSoundEffect.Source.Play();
        }

        [OnEventFire]
        public void PlayEjectorClosingEffect(CaseEjectorCloseEvent evt, ReadyCaseEjectorMovementSoundEffectNode weapon) {
            weapon.caseEjectorOpeningSoundEffect.Source.Stop();
            weapon.caseEjectorClosingSoundEffect.Source.Play();
        }

        void PrepareCaseSoundEffect(CaseSoundEffectComponent caseSoundEffect, Transform root) {
            GameObject caseSoundAsset = caseSoundEffect.CaseSoundAsset;
            AudioSource source = InstantiateCaseSoundEffect(caseSoundAsset, root);
            caseSoundEffect.Source = source;
        }

        AudioSource InstantiateCaseSoundEffect(GameObject prefab, Transform root) {
            GameObject gameObject = Object.Instantiate(prefab);
            gameObject.transform.parent = root;
            gameObject.transform.localPosition = Vector3.zero;
            return gameObject.GetComponent<AudioSource>();
        }

        public class InitialCaseEjectionSoundEffectNode : Node {
            public AnimationPreparedComponent animationPrepared;
            public CaseEjectionSoundEffectComponent caseEjectionSoundEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class ReadyCaseEjectionSoundEffectNode : Node {
            public CaseEjectionSoundEffectComponent caseEjectionSoundEffect;

            public CaseEjectionSoundEffectReadyComponent caseEjectionSoundEffectReady;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class InitialCaseEjectorMovementSoundEffectNode : Node {
            public CaseEjectorClosingSoundEffectComponent caseEjectorClosingSoundEffect;

            public CaseEjectorMovementTriggerComponent caseEjectorMovementTrigger;
            public CaseEjectorOpeningSoundEffectComponent caseEjectorOpeningSoundEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class ReadyCaseEjectorMovementSoundEffectNode : Node {
            public CaseEjectorClosingSoundEffectComponent caseEjectorClosingSoundEffect;

            public CaseEjectorMovementSoundEffectReadyComponent caseEjectorMovementSoundEffectReady;
            public CaseEjectorOpeningSoundEffectComponent caseEjectorOpeningSoundEffect;

            public TankGroupComponent tankGroup;

            public WeaponSoundRootComponent weaponSoundRoot;
        }
    }
}