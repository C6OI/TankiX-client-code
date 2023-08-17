using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponHitSystem : ECSSystem {
        [OnEventComplete]
        public void PrepareTargets(ShotPrepareEvent evt, UnblockedWeaponNode weaponNode) {
            TargetingData targetingData = new();
            ScheduleEvent(new TargetingEvent(targetingData), weaponNode);
            ScheduleEvent(new SendShotToServerEvent(targetingData), weaponNode);
            ScheduleEvent(new SendHitToServerIfNeedEvent(targetingData), weaponNode);
        }

        [OnEventComplete]
        public void SendHit(SendHitToServerIfNeedEvent evt, UnblockedWeaponNode weapon) {
            WeaponHitComponent weaponHit = weapon.weaponHit;
            StaticHit staticHit = null;
            List<HitTarget> list = new(4);

            if (evt.TargetingData.HasTargetHit()) {
                if (weaponHit.RemoveDuplicateTargets) {
                    HashSet<Entity> hashSet = new();

                    foreach (DirectionData direction in evt.TargetingData.Directions) {
                        foreach (TargetData target in direction.Targets) {
                            if (hashSet.Add(target.Entity)) {
                                list.Add(HitTargetAdapter.Adapt(target));
                            }
                        }
                    }
                } else {
                    list = HitTargetAdapter.Adapt(evt.TargetingData.BestDirection.Targets);
                }
            }

            if (weaponHit.SendStaticHit && evt.TargetingData.HasStaticHit()) {
                staticHit = evt.TargetingData.BestDirection.StaticHit;
            }

            if (staticHit != null || list.Count != 0) {
                ScheduleEvent(new SendHitToServerEvent(evt.TargetingData, list, staticHit), weapon);
            }
        }

        [OnEventComplete]
        public void SendHitToServer(SendHitToServerEvent e, UnblockedWeaponNode weapon) {
            SelfHitEvent selfHitEvent = new(e.Targets, e.StaticHit);
            selfHitEvent.ShotId = weapon.shotId.ShotId;
            ScheduleEvent(selfHitEvent, weapon);
        }

        public class UnblockedWeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShotIdComponent shotId;
            public WeaponHitComponent weaponHit;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}