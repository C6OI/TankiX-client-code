using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SelfTargetHitFeedbackHUDSystem : ECSSystem {
        const float VISIBILITY_RANGE = 50f;

        const float EFFECT_INTERVAL = 0.5f;

        const float SIN_45_DEGREE = 0.70710677f;

        const bool MODIFY_HIT_DIRECTION = true;

        const string EFFECT_INSTANCE_NAME = "SelfTargetHitHUDEffectInstance";

        [OnEventFire]
        public void InitEnemyWeapon(NodeAddedEvent e, [Combine] WeaponNode weapon, [JoinByUser] [Context] RemoteTankNode remoteTank, SingleNode<GameTankSettingsComponent> settings) {
            if (settings.component.SelfTargetHitFeedbackEnabled) {
                weapon.Entity.AddComponent(new WeaponSelfTargetHitFeedbackTimerComponent(Time.time - 0.5f));
                weapon.Entity.CreateGroup<WeaponSelfTargetHitFeedbackGroupComponent>();
            }
        }

        [OnEventFire]
        public void DetachHUDEffectInstance(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] [Combine] SingleNode<SelfTargetHitFeedbackHUDInstanceComponent> effect) {
            tankIncarnation.tankGroup.Detach(effect.Entity);
        }

        [OnEventFire]
        public void DetachHUDEffectInstance(NodeRemoveEvent e, ReadyWeaponNode enemyWeapon,
            [JoinBy(typeof(WeaponSelfTargetHitFeedbackGroupComponent))] [Combine]
            SingleNode<SelfTargetHitFeedbackHUDInstanceComponent> effect) {
            enemyWeapon.weaponSelfTargetHitFeedbackGroup.Detach(effect.Entity);
        }

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, NotStreamWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank,
            [JoinByTank] NotShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode) {
            CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, false);
        }

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, NotStreamWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank,
            [JoinByTank] ShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode) {
            CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, true);
        }

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, StreamWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank,
            [JoinByTank] ShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode) {
            CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, true);
        }

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, StreamHitWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank,
            [JoinByTank] ShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode) {
            CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, true);
        }

        [OnEventFire]
        public void UpdateSelfTargetHitEffect(UpdateEvent e, EffectInstanceFullNode effect, [JoinAll] SelfTankNode selfTank, [JoinByTank] NotShaftAimingWeaponNode selfWeapon,
            [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode) {
            UpdateSelfTargetHitEffect(effect, selfTank, camera, canvasNode, false);
        }

        [OnEventFire]
        public void UpdateSelfTargetHitEffect(UpdateEvent e, EffectInstanceFullNode effect, [JoinAll] SelfTankNode selfTank, [JoinByTank] ShaftAimingWeaponNode selfWeapon,
            [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode) {
            UpdateSelfTargetHitEffect(effect, selfTank, camera, canvasNode, true);
        }

        void UpdateSelfTargetHitEffect(EffectInstanceFullNode effect, SelfTankNode selfTank, BattleCameraNode camera, SingleNode<ScreensLayerComponent> canvasNode, bool isShaftAiming) {
            SelfTargetHitEffectHUDData? dataForSelfTargetHitEffect =
                GetDataForSelfTargetHitEffect(effect.selfTargetHitFeedbackHUDInstance.InitialData.EnemyWeaponWorldSpace, selfTank, camera, canvasNode, isShaftAiming);

            if (dataForSelfTargetHitEffect.HasValue) {
                effect.selfTargetHitFeedbackHUDInstance.UpdateTransform(dataForSelfTargetHitEffect.Value);
            }
        }

        void CreateSelfTargetHitHUDFeedback(ReadyWeaponNode enemyWeapon, RemoteTankNode remoteTank, SelfTankNode selfTank, BattleCameraNode camera,
            SingleNode<ScreensLayerComponent> canvasNode, bool isShaft) {
            if (CheckPossibilityForEffectInstancing(enemyWeapon)) {
                SelfTargetHitEffectHUDData? dataForSelfTargetHitEffect = GetDataForSelfTargetHitEffect(enemyWeapon, selfTank, camera, canvasNode, isShaft);

                if (dataForSelfTargetHitEffect.HasValue) {
                    SelfTargetHitEffectHUDData value = dataForSelfTargetHitEffect.Value;
                    enemyWeapon.weaponSelfTargetHitFeedbackTimer.LastTime = Time.time;

                    SelfTargetHitFeedbackHUDInstanceComponent selfTargetHitFeedbackHUDInstanceComponent =
                        Object.Instantiate(selfTank.selfTargetHitFeedbackHUDConfig.EffectPrefab, canvasNode.component.transform);

                    Entity entity = CreateEntity("SelfTargetHitHUDEffectInstance");
                    remoteTank.tankGroup.Attach(entity);
                    enemyWeapon.weaponSelfTargetHitFeedbackGroup.Attach(entity);
                    selfTargetHitFeedbackHUDInstanceComponent.Init(entity, value);
                }
            }
        }

        SelfTargetHitEffectHUDData? GetDataForSelfTargetHitEffect(ReadyWeaponNode enemyWeapon, SelfTankNode selfTank, BattleCameraNode camera,
            SingleNode<ScreensLayerComponent> canvasNode, bool isShaft) =>
            GetDataForSelfTargetHitEffect(enemyWeapon.weaponVisualRoot.transform.position, selfTank, camera, canvasNode, isShaft);

        SelfTargetHitEffectHUDData? GetDataForSelfTargetHitEffect(Vector3 enemyWeaponWorldPosition, SelfTankNode selfTank, BattleCameraNode camera,
            SingleNode<ScreensLayerComponent> canvasNode, bool isShaft) {
            Camera unityCamera = camera.camera.UnityCamera;
            bool behindCameraForwardPlane;
            Vector2 vector = WorldToViewportPointProjected(unityCamera, enemyWeaponWorldPosition, out behindCameraForwardPlane);
            Vector2 vector2 = new(0.5f, 0.5f);
            Vector2 vector3 = vector2 - vector;

            if (isShaft) {
                if (behindCameraForwardPlane) {
                    vector3.y = Mathf.Abs(vector3.y);
                } else {
                    vector3.y = 0f - Mathf.Abs(vector3.y);
                }

                vector = vector2 - vector3;
            }

            Vector2? boundPosition = selfTank.selfTargetHitFeedbackHUDConfig.GetBoundPosition(vector2, vector3);

            if (!boundPosition.HasValue) {
                return null;
            }

            Vector2 value = boundPosition.Value;
            Vector2 localPositionForCanvasByViewport = GetLocalPositionForCanvasByViewport(value, canvasNode);
            Vector2 vector4 = GetLocalPositionForCanvasByViewport(vector2, canvasNode) - localPositionForCanvasByViewport;
            float magnitude = new Vector3(vector4.x, vector4.y, 0f).magnitude;
            Vector3 normalized = new Vector3(vector4.x, vector4.y, 0f).normalized;

            return new SelfTargetHitEffectHUDData(enemyWeaponWorldPosition, value, localPositionForCanvasByViewport, normalized, canvasNode.component.selfRectTransform.sizeDelta,
                magnitude);
        }

        static Vector2 WorldToViewportPointProjected(Camera camera, Vector3 worldPos, out bool behindCameraForwardPlane) {
            Vector3 forward = camera.transform.forward;
            Vector3 vector = worldPos - camera.transform.position;
            float num = Vector3.Dot(forward, vector);
            behindCameraForwardPlane = num <= 0f;

            if (behindCameraForwardPlane) {
                Vector3 vector2 = forward * num * 1.01f;
                worldPos = camera.transform.position + (vector - vector2);
            }

            Vector3 vector3 = camera.WorldToViewportPoint(worldPos);
            return vector3;
        }

        static bool CheckPossibilityForEffectInstancing(ReadyWeaponNode weapon) {
            if (Time.time - weapon.weaponSelfTargetHitFeedbackTimer.LastTime < 0.5f) {
                return false;
            }

            if (weapon.cameraVisibleTrigger.IsVisibleAtRange(50f)) {
                return false;
            }

            return true;
        }

        static Vector2 GetLocalPositionForCanvasByViewport(Vector2 viewportPos, SingleNode<ScreensLayerComponent> canvasNode) {
            RectTransform selfRectTransform = canvasNode.component.selfRectTransform;
            Vector2 vector = new(selfRectTransform.sizeDelta.x / 2f, selfRectTransform.sizeDelta.y / 2f);
            Vector2 vector2 = new(viewportPos.x * selfRectTransform.sizeDelta.x, viewportPos.y * selfRectTransform.sizeDelta.y);
            return vector2 - vector;
        }

        [OnEventFire]
        public void RemoveEffect(NodeRemoveEvent e, SingleNode<SelfTargetHitFeedbackHUDInstanceComponent> node) {
            Object.DestroyObject(node.component.gameObject);
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public TankVisualRootComponent tankVisualRoot;

            public UserGroupComponent userGroup;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : TankNode {
            public SelfTankComponent selfTank;

            public SelfTargetHitFeedbackHUDConfigComponent selfTargetHitFeedbackHUDConfig;
        }

        public class WeaponNode : Node {
            public CameraVisibleTriggerComponent cameraVisibleTrigger;

            public MuzzlePointComponent muzzlePoint;

            public UserGroupComponent userGroup;
            public WeaponComponent weapon;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        [Not(typeof(ShaftWaitingStateComponent))]
        [Not(typeof(ShaftAimingWorkActivationStateComponent))]
        [Not(typeof(ShaftAimingWorkingStateComponent))]
        [Not(typeof(ShaftAimingWorkFinishStateComponent))]
        public class NotShaftAimingWeaponNode : WeaponNode { }

        public class ShaftAimingWeaponNode : WeaponNode {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class ReadyWeaponNode : WeaponNode {
            public WeaponSelfTargetHitFeedbackGroupComponent weaponSelfTargetHitFeedbackGroup;
            public WeaponSelfTargetHitFeedbackTimerComponent weaponSelfTargetHitFeedbackTimer;
        }

        [Not(typeof(StreamWeaponComponent))]
        [Not(typeof(DroneWeaponComponent))]
        public class NotStreamWeaponNode : ReadyWeaponNode { }

        [Not(typeof(StreamHitConfigComponent))]
        public class StreamWeaponNode : ReadyWeaponNode {
            public StreamWeaponComponent streamWeapon;
        }

        public class StreamHitWeaponNode : ReadyWeaponNode {
            public StreamHitConfigComponent streamHitConfig;
        }

        public class BattleCameraNode : Node {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }

        public class EffectInstanceFullNode : Node {
            public SelfTargetHitFeedbackHUDInstanceComponent selfTargetHitFeedbackHUDInstance;

            public TankGroupComponent tankGroup;

            public WeaponSelfTargetHitFeedbackGroupComponent weaponSelfTargetHitFeedbackGroup;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }

        public class WeaponSelfTargetHitFeedbackTimerComponent : Component {
            public WeaponSelfTargetHitFeedbackTimerComponent(float lastTime) => LastTime = lastTime;

            public float LastTime { get; set; }
        }

        public class WeaponSelfTargetHitFeedbackGroupComponent : GroupComponent {
            public WeaponSelfTargetHitFeedbackGroupComponent(Entity keyEntity)
                : base(keyEntity) { }

            public WeaponSelfTargetHitFeedbackGroupComponent(long key)
                : base(key) { }
        }
    }
}