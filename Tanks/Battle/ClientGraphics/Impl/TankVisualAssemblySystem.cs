using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankVisualAssemblySystem : ECSSystem {
        [OnEventFire]
        public void OnTankPartsPrepared(NodeAddedEvent e, AssembledTankNode tank,
            [JoinAll] SingleNode<CameraComponent> cameraNode) {
            Transform transform = tank.assembledTank.AssemblyRoot.transform;
            Transform transform2 = tank.tankVisualRoot.transform;
            transform2.parent = transform;

            CameraVisibleTriggerComponent cameraVisibleTriggerComponent =
                tank.trackRenderer.Renderer.gameObject.AddComponent<CameraVisibleTriggerComponent>();

            tank.Entity.AddComponent(cameraVisibleTriggerComponent);
            cameraVisibleTriggerComponent.MainCamera = cameraNode.component.Camera;
        }

        [OnEventFire]
        public void LocateWeaponVisualRootUnderMountPoint(NodeAddedEvent evt, TankGraphicsNode tank,
            [Context] [JoinByTank] WeaponGraphicsNode weaponGraphics) {
            WeaponVisualRootComponent weaponVisualRoot = weaponGraphics.weaponVisualRoot;
            weaponVisualRoot.transform.parent = tank.tankVisualRoot.transform;
            Transform mountPoint = tank.mountPoint.MountPoint;
            weaponVisualRoot.transform.localPosition = mountPoint.localPosition;
            weaponVisualRoot.transform.localRotation = mountPoint.localRotation;
            InitCharacterShadowSystem(tank.Entity, tank.tankVisualRoot.transform, weaponGraphics.weaponVisualRoot.transform);
            NewEvent<ContinueAssembleTankEvent>().Attach(tank).ScheduleDelayed(0.1f);
        }

        [OnEventFire]
        public void ContinueAssebleTank(ContinueAssembleTankEvent e, TankGraphicsNode tank) =>
            tank.Entity.AddComponent<AssembledTankInactiveStateComponent>();

        void InitCharacterShadowSystem(Entity tankEntity, Transform tankVisualRoot, Transform weaponVisualRoot) {
            CharacterShadowCastersComponent characterShadowCastersComponent = new();
            characterShadowCastersComponent.Casters = new Transform[2] { tankVisualRoot, weaponVisualRoot };
            tankEntity.AddComponent(characterShadowCastersComponent);
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