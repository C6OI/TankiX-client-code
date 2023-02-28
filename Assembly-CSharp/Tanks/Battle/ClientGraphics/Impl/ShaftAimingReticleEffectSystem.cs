using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingReticleEffectSystem : ECSSystem {
        const string RETICLE_SPOT_COLOR_PROPERTY = "_TintColor";

        [OnEventFire]
        public void InitEffect(NodeAddedEvent evt, ShaftAimingReticleEffectNode weapon, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode) {
            GameObject assetReticle = weapon.shaftAimingReticleEffect.AssetReticle;
            GameObject gameObject = Object.Instantiate(assetReticle);
            gameObject.transform.SetParent(canvasNode.component.transform, false);
            gameObject.transform.SetAsFirstSibling();
            weapon.shaftAimingReticleEffect.CanvasSize = canvasNode.component.selfRectTransform.rect.size;
            weapon.shaftAimingReticleEffect.InstanceReticle = gameObject;
            weapon.shaftAimingReticleEffect.ReticleAnimator = gameObject.GetComponent<Animator>();
            weapon.shaftAimingReticleEffect.DynamicReticle = gameObject.transform.Find("reticle").transform;
            ShaftReticleSpotBehaviour shaftReticleSpotBehaviour = gameObject.GetComponentsInChildren<ShaftReticleSpotBehaviour>(true)[0];
            shaftReticleSpotBehaviour.Init();
            weapon.shaftAimingReticleEffect.ShaftReticleSpotBehaviour = shaftReticleSpotBehaviour;
            weapon.shaftAimingReticleEffect.ReticleGroup = gameObject.GetComponent<CanvasGroup>();
            RawImage component = shaftReticleSpotBehaviour.gameObject.GetComponent<RawImage>();
            Material material = Object.Instantiate(component.material);
            weapon.shaftAimingReticleEffect.ReticleSpotMaterialInstance = material;
            component.material = material;
            component.material.SetColor("_TintColor", weapon.shaftAimingColorEffect.ChoosenColor);
            weapon.Entity.AddComponent<ShaftAimingReticleReadyComponent>();
        }

        [OnEventFire]
        public void FinalizeEffect(NodeRemoveEvent evt, ShaftAimingReadyReticleForNRNode nr, [JoinSelf] ShaftAimingReadyReticleNode weapon) {
            weapon.shaftAimingReticleEffect.ReticleSpotMaterialInstance = null;
            Object.Destroy(weapon.shaftAimingReticleEffect.InstanceReticle);
            weapon.Entity.RemoveComponent<ShaftAimingReticleReadyComponent>();
        }

        [OnEventFire]
        public void LaunchEffect(NodeAddedEvent evt, ShaftAimingWorkActivationReticleEffectNode weapon) {
            ShaftAimingReticleEffectComponent shaftAimingReticleEffect = weapon.shaftAimingReticleEffect;
            UpdateImageAlpha(0f, shaftAimingReticleEffect);
            shaftAimingReticleEffect.InstanceReticle.SetActive(true);
            shaftAimingReticleEffect.ReticleAnimator.SetFloat("Time", weapon.shaftEnergy.UnloadAimingEnergyPerSec);
        }

        [OnEventFire]
        public void UpdateEffectAlpha(UpdateEvent evt, ShaftAimingWorkActivationReticleEffectNode weapon) {
            ShaftAimingReticleEffectComponent shaftAimingReticleEffect = weapon.shaftAimingReticleEffect;
            float a = weapon.shaftAimingWorkActivationState.ActivationTimer / weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec;
            UpdateImageAlpha(a, shaftAimingReticleEffect);
            Vector3 barrelOriginWorld = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();
            float z = weapon.muzzlePoint.Current.transform.localPosition.z;
            Vector3 vector = weapon.weaponInstance.WeaponInstance.transform.forward * z;
            Vector2 canvasSize = weapon.shaftAimingReticleEffect.CanvasSize;
            Vector2 vector2 = Camera.main.WorldToScreenPoint(barrelOriginWorld + vector);
            Vector2 vector3 = new(vector2.x / Screen.width * canvasSize.x, vector2.y / Screen.height * canvasSize.y);
            weapon.shaftAimingReticleEffect.DynamicReticle.localPosition = vector3 - canvasSize / 2f;
        }

        [OnEventFire]
        public void UpdateReticleSpotScale(WeaponRotationUpdateAimEvent e, ShaftAimingWorkingReticleEffectNode weapon) {
            bool isInsideTankPart = weapon.shaftAimingTargetPointContainer.IsInsideTankPart;
            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            Vector3 barrelOriginWorld = muzzleLogicAccessor.GetBarrelOriginWorld();
            Vector3 point = weapon.shaftAimingTargetPointContainer.Point;
            float distance = Vector3.Magnitude(point - barrelOriginWorld);
            Vector3 barrelOriginWorld2 = muzzleLogicAccessor.GetBarrelOriginWorld();
            float z = weapon.muzzlePoint.Current.transform.localPosition.z;
            Vector3 vector = weapon.shaftAimingWorkingState.WorkingDirection * z;
            Vector2 canvasSize = weapon.shaftAimingReticleEffect.CanvasSize;
            Vector2 vector2 = Camera.main.WorldToScreenPoint(barrelOriginWorld2 + vector);
            Vector2 vector3 = new(vector2.x / Screen.width * canvasSize.x, vector2.y / Screen.height * canvasSize.y);
            weapon.shaftAimingReticleEffect.DynamicReticle.localPosition = vector3 - canvasSize / 2f;
            weapon.shaftAimingReticleEffect.ShaftReticleSpotBehaviour.SetSizeByDistance(distance, isInsideTankPart);
        }

        [OnEventFire]
        public void SetEffectOpaque(NodeAddedEvent evt, ShaftAimingWorkingReticleEffectNode weapon) {
            ShaftAimingReticleEffectComponent shaftAimingReticleEffect = weapon.shaftAimingReticleEffect;
            UpdateImageAlpha(1f, shaftAimingReticleEffect);
        }

        [OnEventFire]
        public void StopEffect(NodeAddedEvent evt, ShaftAimingIdleReticleEffectNode weapon) {
            weapon.shaftAimingReticleEffect.ShaftReticleSpotBehaviour.SetDefaultSize();
            weapon.shaftAimingReticleEffect.InstanceReticle.SetActive(false);
        }

        void UpdateImageAlpha(float a, ShaftAimingReticleEffectComponent effect) {
            effect.ReticleGroup.alpha = a;
        }

        public class ShaftAimingReticleEffectNode : Node {
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;

            public ShaftAimingColorEffectPreparedComponent shaftAimingColorEffectPrepared;
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;

            public ShaftStateControllerComponent shaftStateController;
        }

        public class ShaftAimingWorkActivationReticleEffectNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;

            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public ShaftEnergyComponent shaftEnergy;

            public ShaftStateConfigComponent shaftStateConfig;

            public ShaftStateControllerComponent shaftStateController;

            public WeaponInstanceComponent weaponInstance;
        }

        public class ShaftAimingWorkingReticleEffectNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;

            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;

            public ShaftAimingTargetPointContainerComponent shaftAimingTargetPointContainer;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public ShaftStateControllerComponent shaftStateController;

            public WeaponInstanceComponent weaponInstance;
        }

        public class ShaftAimingIdleReticleEffectNode : Node {
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;

            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;
            public ShaftIdleStateComponent shaftIdleState;

            public ShaftStateControllerComponent shaftStateController;
        }

        public class ShaftAimingReadyReticleNode : Node {
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;

            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;

            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingReadyReticleForNRNode : Node {
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;

            public TankGroupComponent tankGroup;
        }
    }
}