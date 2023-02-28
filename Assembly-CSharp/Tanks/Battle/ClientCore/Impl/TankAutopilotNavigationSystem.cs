using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankAutopilotNavigationSystem : ECSSystem {
        static readonly float SELF_DESTRUCT_ON_UNDEGROUND_PROBABILITY = 0.01f;

        static float PREFARE_ATTACKING_RANGE = 15f;

        [OnEventFire]
        public void AddNavigationComponentToTank(NodeAddedEvent e, SingleNode<TankAutopilotComponent> tank) {
            tank.Entity.AddComponent(new NavigationDataComponent());
        }

        [OnEventFire]
        public void Control(FixedUpdateEvent e, AutopilotTankNode tank, [JoinByTank] WeaponNode weapon) {
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;

            if (!rigidbody) {
                return;
            }

            Vector3 targetPosition;

            if (tank.autopilotMovementController.MoveToTarget) {
                Entity target = tank.autopilotMovementController.Target;

                if (target == null && tank.autopilotMovementController.MoveToTarget || !target.HasComponent<RigidbodyComponent>() || !target.HasComponent<TankCollidersComponent>()) {
                    return;
                }

                AutopilotMovementControllerComponent autopilotMovementController = tank.autopilotMovementController;
                Vector3 position = target.GetComponent<RigidbodyComponent>().Rigidbody.position;
                targetPosition = position;
            } else {
                targetPosition = tank.autopilotMovementController.Destination;
            }

            if (tank.navigationData.PathData == null) {
                tank.navigationData.PathData = new PathData {
                    timeToRecalculatePath = Time.timeSinceLevelLoad
                };
            }

            ControlMove(tank, targetPosition);
        }

        [OnEventFire]
        public void SelfDestroy(FixedUpdateEvent e, AutopilotTankNode tank, [JoinByTank] SingleNode<WeaponUndergroundComponent> weapon) {
            if (Random.value < SELF_DESTRUCT_ON_UNDEGROUND_PROBABILITY) {
                tank.Entity.AddComponentIfAbsent<SelfDestructionComponent>();
            }
        }

        void ControlMove(AutopilotTankNode tank, Vector3 targetPosition) {
            TankAutopilotComponent tankAutopilot = tank.tankAutopilot;
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            Vector3 position = rigidbody.transform.position;
            bool flag = rigidbody.velocity.magnitude < 0.5f;

            if (tank.autopilotMovementController.Moving) {
                MoveToTarget(tank.navigationData, position, targetPosition, rigidbody);
            }
        }

        void MoveToTarget(NavigationDataComponent autopilot, Vector3 currentPosition, Vector3 targetPosition, Rigidbody rigidbody) {
            PathData pathData = autopilot.PathData;

            if (TimeToRecalculatePath(pathData) || pathData.currentPathIndex >= pathData.currentPath.Length - 3) {
                NavMeshPath navMeshPath = new();
                NavMesh.CalculatePath(currentPosition, targetPosition, -1, navMeshPath);

                if (navMeshPath.corners.Length > 1) {
                    pathData.currentPath = navMeshPath.corners;
                    pathData.currentPathIndex = 1;
                    autopilot.MovePosition = pathData.currentPath[pathData.currentPathIndex];
                    autopilot.PathData.timeToRecalculatePath = Time.timeSinceLevelLoad + Random.Range(0.5f, 2f);
                }
            } else if (CurrentPointReached(currentPosition, pathData.currentPath[pathData.currentPathIndex], rigidbody)) {
                try {
                    pathData.currentPathIndex += 2;
                    autopilot.MovePosition = pathData.currentPath[pathData.currentPathIndex];
                } catch (Exception) {
                    Debug.LogWarning("Index out of range! current index " + pathData.currentPathIndex + " array lenght" + pathData.currentPath.Length);
                }
            }

            try {
                for (int i = pathData.currentPathIndex; i < pathData.currentPath.Length - 2; i++) { }

                for (int j = 0; j < pathData.currentPathIndex - 1; j++) { }
            } catch (Exception) { }
        }

        bool TimeToRecalculatePath(PathData pathData) => pathData.timeToRecalculatePath <= Time.timeSinceLevelLoad;

        bool CurrentPointReached(Vector3 currentPosision, Vector3 currentPoint, Rigidbody rigidbody) {
            Vector3 normalized = (currentPoint - currentPosision).normalized;
            float num = Vector3.Dot(normalized, rigidbody.transform.forward);
            return num > -0.2 && num < 0.2f;
        }

        public class AutopilotTankNode : Node {
            public AutopilotMovementControllerComponent autopilotMovementController;

            public AutopilotWeaponControllerComponent autopilotWeaponController;

            public ChassisComponent chassis;

            public NavigationDataComponent navigationData;

            public RigidbodyComponent rigidbody;

            public TankActiveStateComponent tankActiveState;

            public TankAutopilotComponent tankAutopilot;

            public TankCollidersComponent tankColliders;
            public TankSyncComponent tankSync;
        }

        public class MapComponent : Node {
            public MapInstanceComponent mapInstance;
        }

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;
            public RigidbodyComponent rigidbody;

            public TankCollidersComponent tankColliders;
        }
    }
}