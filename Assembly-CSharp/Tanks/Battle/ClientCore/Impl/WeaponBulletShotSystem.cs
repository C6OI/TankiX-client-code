using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class WeaponBulletShotSystem : ECSSystem {
        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventComplete]
        public void PrepareBestDirection(ShotPrepareEvent evt, UnblockedWeaponNode weaponNode, [JoinByTank] SingleNode<TankSyncComponent> tankNode) {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            ScheduleEvent(new TargetingEvent(targetingData), weaponNode);
            ScheduleEvent(new SendShotToServerEvent(targetingData), weaponNode);
        }

        [OnEventComplete]
        public void RequestBulletBuild(SendShotToServerEvent evt, UnblockedWeaponNode weaponNode) {
            ScheduleEvent(new BulletBuildEvent(evt.TargetingData.BestDirection.Dir), weaponNode);
        }

        [OnEventComplete]
        public void RequestBulletBuild(BaseShotEvent evt, BlockedWeaponNode weaponNode) {
            ScheduleEvent(new BulletBuildEvent(new MuzzleVisualAccessor(weaponNode.muzzlePoint).GetFireDirectionWorld()), weaponNode);
        }

        [OnEventComplete]
        public void RequestBulletBuild(RemoteShotEvent evt, UnblockedWeaponNode weaponNode) {
            ScheduleEvent(new BulletBuildEvent(evt.ShotDirection), weaponNode);
        }

        public class BlockedWeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public WeaponBlockedComponent weaponBlocked;
            public WeaponBulletShotComponent weaponBulletShot;
        }

        public class UnblockedWeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public WeaponBulletShotComponent weaponBulletShot;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}