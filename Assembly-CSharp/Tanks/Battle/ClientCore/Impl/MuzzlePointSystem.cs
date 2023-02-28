using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MuzzlePointSystem : ECSSystem {
        public const string MUZZLE_POINT_NAME = "muzzle_point";

        [OnEventFire]
        public void CreateMuzzlePoint(NodeAddedEvent e, SingleNode<WeaponVisualRootComponent> weaponVisualNode) {
            List<Transform> list = new();
            Transform transform = weaponVisualNode.component.transform;

            if (transform.name == "muzzle_point") {
                list.Add(transform);
            }

            IEnumerator enumerator = transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform2 = (Transform)enumerator.Current;

                    if (transform2.name == "muzzle_point") {
                        list.Add(transform2);
                    }
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }

            MuzzlePointComponent muzzlePointComponent = new();
            muzzlePointComponent.Points = list.ToArray();
            MuzzlePointComponent component = muzzlePointComponent;
            weaponVisualNode.Entity.AddComponent(component);
            weaponVisualNode.Entity.AddComponent<MuzzlePointInitializedComponent>();
        }
    }
}