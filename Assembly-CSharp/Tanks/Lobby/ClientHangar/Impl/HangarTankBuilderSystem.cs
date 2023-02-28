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
using UnityEngine.Rendering;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarTankBuilderSystem : HangarTankBaseSystem {
        [OnEventFire]
        public void BuildTank(NodeAddedEvent e, HangarNode hangar, WeaponSkinItemPreviewLoadedNode weaponSkin, [Context] [JoinByParentGroup] WeaponItemPreviewNode weaponItem,
            HullSkinItemPreviewLoadedNode tankSkin, [Context] [JoinByParentGroup] TankItemPreviewNode tankItem, TankPaintItemPreviewLoadedNode tankPaint,
            WeaponPaintItemPreviewLoadedNode weaponPaint, HangarCameraNode hangarCamera, SingleNode<SupplyEffectSettingsComponent> settings) {
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
            GameObject gameObject3 = (GameObject)Object.Instantiate(tankPaint.resourceData.Data);
            gameObject3.transform.SetParent(gameObject.transform);
            gameObject3.transform.localPosition = Vector3.zero;
            GameObject gameObject4 = (GameObject)Object.Instantiate(weaponPaint.resourceData.Data);
            gameObject4.transform.SetParent(gameObject.transform);
            gameObject4.transform.localPosition = Vector3.zero;
            PhysicsUtil.SetGameObjectLayer(transform.gameObject, Layers.STATIC);
            ApplyPaint(gameObject, gameObject2, gameObject3, gameObject4);
            DoubleDamageEffectComponent componentInChildren2 = gameObject2.GetComponentInChildren<DoubleDamageEffectComponent>();
            componentInChildren2.InitEffect(settings.component);
            DoubleDamageSoundEffectComponent componentInChildren3 = gameObject2.GetComponentInChildren<DoubleDamageSoundEffectComponent>();
            componentInChildren3.RecalculatePlayingParameters();
            Renderer weaponRenderer = TankBuilderUtil.GetWeaponRenderer(gameObject2);
            weaponRenderer.tag = "TankWeapon";
            Renderer hullRenderer = TankBuilderUtil.GetHullRenderer(gameObject);
            hullRenderer.tag = "TankHull";
            weaponRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            weaponRenderer.lightProbeUsage = LightProbeUsage.Off;
            hullRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            hullRenderer.lightProbeUsage = LightProbeUsage.Off;
            BurningTargetBloom componentInChildren4 = hangarCamera.cameraRootTransform.Root.GetComponentInChildren<BurningTargetBloom>();
            componentInChildren4.targets.Clear();
            componentInChildren4.targets.Add(weaponRenderer);
            componentInChildren4.targets.Add(hullRenderer);
            ScheduleEvent<HangarTankBuildedEvent>(hangar);
        }

        void ApplyPaint(GameObject tankInstance, GameObject weaponInstance, GameObject tankPaintInstance, GameObject weaponPaintInstance) {
            TankMaterialsUtil.ApplyColoring(TankBuilderUtil.GetHullRenderer(tankInstance), TankBuilderUtil.GetWeaponRenderer(weaponInstance),
                tankPaintInstance.GetComponent<ColoringComponent>(), weaponPaintInstance.GetComponent<ColoringComponent>());
        }

        public class TankPaintItemPreviewNode : HangarPreviewItemNode {
            public TankPaintItemComponent tankPaintItem;
        }

        public class TankPaintItemPreviewLoadedNode : TankPaintItemPreviewNode {
            public ResourceDataComponent resourceData;
        }

        public class WeaponPaintItemPreviewNode : HangarPreviewItemNode {
            public WeaponPaintItemComponent weaponPaintItem;
        }

        public class WeaponPaintItemPreviewLoadedNode : WeaponPaintItemPreviewNode {
            public ResourceDataComponent resourceData;
        }
    }
}