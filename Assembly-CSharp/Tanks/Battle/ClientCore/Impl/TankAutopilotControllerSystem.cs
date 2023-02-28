using System;
using System.Collections.Generic;
using System.Linq;
using Assets.tanks.modules.battle.ClientCore.Scripts.Impl.Autopilot;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tanks.Battle.ClientCore.Impl {
    public class TankAutopilotControllerSystem : ECSSystem {
        public const string SURVIVOR_PREFIX = "Survivor ";

        public const string DESERTER_PREFIX = "Deserter ";

        static float NEAR_EQUALS_DOT = 0.99f;

        [Inject] public static BattleFlowInstancesCache BattleCache { get; set; }

        [OnEventFire]
        public void FixUid(NodeAddedEvent e, [Combine] UserUidNode uid, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            if (!uid.Entity.Equals(selfUser.Entity)) {
                bool flag = uid.userUid.Uid.Contains("Deserter ");
                bool flag2 = uid.battleLeaveCounter.NeedGoodBattles > 0;

                if (!flag2 && flag) {
                    uid.userUid.Uid = uid.userUid.Uid.Replace("Deserter ", "Survivor ");
                } else if (flag2 && !flag) {
                    uid.userUid.Uid = uid.userUid.Uid.Insert(0, "Deserter ");
                } else if (!flag2) {
                    uid.userUid.Uid = uid.userUid.Uid.Insert(0, "Survivor ");
                }

                uid.userUid.Uid = uid.userUid.Uid.Replace("botxz_", string.Empty);
            }
        }

        [OnEventFire]
        public void CreateBehaviourTree(NodeAddedEvent e, AutopilotTankNode tank, [JoinByTank] WeaponNode weapon) {
            ConditionNode conditionNode = new(string.Empty);
            conditionNode.Condition = () => tank.autopilotMovementController.Moving;
            ConditionNode condition = conditionNode;
            ActionNode actionNode = new();

            actionNode.Action = delegate {
                MoveBack(tank);
            };

            ActionNode action = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                StopMovement(tank);
            };

            ActionNode action2 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                StopRotation(tank);
            };

            ActionNode action3 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                CalculateDirection(tank);
            };

            ActionNode action4 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                CalculateRotation(tank);
            };

            ActionNode action5 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                InvertTurnAxisIfReallyMoveBack(tank);
            };

            ActionNode action6 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                ObstacleAvoidanceRaycasts(tank);
            };

            ActionNode action7 = actionNode;
            conditionNode = new ConditionNode(string.Empty);
            conditionNode.Condition = () => tank.rigidbody.Rigidbody.angularVelocity.magnitude > 0.1;
            ConditionNode condition2 = conditionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                TanksAvoidanceRaycasts(tank);
            };

            ActionNode action8 = actionNode;
            conditionNode = new ConditionNode(string.Empty);
            conditionNode.Condition = () => tank.navigationData.ObstacleOnCriticalDistance;
            ConditionNode condition3 = conditionNode;
            conditionNode = new ConditionNode(string.Empty);
            conditionNode.Condition = () => tank.navigationData.TankInTheFront;
            ConditionNode condition4 = conditionNode;
            conditionNode = new ConditionNode(string.Empty);
            conditionNode.Condition = () => tank.navigationData.ObstacleOnAvoidanceDistance;
            ConditionNode condition5 = conditionNode;

            BehaviourTreeBuilder treePart = new BehaviourTreeBuilder("Drive back when obstacle on critical distance").StartPreconditionSequence().If(condition3).ForTime(0.3f)
                .StartParallel()
                .Do(action)
                .Do(action6)
                .End()
                .End();

            BehaviourTreeBuilder treePart2 = new BehaviourTreeBuilder("React on tank in front").StartSequence().If(condition4).ForTime(0.5f)
                .StartParallel()
                .Do(action)
                .Do(action6)
                .End()
                .ForTime(3f)
                .Do(action2)
                .End();

            BehaviourTreeBuilder treePart3 = new BehaviourTreeBuilder("Avoid obstacle").StartPreconditionSequence().If(condition5).If(condition2)
                .Do(action2)
                .End();

            BehaviourTreeBuilder treePart4 = new BehaviourTreeBuilder("Try move to target").StartSelector().ConnectTree(treePart).ConnectTree(treePart2)
                .ConnectTree(treePart3)
                .Do(action4)
                .End();

            BehaviourTreeBuilder treePart5 = new BehaviourTreeBuilder("Enviroment analysis").StartDoOnceIn(0.3f).Do(action8).Do(action7)
                .End();

            BehaviourTreeBuilder treePart6 = new BehaviourTreeBuilder("Start movement if allowed").StartPreconditionSequence().If(condition).StartParallel()
                .ConnectTree(treePart5)
                .Do(action5)
                .ConnectTree(treePart4)
                .End()
                .End();

            BehaviourTreeBuilder treePart7 = new BehaviourTreeBuilder("Stop movement").StartParallel().Do(action2).Do(action3)
                .End();

            BehaviourTreeBuilder treePart8 = new BehaviourTreeBuilder("movement tree").StartSelector().ConnectTree(treePart6).ConnectTree(treePart7)
                .End();

            actionNode = new ActionNode();

            actionNode.Action = delegate {
                StopFireAndRotation(tank, weapon);
            };

            ActionNode action9 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                RotateToTarget(tank, weapon);
            };

            ActionNode action10 = actionNode;
            conditionNode = new ConditionNode(string.Empty);
            conditionNode.Condition = () => tank.autopilotWeaponController.Attack;
            ConditionNode condition6 = conditionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                tank.autopilotWeaponController.ShouldMiss = CheckAccuracy(tank);
            };

            ActionNode action11 = actionNode;
            conditionNode = new ConditionNode(string.Empty);
            conditionNode.Condition = () => tank.autopilotWeaponController.ShouldMiss;
            ConditionNode condition7 = conditionNode;
            conditionNode = new ConditionNode(string.Empty);
            conditionNode.Condition = () => IsInShootingRange(tank, weapon);
            ConditionNode condition8 = conditionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                tank.autopilotWeaponController.IsOnShootingLine = IsOnShootingLine(tank, weapon);
            };

            ActionNode action12 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                FireIfShould(tank);
            };

            ActionNode action13 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                CheckIsTargetAchievable(tank, weapon);
            };

            ActionNode action14 = actionNode;
            actionNode = new ActionNode();

            actionNode.Action = delegate {
                ApplyMissBehaviour(tank, weapon);
            };

            ActionNode action15 = actionNode;

            BehaviourTreeBuilder treePart9 = new BehaviourTreeBuilder("Missing").StartPreconditionSequence().If(condition7).If(condition8)
                .Do(action15)
                .End();

            BehaviourTreeBuilder treePart10 = new BehaviourTreeBuilder("Rotate and attack").StartSelector().ConnectTree(treePart9).StartParallel()
                .Do(action10)
                .Do(action13)
                .End()
                .End();

            BehaviourTreeBuilder treePart11 = new BehaviourTreeBuilder("Attack target").StartPreconditionSequence().If(condition6).StartParallel()
                .StartDoOnceIn(1f)
                .Do(action11)
                .Do(action12)
                .End()
                .StartDoOnceIn(3f)
                .Do(action14)
                .End()
                .ConnectTree(treePart10)
                .End()
                .End();

            BehaviourTreeBuilder treePart12 = new BehaviourTreeBuilder("Main targeting tree").StartSelector().ConnectTree(treePart11).Do(action9)
                .End();

            BehaviourTreeNode behavouTree = new BehaviourTreeBuilder("Main tree").StartParallel().ConnectTree(treePart8).ConnectTree(treePart12)
                .End()
                .Build();

            tank.navigationData.BehavouTree = behavouTree;
        }

        [OnEventComplete]
        public void Control(UpdateEvent e, AutopilotTankNode tank) {
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;

            if (!rigidbody || tank.navigationData.BehavouTree == null) {
                return;
            }

            Entity target = tank.autopilotMovementController.Target;

            if (tank.autopilotMovementController.MoveToTarget && target == null) {
                return;
            }

            if (target != null) {
                if (!target.HasComponent<RigidbodyComponent>()) {
                    return;
                }

                if (tank.autopilotWeaponController.TargetRigidbody != target.GetComponent<RigidbodyComponent>().Rigidbody) {
                    tank.autopilotWeaponController.TargetRigidbody = target.GetComponent<RigidbodyComponent>().Rigidbody;
                }
            }

            tank.navigationData.BehavouTree.Update();
            CheckChassisChange(tank);
        }

        void ObstacleAvoidanceRaycasts(AutopilotTankNode tank) {
            Transform transform = tank.tankColliders.BoundsCollider.transform;
            BoxCollider boundsCollider = tank.tankColliders.BoundsCollider;
            tank.navigationData.ObstacleOnAvoidanceDistance = false;
            tank.navigationData.ObstacleOnCriticalDistance = false;
            float num = boundsCollider.size.x * 0.5f;

            for (float num2 = (0f - boundsCollider.size.x) * 0.5f; num2 < num; num2 += 0.5f) {
                Vector3 origin = transform.TransformPoint(new Vector3(num2, boundsCollider.size.y * 0.8f, boundsCollider.size.z * 0.5f));
                RaycastHit hitInfo;

                if (Physics.Raycast(new Ray(origin, transform.forward), out hitInfo, 2f, LayerMasks.STATIC) && Math.Abs(hitInfo.normal.y) < 0.5) {
                    tank.navigationData.ObstacleOnAvoidanceDistance = true;

                    if (hitInfo.distance < 0.5) {
                        tank.navigationData.ObstacleOnCriticalDistance = true;
                    }

                    break;
                }
            }
        }

        void TanksAvoidanceRaycasts(AutopilotTankNode tank) {
            Transform transform = tank.tankColliders.BoundsCollider.transform;
            BoxCollider boundsCollider = tank.tankColliders.BoundsCollider;
            tank.navigationData.TankInTheFront = false;
            float num = boundsCollider.size.x * 0.5f;

            for (float num2 = (0f - boundsCollider.size.x) * 0.5f; num2 < num; num2 += 0.5f) {
                Vector3 origin = transform.TransformPoint(new Vector3(num2, boundsCollider.size.y * 0.8f, boundsCollider.size.z * 0.5f));
                RaycastHit hitInfo;

                if (Physics.Raycast(new Ray(origin, transform.forward), out hitInfo, 1f, LayerMasks.TANK_TO_TANK)) {
                    tank.navigationData.TankInTheFront = true;
                    break;
                }
            }
        }

        void CalculateDirection(AutopilotTankNode tank) {
            Vector3 movePosition = tank.navigationData.MovePosition;
            ChassisComponent chassis = tank.chassis;
            chassis.MoveAxis = 0f;
            Vector3 vector = tank.rigidbody.Rigidbody.transform.InverseTransformPoint(movePosition);

            if (vector.z > 0.2) {
                chassis.MoveAxis = 1f;
            }

            if (vector.z < -0.2) {
                chassis.MoveAxis = -1f;
            }
        }

        void CalculateRotation(AutopilotTankNode tank) {
            Vector3 movePosition = tank.navigationData.MovePosition;
            ChassisComponent chassis = tank.chassis;
            chassis.TurnAxis = 0f;
            Vector3 vector = tank.rigidbody.Rigidbody.transform.InverseTransformPoint(movePosition);

            if (vector.x > 0.2) {
                chassis.TurnAxis = 1f;
            }

            if (vector.x < -0.2) {
                chassis.TurnAxis = -1f;
            }
        }

        void CheckChassisChange(AutopilotTankNode tank) {
            ChassisComponent chassis = tank.chassis;

            if (chassis.MoveAxis != tank.navigationData.LastMove || chassis.TurnAxis != tank.navigationData.LastTurn) {
                ScheduleEvent<ChassisControlChangedEvent>(tank);
                tank.navigationData.LastMove = chassis.MoveAxis;
                tank.navigationData.LastTurn = chassis.TurnAxis;
            }
        }

        void StopMovement(AutopilotTankNode tank) {
            tank.chassis.MoveAxis = 0f;
        }

        void MoveBack(AutopilotTankNode tank) {
            tank.chassis.MoveAxis = -1f;
        }

        void InvertTurnAxisIfReallyMoveBack(AutopilotTankNode tank) {
            Vector3 velocity = tank.rigidbody.Rigidbody.velocity;

            if (tank.rigidbody.Rigidbody.transform.InverseTransformDirection(velocity).z <= -1.0) {
                tank.chassis.TurnAxis *= -1f;
            }
        }

        void StopRotation(AutopilotTankNode tank) {
            tank.chassis.TurnAxis = 0f;
        }

        [OnEventFire]
        public void SetTankSync(NodeAddedEvent e, SingleNode<TankAutopilotComponent> autopilot, [JoinAll] SingleNode<ClientSessionComponent> session) {
            if (session.Entity.Equals(autopilot.component.Session)) {
                autopilot.Entity.AddComponentIfAbsent<TankSyncComponent>();
            }
        }

        [OnEventFire]
        public void SetTankSync(ChangeAutopilotControllerEvent e, SingleNode<TankAutopilotComponent> autopilot, [JoinAll] SingleNode<ClientSessionComponent> session) {
            if (session.Entity.Equals(autopilot.component.Session)) {
                autopilot.Entity.AddComponentIfAbsent<TankSyncComponent>();
            } else {
                autopilot.Entity.RemoveComponentIfPresent<TankSyncComponent>();
            }
        }

        [OnEventFire]
        public void Accept(RequestAutopilotControllerEvent e, SingleNode<TankAutopilotComponent> autopilot, [JoinAll] SelfUserReadyToBattleNode user) {
            AcceptAutopilotControllerEvent acceptAutopilotControllerEvent = new();
            acceptAutopilotControllerEvent.Version = autopilot.component.Version;
            AcceptAutopilotControllerEvent eventInstance = acceptAutopilotControllerEvent;
            ScheduleEvent(eventInstance, autopilot);
        }

        void ApplyMissBehaviour(AutopilotTankNode tank, WeaponNode weapon) {
            Vector3 position = tank.autopilotWeaponController.TargetRigidbody.position;
            Transform transform = weapon.weaponInstance.WeaponInstance.transform;
            Vector3 vector = transform.InverseTransformPoint(position);
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            weaponRotationControl.Centering = false;

            if (Math.Abs(vector.x) > 3f) {
                tank.autopilotWeaponController.Fire = true;
                weaponRotationControl.Control = 0f;
            } else {
                tank.autopilotWeaponController.Fire = false;
                weaponRotationControl.Control = 0f - ToDiscrete(vector.x, 0f);
            }
        }

        void FireIfShould(AutopilotTankNode tank) {
            tank.autopilotWeaponController.Fire = tank.autopilotWeaponController.IsOnShootingLine;
        }

        void CheckIsTargetAchievable(AutopilotTankNode tank, WeaponNode weapon) {
            Vector3 position = tank.autopilotWeaponController.TargetRigidbody.position;
            Vector3 position2 = weapon.weaponInstance.WeaponInstance.transform.position;
            Vector3 normalized = (position - position2).normalized;
            DirectionData directionData = weapon.targetCollector.Collect(position2, new Vector3(normalized.x, 0f, normalized.z), Vector3.Distance(position2, position));
            bool flag = !directionData.HasTargetHit();

            if (tank.autopilotWeaponController.TragerAchievable != flag) {
                tank.autopilotWeaponController.TragerAchievable = flag;
                tank.autopilotWeaponController.OnChange();
            }
        }

        bool IsInShootingRange(AutopilotTankNode tank, WeaponNode weapon) {
            Vector3 position = tank.autopilotWeaponController.TargetRigidbody.position;
            Transform transform = weapon.weaponInstance.WeaponInstance.transform;
            Vector3 vector = transform.InverseTransformPoint(position);
            return Math.Abs(vector.x) < 4f && vector.z > 0f;
        }

        void RotateToTarget(AutopilotTankNode tank, WeaponNode weapon) {
            Vector3 position = tank.autopilotWeaponController.TargetRigidbody.position;
            Transform transform = weapon.weaponInstance.WeaponInstance.transform;
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            weaponRotationControl.Control = ToDiscrete(transform.InverseTransformPoint(position).x, 0f);
        }

        bool CheckAccuracy(AutopilotTankNode tank) => tank.autopilotWeaponController.Accurasy < Random.value;

        bool IsOnShootingLine(AutopilotTankNode tank, WeaponNode weapon) {
            Entity target = tank.autopilotMovementController.Target;
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            TargetingEvent eventInstance = BattleCache.targetingEvent.GetInstance().Init(targetingData);
            ScheduleEvent(eventInstance, weapon);

            if (targetingData.HasTargetHit()) {
                List<DirectionData> directions = targetingData.Directions;

                if (directions.Any(direction => direction.HasTargetHit() && direction.Targets[0].TargetEntity.Equals(target)) && !weapon.Entity.HasComponent<WeaponBlockedComponent>()) {
                    return true;
                }
            }

            return false;
        }

        float ToDiscrete(float value, float zeroMinValue) {
            if (value > zeroMinValue) {
                return 1f;
            }

            if (value < 0f - zeroMinValue) {
                return -1f;
            }

            return 0f;
        }

        void StopFireAndRotation(AutopilotTankNode tank, WeaponNode weapon) {
            weapon.weaponRotationControl.Rotation = 0f;
            tank.autopilotWeaponController.Fire = false;
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

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public TargetCollectorComponent targetCollector;

            public WeaponGyroscopeRotationComponent weaponGyroscopeRotation;

            public WeaponInstanceComponent weaponInstance;

            public WeaponRotationComponent weaponRotation;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class SelfUserReadyToBattleNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class UserUidNode : Node {
            public BattleLeaveCounterComponent battleLeaveCounter;
            public UserUidComponent userUid;
        }
    }
}