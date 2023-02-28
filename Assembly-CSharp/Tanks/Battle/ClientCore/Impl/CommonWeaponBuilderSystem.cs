using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class CommonWeaponBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildWeapon(NodeAddedEvent evt, [Combine] WeaponSkinNode skin, [Context] [JoinByTank] WeaponNode weapon, SingleNode<MapInstanceComponent> map) {
            WeaponInstanceIsReadyEvent weaponInstanceIsReadyEvent = new();
            weaponInstanceIsReadyEvent.WeaponInstance = CreateInstance(skin.resourceData);
            WeaponInstanceIsReadyEvent eventInstance = weaponInstanceIsReadyEvent;
            NewEvent(eventInstance).Attach(weapon).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void BuildWeapon(WeaponInstanceIsReadyEvent evt, WeaponNode weapon) {
            GameObject weaponInstance = evt.WeaponInstance;
            weapon.Entity.AddComponent(new WeaponInstanceComponent(weaponInstance));
            weaponInstance.AddComponent<NanFixer>().Init(null, weaponInstance.transform, weapon.Entity.GetComponent<UserGroupComponent>().Key);
        }

        [OnEventFire]
        public void BuildShell(NodeAddedEvent evt, [Combine] ShellNode shell, SingleNode<MapInstanceComponent> map) {
            NewEvent<BuildWeaponShellEvent>().Attach(shell).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void BuildShell(BuildWeaponShellEvent e, ShellNode shell) {
            GameObject gameObject = CreateInstance(shell.resourceData);
            gameObject.SetActive(true);

            if (shell.Entity.HasComponent<ShellInstanceComponent>()) {
                shell.Entity.GetComponent<ShellInstanceComponent>().ShellInstance = gameObject;
            } else {
                shell.Entity.AddComponent(new ShellInstanceComponent(gameObject));
            }
        }

        [OnEventFire]
        public void AssembleWeaponWithShell(NodeAddedEvent evt, WeaponInstanceNode weapon, [Context] [JoinByTank] ShellInstanceNode shell) {
            Transform transform = shell.shellInstance.ShellInstance.transform;
            GameObject weaponInstance = weapon.weaponInstance.WeaponInstance;
            WeaponVisualRootComponent componentInChildren = weaponInstance.GetComponentInChildren<WeaponVisualRootComponent>();
            transform.parent = componentInChildren.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            BuildWeaponEntity(weaponInstance, weapon.Entity);
            weapon.Entity.AddComponent<WeaponPreparedByEntityBehaviourComponent>();
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

        public class WeaponInstanceIsReadyEvent : Event {
            public GameObject WeaponInstance;
        }
    }
}