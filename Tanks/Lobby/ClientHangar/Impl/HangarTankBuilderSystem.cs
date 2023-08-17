using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientHangar.Impl.Builder;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarTankBuilderSystem : HangarTankBaseSystem {
        [OnEventFire]
        public void BuildTank(NodeAddedEvent e, HangarNode hangar, WeaponSkinItemPreviewLoadedNode weaponSkin,
            [Context] [JoinByParentGroup] WeaponItemPreviewNode weaponItem, HullSkinItemPreviewLoadedNode tankSkin,
            [JoinByParentGroup] [Context] TankItemPreviewNode tankItem, PaintItemPreviewLoadedNode paint,
            HangarCameraNode hangarCamera, SingleNode<SupplyEffectSettingsComponent> settings) {
            Transform transform = hangar.hangarTankPosition.transform;
            transform.DestroyChildren();
            GameObject gameObject = (GameObject)Object.Instantiate(tankSkin.resourceData.Data);
            gameObject.transform.SetParent(transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            tankSkin.hangarItemPreview.Instance = gameObject;
            NitroEffectComponent componentInChildren = gameObject.GetComponentInChildren<NitroEffectComponent>();
            componentInChildren.InitEffect(settings.component);
            Transform mountPoint = gameObject.GetComponent<MountPointComponent>().MountPoint;
            GameObject gameObject2 = (GameObject)Object.Instantiate(weaponSkin.resourceData.Data);
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.transform.localPosition = mountPoint.localPosition;
            gameObject2.transform.localRotation = mountPoint.localRotation;
            weaponSkin.hangarItemPreview.Instance = gameObject2;
            GameObject gameObject3 = (GameObject)Object.Instantiate(paint.resourceData.Data);
            gameObject3.transform.SetParent(gameObject.transform);
            PhysicsUtil.SetGameObjectLayer(transform.gameObject, Layers.HANGAR);
            ApplyPaint(gameObject, gameObject2, gameObject3);

            DoubleDamageEffectComponent componentInChildren2 =
                gameObject2.GetComponentInChildren<DoubleDamageEffectComponent>();

            componentInChildren2.InitEffect(settings.component);

            DoubleDamageSoundEffectComponent componentInChildren3 =
                gameObject2.GetComponentInChildren<DoubleDamageSoundEffectComponent>();

            componentInChildren3.RecalculatePlayingParameters();
            Renderer weaponRenderer = TankBuilderUtil.GetWeaponRenderer(gameObject2);
            Renderer hullRenderer = TankBuilderUtil.GetHullRenderer(gameObject);
            BurningTargetBloom component = hangarCamera.camera.Camera.GetComponent<BurningTargetBloom>();
            component.targets.Clear();
            component.targets.Add(weaponRenderer);
            component.targets.Add(hullRenderer);
            ScheduleEvent<HangarTankBuildedEvent>(hangar);
        }

        void ApplyPaint(GameObject tankInstance, GameObject weaponInstance, GameObject paintInstance) {
            TankActiveTextureBehaviourComponent
                component = paintInstance.GetComponent<TankActiveTextureBehaviourComponent>();

            Texture2D tankActiveTexture = component.tankActiveTexture;
            TankMaterialsUtil.SetColoringTexture(TankBuilderUtil.GetHullRenderer(tankInstance), tankActiveTexture);
            TankMaterialsUtil.SetColoringTexture(TankBuilderUtil.GetWeaponRenderer(weaponInstance), tankActiveTexture);
        }

        public class PaintItemPreviewNode : HangarPreviewItemNode {
            public PaintItemComponent paintItem;
        }

        public class PaintItemPreviewLoadedNode : PaintItemPreviewNode {
            public ResourceDataComponent resourceData;
        }
    }
}