using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MuzzlePointGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void AttachMuzzlePointToVisualRootForRemoteTank(NodeAddedEvent e, MuzzlePointInitNode weaponNode) {
            MuzzlePointComponent muzzlePoint = weaponNode.muzzlePoint;
            Transform transform = weaponNode.weaponVisualRoot.transform;
            Transform[] points = muzzlePoint.Points;

            foreach (Transform transform2 in points) {
                transform2.parent = transform;
            }
        }

        public class MuzzlePointInitNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public MuzzlePointInitializedComponent muzzlePointInitialized;

            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}