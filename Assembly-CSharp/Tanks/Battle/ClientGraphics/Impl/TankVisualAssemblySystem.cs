using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankVisualAssemblySystem : ECSSystem {
        [OnEventFire]
        public void OnTankPartsPrepared(NodeAddedEvent e, AssembledTankNode tank, [JoinAll] SingleNode<CameraRootTransformComponent> cameraNode) {
            Transform transform = tank.assembledTank.AssemblyRoot.transform;
            Transform transform2 = tank.tankVisualRoot.transform;
            transform2.parent = transform;
            CameraVisibleTriggerComponent cameraVisibleTriggerComponent = tank.trackRenderer.Renderer.gameObject.AddComponent<CameraVisibleTriggerComponent>();
            tank.Entity.AddComponent(cameraVisibleTriggerComponent);
            cameraVisibleTriggerComponent.MainCameraTransform = cameraNode.component.Root;
        }

        [OnEventFire]
        public void LocateWeaponVisualRootUnderMountPoint(NodeAddedEvent evt, TankGraphicsNode tank, [Context] [JoinByTank] WeaponGraphicsNode weaponGraphics) {
            WeaponVisualRootComponent weaponVisualRoot = weaponGraphics.weaponVisualRoot;
            weaponVisualRoot.transform.parent = tank.tankVisualRoot.transform;
            Transform mountPoint = tank.mountPoint.MountPoint;
            GameObject gameObject = new("VisualMountPoint");
            gameObject.transform.SetParent(tank.tankVisualRoot.transform, false);
            gameObject.transform.localPosition = mountPoint.localPosition;
            gameObject.transform.localRotation = mountPoint.localRotation;

            tank.Entity.AddComponent(new VisualMountPointComponent {
                MountPoint = gameObject.transform
            });

            weaponVisualRoot.transform.SetParent(gameObject.transform, false);
            weaponVisualRoot.transform.localPosition = Vector3.zero;
            weaponVisualRoot.transform.localRotation = Quaternion.identity;
            InitCharacterShadowSystem(tank.Entity, tank.tankVisualRoot.transform, weaponGraphics.weaponVisualRoot.transform);
            NewEvent<ContinueAssembleTankEvent>().Attach(tank).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void ContinueAssebleTank(ContinueAssembleTankEvent e, TankGraphicsNode tank) {
            tank.Entity.AddComponent<AssembledTankInactiveStateComponent>();
        }

        void InitCharacterShadowSystem(Entity tankEntity, Transform tankVisualRoot, Transform weaponVisualRoot) {
            CharacterShadowCastersComponent characterShadowCastersComponent = new();
            characterShadowCastersComponent.Casters = new Transform[2] { tankVisualRoot, weaponVisualRoot };
            CharacterShadowCastersComponent component = characterShadowCastersComponent;
            tankEntity.AddComponent(component);
        }

        public class AssembledTankNode : Node {
            public AssembledTankComponent assembledTank;

            public TankVisualRootComponent tankVisualRoot;

            public TrackRendererComponent trackRenderer;
        }

        public class WeaponGraphicsNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class TankGraphicsNode : Node {
            public AssembledTankComponent assembledTank;

            public MountPointComponent mountPoint;

            public TankGroupComponent tankGroup;

            public TankVisualRootComponent tankVisualRoot;
        }
    }
}