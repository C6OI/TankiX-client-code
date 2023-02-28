using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class EMPEffectSystem : ECSSystem {
        [OnEventFire]
        public void CollectTargetsForEMPEffectInTeamBattle(NodeAddedEvent e, EMPEffectNode emp, [JoinByTank] SelfTankTeamNode selfTank, [JoinByTeam] TeamNode selfTeam,
            [JoinByBattle] TeamBattleNode battle, [JoinByBattle] [Combine] TeamNode team) {
            if (!team.Entity.Equals(selfTeam.Entity)) {
                NewEvent(new CollectTargetsInRadius {
                    Radius = emp.empEffect.Radius
                }).AttachAll(emp, selfTank, battle, team).Schedule();
            }
        }

        [OnEventFire]
        public void CollectTargetsForEMPEffectInTeamBattle(CollectTargetsInRadius e, EffectNode any, SelfTankTeamNode selfTank, TeamBattleNode battle, TeamNode team,
            [JoinByTeam] ICollection<RemoteTankNode> otherTanks) {
            CollectTargetsForEMP(e, selfTank, otherTanks);
        }

        [OnEventFire]
        public void CollectTargetsForEMPEffectInNonTeamBattle(NodeAddedEvent e, EMPEffectNode emp, [JoinByTank] SelfTankNonTeamNode selfTank, [JoinByBattle] NonTeamBattleNode battle,
            [JoinByBattle] ICollection<RemoteTankNode> otherTanks) {
            NewEvent(new CollectTargetsInRadius {
                Radius = emp.empEffect.Radius
            }).AttachAll(emp, selfTank, battle).Schedule();
        }

        [OnEventFire]
        public void CollectTargetsForEMPEffectInNonTeamBattle(CollectTargetsInRadius e, EffectNode any, SelfTankNonTeamNode selfTank, NonTeamBattleNode battle,
            [JoinByBattle] ICollection<RemoteTankNode> otherTanks) {
            CollectTargetsForEMP(e, selfTank, otherTanks);
        }

        void CollectTargetsForEMP(CollectTargetsInRadius e, TankNode tank, IEnumerable<RemoteTankNode> otherTanks) {
            Vector3 position = tank.rigidbody.Rigidbody.position;
            e.Targets = new List<Entity>();

            foreach (RemoteTankNode otherTank in otherTanks) {
                Vector3 position2 = otherTank.rigidbody.Rigidbody.position;
                Collider boundsCollider = otherTank.tankColliders.BoundsCollider;
                SkinnedMeshRenderer skinnedMeshRenderer = otherTank.baseRenderer.Renderer as SkinnedMeshRenderer;

                if (!(skinnedMeshRenderer == null)) {
                    float y = skinnedMeshRenderer.localBounds.extents.y;

                    if (CheckBodyInRadius(position, e.Radius, position2, boundsCollider, y)) {
                        e.Targets.Add(otherTank.Entity);
                    }
                }
            }
        }

        [OnEventComplete]
        public void SendEmpTargetsToServer(CollectTargetsInRadius evt, EMPEffectNode emp, SelfTankNode tank) {
            ApplyTargetsForEMPEffectEvent applyTargetsForEMPEffectEvent = new();
            applyTargetsForEMPEffectEvent.Targets = evt.Targets.ToArray();
            ApplyTargetsForEMPEffectEvent eventInstance = applyTargetsForEMPEffectEvent;
            ScheduleEvent<SynchronizeSelfTankPositionBeforeEffectEvent>(tank);
            ScheduleEvent(eventInstance, emp);
        }

        [OnEventFire]
        public void EmptySlotLocked(NodeAddedEvent e, SingleNode<SlotLockedByEMPComponent> node) { }

        bool CheckBodyInRadius(Vector3 center, float radius, Vector3 targetPosition, Collider collider, float yOffset = 0f) {
            Vector3 vector = targetPosition + new Vector3(0f, yOffset, 0f);
            Ray ray = new(center, (vector - center).normalized);
            RaycastHit hitInfo;
            return collider.Raycast(ray, out hitInfo, radius);
        }

        public class EMPEffectNode : Node {
            public EMPEffectComponent empEffect;

            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public BaseRendererComponent baseRenderer;

            public BattleGroupComponent battleGroup;

            public RigidbodyComponent rigidbody;

            public TankActiveStateComponent tankActiveState;

            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : TankNode {
            public SelfTankComponent selfTank;
        }

        public class SelfTankTeamNode : SelfTankNode {
            public TeamGroupComponent teamGroup;
        }

        [Not(typeof(TeamGroupComponent))]
        public class SelfTankNonTeamNode : SelfTankNode { }

        public class TeamNode : Node {
            public BattleGroupComponent battleGroup;
            public TeamComponent team;

            public TeamGroupComponent teamGroup;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
        }

        public class TeamBattleNode : BattleNode {
            public TeamBattleComponent teamBattle;
        }

        [Not(typeof(TeamGroupComponent))]
        public class NonTeamBattleNode : BattleNode { }

        public class EffectNode : Node {
            public EffectComponent effect;
        }
    }
}