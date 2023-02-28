using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class HammerHitSystem : ECSSystem {
        [OnEventFire]
        public void SetShotFrame(NodeAddedEvent e, HammerNode hammer) {
            hammer.hammerPelletCone.ShotSeed = Time.frameCount;
        }

        [OnEventComplete]
        public void SetShotFrame(SelfHammerShotEvent e, HammerNode hammer) {
            hammer.hammerPelletCone.ShotSeed = Time.frameCount;
        }

        [OnEventComplete]
        public void SendShot(SendShotToServerEvent evt, UnblockedHammerNode hammer) {
            SelfHammerShotEvent selfHammerShotEvent = new();
            selfHammerShotEvent.RandomSeed = hammer.hammerPelletCone.ShotSeed;
            selfHammerShotEvent.ShotDirection = evt.TargetingData.BestDirection.Dir;
            ScheduleEvent(selfHammerShotEvent, hammer);
        }

        [OnEventComplete]
        public void SendShot(ShotPrepareEvent evt, NotUnblockedHammerNode hammer) {
            SelfHammerShotEvent selfHammerShotEvent = new();
            selfHammerShotEvent.RandomSeed = hammer.hammerPelletCone.ShotSeed;
            selfHammerShotEvent.ShotDirection = new MuzzleLogicAccessor(hammer.muzzlePoint, hammer.weaponInstance).GetFireDirectionWorld();
            ScheduleEvent(selfHammerShotEvent, hammer);
        }

        public class HammerNode : Node {
            public HammerComponent hammer;

            public HammerPelletConeComponent hammerPelletCone;

            public MuzzlePointComponent muzzlePoint;
        }

        public class UnblockedHammerNode : HammerNode {
            public WeaponUnblockedComponent weaponUnblocked;
        }

        [Not(typeof(WeaponUnblockedComponent))]
        public class NotUnblockedHammerNode : HammerNode {
            public WeaponInstanceComponent weaponInstance;
        }
    }
}