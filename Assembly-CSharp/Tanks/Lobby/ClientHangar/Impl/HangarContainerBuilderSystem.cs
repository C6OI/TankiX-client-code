using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientHangar.Impl.Builder;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarContainerBuilderSystem : HangarTankBaseSystem {
        [OnEventFire]
        public void BuildContainer(NodeAddedEvent e, HangarNode hangar, ContainerItemPreviewLoadedNode container, HangarCameraNode hangarCamera, SingleNode<MainScreenComponent> screen) {
            screen.component.HideNewItemNotification();
            Transform transform = hangar.hangarContainerPosition.transform;
            ContainerComponent componentInChildren = hangar.hangarContainerPosition.GetComponentInChildren<ContainerComponent>();

            if (componentInChildren != null && componentInChildren.assetGuid == container.assetReference.Reference.AssetGuid) {
                ScheduleEvent<HangarContainerBuildedEvent>(hangar);
                return;
            }

            BuildContainer(transform, container.resourceData.Data, hangarCamera.cameraRootTransform.Root);
            transform.GetComponentInChildren<ContainerComponent>().assetGuid = container.assetReference.Reference.AssetGuid;
            ScheduleEvent<HangarContainerBuildedEvent>(hangar);
        }

        void BuildContainer(Transform containerPosition, Object containerPrefab, Transform hangarCamera) {
            containerPosition.DestroyChildren();
            GameObject gameObject = (GameObject)Object.Instantiate(containerPrefab);
            gameObject.transform.SetParent(containerPosition.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            PhysicsUtil.SetGameObjectLayer(containerPosition.gameObject, Layers.HANGAR);
            Renderer containerRenderer = TankBuilderUtil.GetContainerRenderer(gameObject);
            BurningTargetBloom componentInChildren = hangarCamera.GetComponentInChildren<BurningTargetBloom>();
            componentInChildren.targets.Clear();
            componentInChildren.targets.Add(containerRenderer);
        }

        [OnEventFire]
        public void DestroyHangarContainer(NodeRemoveEvent e, ContainersScreenNode screen, [JoinAll] ICollection<SingleNode<ContainerMarkerComponent>> containers,
            [JoinAll] SingleNode<HangarContainerPositionComponent> containerPosition) {
            if (containers.Count == 0) {
                containerPosition.component.transform.DestroyChildren();
            }
        }

        [Not(typeof(ResourceDataComponent))]
        [Not(typeof(AssetRequestComponent))]
        public class NotLoadedContainerItemNode : Node {
            public AssetReferenceComponent assetReference;
            public ContainerMarkerComponent containerMarker;
        }

        public class ContainerItemPreviewLoadedNode : HangarPreviewItemNode {
            public AssetReferenceComponent assetReference;
            public ContainerMarkerComponent containerMarker;

            public ResourceDataComponent resourceData;
        }

        public class ContainersScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ContainersScreenComponent containersScreen;
        }
    }
}