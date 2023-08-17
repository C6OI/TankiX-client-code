using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerCleanerSystem : ECSSystem {
        [OnEventFire]
        public void PrepareCleaningForTankParts(NodeRemoveEvent evt, WeaponNode weapon, [JoinByTank] TankNode tank,
            [JoinAll] SoundListenerNode listener) {
            Transform transform = weapon.weaponSoundRoot.transform;
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            float tankPartCleanTimeSec = listener.soundListenerCleaner.TankPartCleanTimeSec;
            PrepareCleaningForTankPart(soundRootTransform, tankPartCleanTimeSec);
            PrepareCleaningForTankPart(transform, tankPartCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForMines(PrepareDestroyMineEvent evt, SingleNode<MineSoundsComponent> mine,
            [JoinAll] SoundListenerNode listener) {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            PrepareCleaningForTankPart(mine.component.DeactivationSound.transform, mineCleanTimeSec);
            PrepareCleaningForTankPart(mine.component.ExplosionSound.transform, mineCleanTimeSec);
            PrepareCleaningForTankPart(mine.component.DropGroundSound.transform, mineCleanTimeSec);
            PrepareCleaningForTankPart(mine.component.DropNonGroundSound.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForCTF(PrepareDestroyCTFSoundsEvent evt, SingleNode<CTFSoundsComponent> battle,
            [JoinAll] SoundListenerNode listener) => Object.DestroyObject(battle.component.EffectRoot,
            listener.soundListenerCleaner.CTFCleanTimeSec);

        void PrepareCleaningForTankPart(Transform tankPartTransform, float destroyDelay) {
            tankPartTransform.SetParent(null, true);
            Object.DestroyObject(tankPartTransform.gameObject, destroyDelay);
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