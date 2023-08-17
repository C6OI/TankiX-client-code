using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusBuilderSystem : ECSSystem {
        [OnEventFire]
        public void RequestBonusPrefab(NodeAddedEvent e, BonusPrefabNode bonusPrefab) {
            List<AssetReference> list = new();
            list.Add(new AssetReference(bonusPrefab.bonusBoxPrefab.AssetGuid));
            list.Add(new AssetReference(bonusPrefab.bonusParachutePrefab.AssetGuid));
            list.Add(new AssetReference(bonusPrefab.brokenBonusBoxPrefab.AssetGuid));
            List<AssetReference> references = list;
            bonusPrefab.Entity.AddComponent(new AssetReferenceListComponent(references));
            bonusPrefab.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void BuildBonusBox(NodeAddedEvent e, [Combine] BonusBoxBuildNode bonus,
            [JoinByBattle] [Context] SingleNode<BonusClientConfigComponent> bonusClientConfig,
            [JoinByMap] [Context] SingleNode<MapInstanceComponent> map) {
            GameObject original = (GameObject)bonus.resourceDataList.DataList[0];
            GameObject gameObject = Object.Instantiate(original);
            BonusPhysicsBehaviour bonusPhysicsBehaviour = gameObject.AddComponent<BonusPhysicsBehaviour>();
            bonusPhysicsBehaviour.TriggerEntity = bonus.Entity;
            BonusBoxInstanceComponent bonusBoxInstanceComponent = new();
            bonusBoxInstanceComponent.BonusBoxInstance = gameObject;
            bonus.Entity.AddComponent(bonusBoxInstanceComponent);
        }

        [OnEventFire]
        public void PrepareBonusBoxData(NodeAddedEvent e, BonusBoxPrepareDataNode bonus) {
            GameObject bonusBoxInstance = bonus.bonusBoxInstance.BonusBoxInstance;
            BonusDataComponent bonusData = new();
            Vector3 position = bonus.position.Position;
            bonusData.BoxHeight = bonusBoxInstance.GetComponent<BoxCollider>().size.y;
            CalculateGroundPointAndNormal(position, bonusData, bonus);
            CalculateLandingPivot(position, ref bonusData);
            bonusData.FallDuration = (position.y - bonusData.LandingPoint.y) / bonus.bonusConfig.FallSpeed;

            if (bonusData.GroundPointNormal != Vector3.up) {
                bonusData.AlignmentToGroundDuration = Mathf.Acos(bonusData.GroundPointNormal.y) *
                                                      57.29578f /
                                                      bonus.bonusConfig.AlignmentToGroundAngularSpeed;

                bonusData.LandingAxis = Vector3.Cross(Vector3.up, bonusData.GroundPointNormal);
            }

            bonus.Entity.AddComponent(bonusData);
        }

        [OnEventFire]
        public void BuildParachuteIfNeed(NodeAddedEvent e, BonusParachuteBuildNode bonus) {
            if (IsUnderCeil(bonus.position.Position) ||
                IsOnGround(bonus.position.Position, bonus.bonusData, bonus.bonusDropTime)) {
                bonus.Entity.AddComponent<BonusSpawnOnGroundStateComponent>();
                bonus.Entity.AddComponent<BonusGroundedStateComponent>();
                return;
            }

            GameObject original = (GameObject)bonus.resourceDataList.DataList[1];
            GameObject gameObject = Object.Instantiate(original);
            bonus.Entity.AddComponent(new BonusParachuteInstanceComponent(gameObject));
            gameObject.CollectComponentsInChildren(bonus.Entity);
            gameObject.transform.parent = bonus.bonusBoxInstance.BonusBoxInstance.transform;
            gameObject.transform.localPosition = new Vector3(0f, bonus.bonusData.BoxHeight, 0f);
        }

        [OnEventFire]
        public void CalculateOnGroundPosition(NodeAddedEvent e, BonusOnGroundNode bonus) {
            RaycastHit hitInfo;

            if (Physics.Raycast(bonus.position.Position,
                    Vector3.down,
                    out hitInfo,
                    float.PositiveInfinity,
                    LayerMasks.STATIC)) {
                Vector3 point = hitInfo.point;
                bonus.position.Position = point;
                bonus.rotation.RotationEuler = hitInfo.transform.eulerAngles;
                ScheduleEvent<SetBonusPositionEvent>(bonus);
            }
        }

        [OnEventFire]
        public void PrepareParachuteData(NodeAddedEvent e, BonusParachutePrepareDateNode bonus) {
            BonusDataComponent bonusData = bonus.bonusData;

            Renderer componentInChildren =
                bonus.bonusParachuteInstance.BonusParachuteInstance.GetComponentInChildren<Renderer>();

            bonusData.ParachuteHalfHeight = componentInChildren.bounds.size.y * 0.5f;
            bonusData.SwingPivotY = bonusData.BoxHeight + bonusData.ParachuteHalfHeight;
        }

        [OnEventComplete]
        public void BonusToSpawn(NodeAddedEvent e, BonusToSpawnStateNode bonus) =>
            bonus.Entity.AddComponent<BonusSpawnStateComponent>();

        static void CalculateGroundPointAndNormal(Vector3 spawnPosition, BonusDataComponent bonusData,
            BonusBoxPrepareDataNode bonus) {
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

            bonusData.LandingPoint =
                bonusData.GroundPoint + vector * (bonusData.BoxHeight * 0.5f / bonusData.GroundPointNormal.y);

            RaycastHit hitInfo;

            if (Physics.Raycast(origin, Vector3.down, out hitInfo, float.PositiveInfinity, LayerMasks.STATIC) &&
                bonusData.GroundPoint.y < hitInfo.point.y &&
                hitInfo.point.y < bonusData.LandingPoint.y) {
                bonusData.LandingPoint +=
                    vector *
                    (bonusData.BoxHeight *
                     0.5f /
                     bonusData.GroundPointNormal.y *
                     (bonusData.LandingPoint.y - hitInfo.point.y) /
                     (bonusData.LandingPoint.y - bonusData.GroundPoint.y));
            }
        }

        static bool IsUnderCeil(Vector3 spawnPosition) =>
            Physics.Raycast(spawnPosition, Vector3.up, float.PositiveInfinity, LayerMasks.STATIC);

        bool IsOnGround(Vector3 position, BonusDataComponent bonusData, BonusDropTimeComponent bonusDropTime) {
            Date beginDate = bonusDropTime.DropTime + bonusData.FallDuration;
            float progress = Date.Now.GetProgress(beginDate, bonusData.AlignmentToGroundDuration);
            return MathUtil.NearlyEqual(progress, 1f, 0.01f);
        }

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, InstantiatedBonusNode bonus) {
            if (!bonus.bonusBoxInstance.Removed) {
                UnityUtil.Destroy(bonus.bonusBoxInstance.BonusBoxInstance);
            }
        }

        public class BonusPrefabNode : Node {
            public BonusBoxPrefabComponent bonusBoxPrefab;

            public BonusParachutePrefabComponent bonusParachutePrefab;

            public BrokenBonusBoxPrefabComponent brokenBonusBoxPrefab;
        }

        public class BonusBoxBuildNode : Node {
            public BattleGroupComponent battleGroup;
            public BonusComponent bonus;

            public BonusDropTimeComponent bonusDropTime;

            public ResourceDataListComponent resourceDataList;
        }

        public class BonusBoxPrepareDataNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BonusConfigComponent bonusConfig;

            public PositionComponent position;

            public RotationComponent rotation;
        }

        public class BonusParachuteBuildNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BonusDataComponent bonusData;

            public BonusDropTimeComponent bonusDropTime;

            public PositionComponent position;

            public ResourceDataListComponent resourceDataList;
        }

        public class BonusParachutePrepareDateNode : Node {
            public BonusDataComponent bonusData;

            public BonusParachuteInstanceComponent bonusParachuteInstance;

            public TopParachuteMarkerComponent topParachuteMarker;
        }

        public class BonusToSpawnStateNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BonusDataComponent bonusData;
        }

        public class BonusOnGroundNode : Node {
            public BonusDataComponent bonusData;
            public BonusSpawnOnGroundStateComponent bonusSpawnOnGroundState;

            public PositionComponent position;

            public RotationComponent rotation;
        }

        public class InstantiatedBonusNode : Node {
            public BonusComponent bonus;

            public BonusBoxInstanceComponent bonusBoxInstance;
        }
    }
}