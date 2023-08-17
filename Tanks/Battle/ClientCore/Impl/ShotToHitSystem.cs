using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShotToHitSystem : ECSSystem {
        [OnEventFire]
        public void GenerateShotId(BeforeShotEvent e, SingleNode<ShotIdComponent> shotId) => shotId.component.NextShotId();

        [OnEventFire]
        public void SetShotIdToBaseShotEvent(BaseShotEvent e, SingleNode<ShotIdComponent> shotId,
            [JoinByTank] SingleNode<SelfTankComponent> selfTank) => e.ShotId = shotId.component.ShotId;

        [OnEventFire]
        public void SetShotIdToBaseHitEvent(HitEvent e, NotBulletWeaponNode weapon,
            [JoinByTank] SingleNode<SelfTankComponent> selfTank) => e.ShotId = weapon.shotId.ShotId;

        [Not(typeof(TwinsComponent))]
        [Not(typeof(RicochetComponent))]
        public class NotBulletWeaponNode : Node {
            public ShotIdComponent shotId;
        }
    }
}