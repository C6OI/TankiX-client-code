using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankAssemblySystem : ECSSystem {
        [OnEventComplete]
        public void AssembleTank(NodeAddedEvent e, TankNode tank, [Context] [JoinByTank] WeaponNode weapon) {
            GameObject gameObject = new("Tank");
            Transform transform = weapon.weaponInstance.WeaponInstance.transform;
            Transform mountPoint = tank.mountPoint.MountPoint;
            Transform transform3 = transform.parent = tank.hullInstance.HullInstance.transform;
            UnityUtil.InheritAndEmplace(tank.tankCommonInstance.TankCommonInstance.transform, transform3);
            tank.paintInstance.PaintInstance.transform.parent = transform3;
            transform.localPosition = mountPoint.localPosition;
            transform.localRotation = mountPoint.localRotation;
            transform3.parent = gameObject.transform;
            gameObject.SetActive(false);
            tank.Entity.AddComponent(new AssembledTankComponent(gameObject));
        }

        [OnEventComplete]
        public void ActivateAssembledTank(NodeAddedEvent e, AssembledActivationTankNode tank,
            [JoinByTank] WeaponNode weapon) {
            tank.assembledTank.AssemblyRoot.SetActive(true);
            tank.hullInstance.HullInstance.SetActive(true);
            weapon.weaponInstance.WeaponInstance.SetActive(true);
            tank.assembledTank.AssemblyRoot.gameObject.AddComponent<TargetBehaviour>().Entity = tank.Entity;
            tank.Entity.RemoveComponent<AssembledTankInactiveStateComponent>();
            tank.Entity.AddComponent<AssembledTankActivatedStateComponent>();
        }

        [OnEventComplete]
        public void DestroyTank(NodeRemoveEvent e, AssembledTankNode tank) {
            UnityUtil.Destroy(tank.hullInstance.HullInstance);
            UnityUtil.Destroy(tank.assembledTank.AssemblyRoot);
        }

        [OnEventComplete]
        public void DestroyTankHullIfTankNotAssembled(NodeRemoveEvent e, SingleNode<HullInstanceComponent> tank) {
            if (!tank.Entity.HasComponent<AssembledTankComponent>()) {
                GameObject hullInstance = tank.component.HullInstance;
                UnityUtil.Destroy(hullInstance);
            }
        }

        [OnEventComplete]
        public void DestroyTankWeaponIfTankNotAssembled(NodeRemoveEvent e, SingleNode<HullInstanceComponent> tank,
            [JoinByUser] SingleNode<WeaponInstanceComponent> weapon) {
            if (!tank.Entity.HasComponent<AssembledTankComponent>()) {
                GameObject weaponInstance = weapon.component.WeaponInstance;
                Object.Destroy(weaponInstance);
            }
        }

        public class TankNode : Node {
            public HullInstanceComponent hullInstance;

            public MountPointComponent mountPoint;

            public PaintInstanceComponent paintInstance;

            public TankCommonInstanceComponent tankCommonInstance;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;
        }

        public class AssembledActivationTankNode : Node {
            public AssembledTankComponent assembledTank;

            public AssembledTankInactiveStateComponent assembledTankInactiveState;

            public HullInstanceComponent hullInstance;

            public MountPointComponent mountPoint;

            public PaintInstanceComponent paintInstance;

            public TankCommonInstanceComponent tankCommonInstance;
            public TankGroupComponent tankGroup;
        }

        public class AssembledTankNode : Node {
            public AssembledTankComponent assembledTank;

            public HullInstanceComponent hullInstance;
            public TankGroupComponent tankGroup;
        }
    }
}