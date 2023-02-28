using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerCleanerSystem : ECSSystem {
        [OnEventFire]
        public void PrepareCleaningForTankParts(NodeRemoveEvent evt, WeaponNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener) {
            Transform transform = weapon.weaponSoundRoot.transform;
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            float tankPartCleanTimeSec = listener.soundListenerCleaner.TankPartCleanTimeSec;
            PrepareCleaningForTankPart(soundRootTransform, tankPartCleanTimeSec);
            PrepareCleaningForTankPart(transform, tankPartCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForModuleEffectsParts(RemoveEffectEvent evt, SingleNode<EffectComponent> effect, [JoinByUnit] SingleNode<WeaponSoundRootComponent> weapon,
            [JoinAll] SoundListenerNode listener) {
            Transform transform = weapon.component.transform;
            float tankPartCleanTimeSec = listener.soundListenerCleaner.TankPartCleanTimeSec;
            PrepareCleaningForTankPart(transform, tankPartCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForMines(PrepareDestroyModuleEffectEvent evt, SingleNode<MineSoundsComponent> mine, [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(mine.component.DeactivationSound, mineCleanTimeSec);
            PrepareCleaningForTankPart(mine.component.ExplosionSound, mineCleanTimeSec);
            PrepareCleaningForTankPart(mine.component.DropGroundSound, mineCleanTimeSec);
            PrepareCleaningForTankPart(mine.component.DropNonGroundSound, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForMines(PrepareDestroyModuleEffectEvent evt, SingleNode<SpiderMineSoundsComponent> mine, [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(mine.component.RunSoundController.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<EffectRemovingSoundComponent> effect, [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(effect.component.Sound, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<EffectActivationSoundComponent> effect, [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(effect.component.Sound, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<DroneFlySoundEffectComponent> effect, [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(effect.component.Sound.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<WeaponShootingSoundEffectComponent> effect, [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(effect.component.SoundController.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<WeaponStreamHitSoundsEffectComponent> effect, [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(effect.component.SoundController.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForCTF(PrepareDestroyCTFSoundsEvent evt, SingleNode<CTFSoundsComponent> battle, [JoinAll] SoundListenerNode listener) {
            Object.DestroyObject(battle.component.EffectRoot, listener.soundListenerCleaner.CTFCleanTimeSec);
        }

        void PrepareCleaningForTankPart(Transform tankPartTransform, float destroyDelay) {
            tankPartTransform.SetParent(null, true);
            Object.DestroyObject(tankPartTransform.gameObject, destroyDelay);
        }

        void PrepareCleaningForTankPart(AudioSource source, float destroyDelay) {
            if (!(source == null)) {
                PrepareCleaningForTankPart(source.transform, Mathf.Max(destroyDelay, source.clip.length + 0.5f));
            }
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public TankGroupComponent tankGroup;
            public TankSoundRootComponent tankSoundRoot;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponSoundRootComponent weaponSoundRoot;
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerCleanerComponent soundListenerCleaner;
        }
    }
}