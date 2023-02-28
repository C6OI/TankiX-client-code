using System;
using System.Collections;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusBuilderSystem : ECSSystem {
        [OnEventFire]
        public void RequestBonusPrefab(NodeAddedEvent e, SingleNode<BonusBoxPrefabComponent> bonusPrefab) {
            bonusPrefab.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(bonusPrefab.component.AssetGuid)));
            bonusPrefab.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void BuildBonusBox(NodeAddedEvent e, [Combine] BonusBoxBuildNode bonus, [Context] [JoinByBattle] BonusClientConfigNode bonusClientConfig,
            [Context] [JoinByMap] MapEffectNode mapEffect) {
            GameObject prefab = (GameObject)bonus.resourceData.Data;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = prefab;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, bonus);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            GameObject gameObject = instance.gameObject;
            BonusPhysicsBehaviour bonusPhysicsBehaviour = gameObject.AddComponent<BonusPhysicsBehaviour>();
            bonusPhysicsBehaviour.TriggerEntity = bonus.Entity;
            BonusBoxInstanceComponent bonusBoxInstanceComponent = new();
            bonusBoxInstanceComponent.BonusBoxInstance = gameObject;
            BonusBoxInstanceComponent component = bonusBoxInstanceComponent;
            bonus.Entity.AddComponent(component);
            gameObject.SetActive(true);
        }

        [OnEventFire]
        public void PrepareBonusBoxData(NodeAddedEvent e, BonusBoxInstantiatedNode bonus) {
            GameObject bonusBoxInstance = bonus.bonusBoxInstance.BonusBoxInstance;
            BonusDataComponent bonusData = new();
            Vector3 position = bonus.position.Position;
            bonusData.BoxHeight = bonusBoxInstance.GetComponent<BoxCollider>().size.y;
            CalculateGroundPointAndNormal(position, bonusData, bonus);
            CalculateLandingPivot(position, ref bonusData);
            bonusData.FallDuration = (position.y - bonusData.LandingPoint.y) / bonus.bonusConfig.FallSpeed;

            if (bonusData.GroundPointNormal != Vector3.up) {
                bonusData.AlignmentToGroundDuration = Mathf.Acos(bonusData.GroundPointNormal.y) * 57.29578f / bonus.bonusConfig.AlignmentToGroundAngularSpeed;
                bonusData.LandingAxis = Vector3.Cross(Vector3.up, bonusData.GroundPointNormal);
            }

            bonus.Entity.AddComponent(bonusData);
        }

        [OnEventFire]
        public void BuildParachuteIfNeed(NodeAddedEvent e, [Combine] BonusBoxDataNode bonus, ParachuteMapEffectNode mapEffect) {
            if (IsUnderCeil(bonus.position.Position) || IsOnGround(bonus.position.Position, bonus.bonusData, bonus.bonusDropTime)) {
                bonus.Entity.AddComponent<BonusSpawnOnGroundStateComponent>();
                bonus.Entity.AddComponent<BonusGroundedStateComponent>();
                bonus.Entity.AddComponent<BonusSpawnStateComponent>();
                return;
            }

            GameObject parachute = mapEffect.bonusParachuteMapEffect.Parachute;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = parachute;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, bonus);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            GameObject gameObject = instance.gameObject;
            instance.parent = bonus.bonusBoxInstance.BonusBoxInstance.transform;
            instance.localPosition = new Vector3(0f, bonus.bonusData.BoxHeight, 0f);
            instance.rotation = Quaternion.identity;
            instance.localScale = Vector3.one;
            IEnumerator enumerator = instance.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    transform.localRotation = Quaternion.identity;
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }

            bonus.Entity.AddComponent(new BonusParachuteInstanceComponent(gameObject));
            gameObject.CollectComponentsInChildren(bonus.Entity);
            bonus.Entity.AddComponent<BonusSpawnStateComponent>();
            gameObject.SetActive(true);
        }

        [OnEventFire]
        public void CalculateOnGroundPosition(NodeAddedEvent e, BonusOnGroundNode bonus) {
            RaycastHit hitInfo;

            if (Physics.Raycast(bonus.position.Position, Vector3.down, out hitInfo, float.PositiveInfinity, LayerMasks.STATIC)) {
                Vector3 point = hitInfo.point;
                bonus.position.Position = point;
                bonus.rotation.RotationEuler = hitInfo.transform.eulerAngles;
                ScheduleEvent<SetBonusPositionEvent>(bonus);
            }
        }

        [OnEventFire]
        public void PrepareParachuteData(NodeAddedEvent e, BonusWithParachuteNode bonus) {
            BonusDataComponent bonusData = bonus.bonusData;
            Renderer componentInChildren = bonus.bonusParachuteInstance.BonusParachuteInstance.GetComponentInChildren<Renderer>();
            bonusData.ParachuteHalfHeight = componentInChildren.bounds.size.y * 0.5f;
            bonusData.SwingPivotY = bonusData.BoxHeight + bonusData.ParachuteHalfHeight;
        }

        static void CalculateGroundPointAndNormal(Vector3 spawnPosition, BonusDataComponent bonusData, BonusBoxInstantiatedNode bonus) {
            RaycastHit hitInfo;

            if (Physics.Raycast(spawnPosition, Vector3.down, out hitInfo, float.PositiveInfinity, LayerMasks.STATIC)) {
                bonusData.GroundPoint = hitInfo.point;
                bonusData.GroundPointNormal = hitInfo.normal;
                bonus.rotation.RotationEuler = hitInfo.transform.eulerAngles;
            } else {
                bonusData.GroundPoint = spawnPosition;
                bonusData.GroundPointNormal = Vector3.up;
            }
        }

        static void CalculateLandingPivot(Vector3 spawnPosition, ref BonusDataComponent bonusData) {
            Vector3 normalized = Vector3.Cross(bonusData.GroundPointNormal, Vector3.up).normalized;
            Vector3 vector = Vector3.Cross(normalized, bonusData.GroundPointNormal);
            Vector3 vector2 = Vector3.Cross(normalized, Vector3.up);
            Vector3 origin = spawnPosition + vector2 * bonusData.BoxHeight * 0.5f;
            bonusData.LandingPoint = bonusData.GroundPoint + vector * (bonusData.BoxHeight * 0.5f / bonusData.GroundPointNormal.y);
            RaycastHit hitInfo;

            if (Physics.Raycast(origin, Vector3.down, out hitInfo, float.PositiveInfinity, LayerMasks.STATIC) && bonusData.GroundPoint.y < hitInfo.point.y &&
                hitInfo.point.y < bonusData.LandingPoint.y) {
                bonusData.LandingPoint += vector * (bonusData.BoxHeight * 0.5f / bonusData.GroundPointNormal.y * (bonusData.LandingPoint.y - hitInfo.point.y) /
                                                    (bonusData.LandingPoint.y - bonusData.GroundPoint.y));
            }
        }

        static bool IsUnderCeil(Vector3 spawnPosition) => Physics.Raycast(spawnPosition, Vector3.up, float.PositiveInfinity, LayerMasks.VISUAL_STATIC);

        bool IsOnGround(Vector3 position, BonusDataComponent bonusData, BonusDropTimeComponent bonusDropTime) {
            Date beginDate = bonusDropTime.DropTime + bonusData.FallDuration;
            float progress = Date.Now.GetProgress(beginDate, bonusData.AlignmentToGroundDuration);
            return MathUtil.NearlyEqual(progress, 1f, 0.01f);
        }

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, InstantiatedBonusNode bonus) {
            if (!bonus.bonusBoxInstance.Removed) {
                bonus.bonusBoxInstance.BonusBoxInstance.RecycleObject();
            }
        }

        public class BonusClientConfigNode : Node {
            public BattleGroupComponent battleGroup;
            public BonusClientConfigComponent bonusClientConfig;

            public MapGroupComponent mapGroup;
        }

        public class MapEffectNode : Node {
            public MapEffectInstanceComponent mapEffectInstance;

            public MapGroupComponent mapGroup;
        }

        public class ParachuteMapEffectNode : MapEffectNode {
            public BonusParachuteMapEffectComponent bonusParachuteMapEffect;
        }

        public class BonusBoxBaseNode : Node {
            public BattleGroupComponent battleGroup;

            public BonusComponent bonus;
            public BonusConfigComponent bonusConfig;

            public BonusDropTimeComponent bonusDropTime;

            public PositionComponent position;

            public RotationComponent rotation;
        }

        public class BonusBoxBuildNode : BonusBoxBaseNode {
            public ResourceDataComponent resourceData;
        }

        public class BonusBoxInstantiatedNode : BonusBoxBuildNode {
            public BonusBoxInstanceComponent bonusBoxInstance;
        }

        public class BonusBoxDataNode : BonusBoxInstantiatedNode {
            public BonusDataComponent bonusData;
        }

        public class BonusWithParachuteNode : BonusBoxDataNode {
            public BonusParachuteInstanceComponent bonusParachuteInstance;

            public TopParachuteMarkerComponent topParachuteMarker;
        }

        public class BonusOnGroundNode : BonusBoxDataNode {
            public BonusSpawnOnGroundStateComponent bonusSpawnOnGroundState;
        }

        public class InstantiatedBonusNode : Node {
            public BonusComponent bonus;

            public BonusBoxInstanceComponent bonusBoxInstance;
        }
    }
}