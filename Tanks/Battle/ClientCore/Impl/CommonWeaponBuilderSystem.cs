using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class CommonWeaponBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildWeapon(NodeAddedEvent evt, [Combine] WeaponSkinNode skin, [JoinByTank] [Context] WeaponNode weapon,
            SingleNode<MapInstanceComponent> map) =>
            weapon.Entity.AddComponent(new WeaponInstanceComponent(CreateInstance(skin.resourceData)));

        [OnEventFire]
        public void BuildShell(NodeAddedEvent evt, [Combine] ShellNode shell, SingleNode<MapInstanceComponent> map) =>
            NewEvent<BuildWeaponShellEvent>().Attach(shell).ScheduleDelayed(0.2f);

        [OnEventFire]
        public void BuildShell(BuildWeaponShellEvent e, ShellNode shell) =>
            shell.Entity.AddComponent(new ShellInstanceComponent(CreateInstance(shell.resourceData)));

        [OnEventFire]
        public void AssembleWeaponWithShell(NodeAddedEvent evt, WeaponInstanceNode weapon,
            [Context] [JoinByTank] ShellInstanceNode shell) {
            Transform transform = shell.shellInstance.ShellInstance.transform;
            GameObject weaponInstance = weapon.weaponInstance.WeaponInstance;

            WeaponVisualRootComponent componentInChildren =
                weaponInstance.GetComponentInChildren<WeaponVisualRootComponent>();

            transform.parent = componentInChildren.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            BuildWeaponEntity(weaponInstance, weapon.Entity);
        }

        void BuildWeaponEntity(GameObject weaponInstance, Entity weaponEntity) {
            EntityBehaviour component = weaponInstance.GetComponent<EntityBehaviour>();
            component.BuildEntity(weaponEntity);
            PhysicsUtil.SetGameObjectLayer(weaponInstance, Layers.INVISIBLE_PHYSICS);
        }

        GameObject CreateInstance(ResourceDataComponent resourceData) {
            GameObject gameObject = (GameObject)Object.Instantiate(resourceData.Data);
            gameObject.SetActive(false);
            return gameObject;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class WeaponInstanceNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
        }

        public class ReadySelfBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class WeaponSkinNode : Node {
            public ResourceDataComponent resourceData;

            public TankGroupComponent tankGroup;
            public WeaponSkinBattleItemComponent weaponSkinBattleItem;
        }

        public class ShellNode : Node {
            public ResourceDataComponent resourceData;
            public ShellBattleItemComponent shellBattleItem;
        }

        public class ShellInstanceNode : Node {
            public ShellInstanceComponent shellInstance;

            public TankGroupComponent tankGroup;
        }
    }
}