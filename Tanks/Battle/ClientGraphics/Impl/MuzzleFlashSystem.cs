using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MuzzleFlashSystem : AbstractMuzzleFlashSystem {
        [OnEventFire]
        public void PlayMuzzleFlash(BaseShotEvent evt, MuzzleFlashNode muzzle) =>
            InstantiateMuzzleEffect(muzzle.muzzleFlash.muzzleFlashPrefab, muzzle.muzzlePoint, muzzle.muzzleFlash.duration);

        public class MuzzleFlashNode : Node {
            public MuzzleFlashComponent muzzleFlash;
            public MuzzlePointComponent muzzlePoint;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}