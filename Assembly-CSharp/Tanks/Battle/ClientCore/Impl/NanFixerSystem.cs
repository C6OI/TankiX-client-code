using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class NanFixerSystem : ECSSystem {
        [OnEventFire]
        public void FixNan1(FixedUpdateEvent e, SingleNode<HullInstanceComponent> tankNode) {
            FixNan(tankNode.component.HullInstance.gameObject);
        }

        [OnEventFire]
        public void FixNan2(AfterFixedUpdateEvent e, SingleNode<HullInstanceComponent> tankNode) {
            FixNan(tankNode.component.HullInstance.gameObject);
        }

        [OnEventFire]
        public void FixNan3(FixedUpdateEvent e, SingleNode<WeaponInstanceComponent> weaponNode) {
            FixNan(weaponNode.component.WeaponInstance.gameObject);
        }

        [OnEventFire]
        public void FixNan4(AfterFixedUpdateEvent e, SingleNode<WeaponInstanceComponent> weaponNode) {
            FixNan(weaponNode.component.WeaponInstance.gameObject);
        }

        static void FixNan(GameObject obj) {
            if (!(obj == null)) {
                NanFixer component = obj.GetComponent<NanFixer>();

                if (component != null) {
                    component.FixAndSave();
                }
            }
        }
    }
}