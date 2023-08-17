using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class MuzzlePointSwitchSystem : ECSSystem {
        [OnEventFire]
        public void SelfSwitchMuzzlePoint(BeforeShotEvent e, WeaponMultyMuzzleNode weaponNode) {
            MuzzlePointComponent muzzlePoint = weaponNode.muzzlePoint;
            muzzlePoint.CurrentIndex = (muzzlePoint.CurrentIndex + 1) % muzzlePoint.Points.Length;
            ScheduleEvent(new MuzzlePointSwitchEvent(muzzlePoint.CurrentIndex), weaponNode);
        }

        [OnEventComplete]
        public void RemoteSwitchMuzzlePoint(RemoteMuzzlePointSwitchEvent e, WeaponMultyMuzzleNode weaponNode) {
            MuzzlePointComponent muzzlePoint = weaponNode.muzzlePoint;
            muzzlePoint.CurrentIndex = e.Index % muzzlePoint.Points.Length;
        }

        public class WeaponMultyMuzzleNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public TwinsComponent twins;
        }
    }
}