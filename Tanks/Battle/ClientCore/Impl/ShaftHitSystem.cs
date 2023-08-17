using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShaftHitSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventComplete]
        public void PrepareQuickShotTargets(ShotPrepareEvent evt, UnblockedShaftNode weapon) =>
            PrepareQuickShotTargets(weapon.Entity);

        [OnEventComplete]
        public void PrepareQuickShotTargets(ShotPrepareEvent evt, BlockedShaftNode weapon) =>
            PrepareQuickShotTargets(weapon.Entity);

        [OnEventComplete]
        public void SendHitToServer(SendHitToServerEvent e, ShaftNode weapon) {
            if (e.TargetingData.BestDirection.HasAnyHit()) {
                SelfShaftAimingHitEvent selfShaftAimingHitEvent =
                    new(HitTargetAdapter.Adapt(e.TargetingData.BestDirection.Targets),
                        e.TargetingData.BestDirection.StaticHit);

                selfShaftAimingHitEvent.HitPower = 0f;
                ScheduleEvent(selfShaftAimingHitEvent, weapon);
            }
        }

        [OnEventComplete]
        public void PrepareAimingTargets(ShaftAimingShotPrepareEvent evt, ShaftNode weapon) =>
            PrepareAimingTargets(weapon.Entity, evt.WorkingDir);

        [OnEventFire]
        public void SendHit(SendShaftAimingHitToServerEvent evt, ShaftNode weapon) {
            if (evt.TargetingData.BestDirection.HasAnyHit()) {
                SelfShaftAimingHitEvent selfShaftAimingHitEvent = new();
                float energy = weapon.weaponEnergy.Energy;
                CompleteTargets(selfShaftAimingHitEvent, evt.TargetingData, energy);
                ScheduleEvent(selfShaftAimingHitEvent, weapon);
            }
        }

        void PrepareQuickShotTargets(Entity weapon) {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            ScheduleEvent(new TargetingEvent(targetingData), weapon);
            ScheduleEvent(new SendShotToServerEvent(targetingData), weapon);
            ScheduleEvent(new SendHitToServerEvent(targetingData), weapon);
        }

        void PrepareAimingTargets(Entity weapon, Vector3 workingDir) {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            ShaftAimingStraightTargetingEvent shaftAimingStraightTargetingEvent = new();
            shaftAimingStraightTargetingEvent.TargetingData = targetingData;
            shaftAimingStraightTargetingEvent.WorkingDirection = workingDir;
            ShaftAimingStraightTargetingEvent eventInstance = shaftAimingStraightTargetingEvent;
            ScheduleEvent(eventInstance, weapon);
            ScheduleEvent(new SendShotToServerEvent(targetingData), weapon);
            ScheduleEvent(new SendShaftAimingHitToServerEvent(targetingData), weapon);
        }

        void CompleteTargets(SelfShaftAimingHitEvent hitEvent, TargetingData targeting, float energy) {
            hitEvent.Targets = HitTargetAdapter.Adapt(targeting.BestDirection.Targets);
            hitEvent.StaticHit = targeting.BestDirection.StaticHit;
            hitEvent.HitPower = 1f - energy;
        }

        public class ShaftNode : Node {
            public ShaftComponent shaft;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class UnblockedShaftNode : ShaftNode {
            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class BlockedShaftNode : ShaftNode {
            public WeaponBlockedComponent weaponBlocked;
        }
    }
}