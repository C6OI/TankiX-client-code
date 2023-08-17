using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MuzzlePointSystem : ECSSystem {
        const string MUZZLE_POINT_NAME = "muzzle_point";

        Vector3 mpPosition = Vector3.zero;

        [OnEventFire]
        public void CreateMuzzlePoint(NodeAddedEvent e, SingleNode<WeaponVisualRootComponent> weaponVisualNode) {
            List<Transform> list = new();
            Transform transform = weaponVisualNode.component.transform;

            foreach (Transform item in transform) {
                if (item.name == "muzzle_point") {
                    list.Add(item);
                }
            }

            MuzzlePointComponent muzzlePointComponent = new();
            muzzlePointComponent.Points = list.ToArray();
            weaponVisualNode.Entity.AddComponent(muzzlePointComponent);
            weaponVisualNode.Entity.AddComponent<MuzzlePointInitializedComponent>();
        }
    }
}