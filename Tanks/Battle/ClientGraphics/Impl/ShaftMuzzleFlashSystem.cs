using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftMuzzleFlashSystem : AbstractMuzzleFlashSystem {
        [OnEventFire]
        public void CreateMuzzleFlashOnAnyRemoteShot(RemoteShotEvent evt, ShaftMuzzleFlashNode muzzle) =>
            InstantiateMuzzleEffect(muzzle.shaftMuzzleFlash.muzzleFlashPrefab,
                muzzle.muzzlePoint,
                muzzle.shaftMuzzleFlash.duration);

        [OnEventFire]
        public void CreateMuzzleFlashOnSelfQuickShot(ShotPrepareEvent evt, ShaftMuzzleFlashNode muzzle) =>
            InstantiateMuzzleEffect(muzzle.shaftMuzzleFlash.muzzleFlashPrefab,
                muzzle.muzzlePoint,
                muzzle.shaftMuzzleFlash.duration);

        public class ShaftMuzzleFlashNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftMuzzleFlashComponent shaftMuzzleFlash;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}