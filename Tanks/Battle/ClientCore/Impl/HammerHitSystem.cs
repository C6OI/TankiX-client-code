using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class HammerHitSystem : ECSSystem {
        [OnEventComplete]
        public void SendShot(SendShotToServerEvent evt, UnblockedHammerNode hammer) {
            SelfHammerShotEvent selfHammerShotEvent = new();
            selfHammerShotEvent.RandomSeed = Time.frameCount;
            selfHammerShotEvent.ShotDirection = evt.TargetingData.BestDirection.Dir;
            ScheduleEvent(selfHammerShotEvent, hammer);
        }

        [OnEventComplete]
        public void SendShot(ShotPrepareEvent evt, NotUnblockedHammerNode hammer) {
            SelfHammerShotEvent selfHammerShotEvent = new();
            selfHammerShotEvent.RandomSeed = Time.frameCount;

            selfHammerShotEvent.ShotDirection =
                new MuzzleLogicAccessor(hammer.muzzlePoint, hammer.weaponInstance).GetFireDirectionWorld();

            ScheduleEvent(selfHammerShotEvent, hammer);
        }

        public class HammerNode : Node {
            public HammerComponent hammer;

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