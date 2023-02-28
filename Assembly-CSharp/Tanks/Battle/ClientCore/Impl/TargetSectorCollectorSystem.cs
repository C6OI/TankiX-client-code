using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TargetSectorCollectorSystem : ECSSystem {
        public const float TANK_LENGTH_TO_HEIGHT_COEFF = 0.6f;

        [OnEventFire]
        public void CollectTargetSectors(CollectTargetSectorsEvent e, WeaponNode weaponNode, [JoinByBattle] ICollection<TargetTankNode> targetTankNodes, WeaponNode weaponNode1,
            [JoinByTank] OwnerTankNode ownerTankNode, [JoinByTeam] Optional<TeamNode> team) {
            MuzzleLogicAccessor muzzleLogicAccessor = new(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            bool flag = team.IsPresent();
            long num = !flag ? 0 : team.Get().teamGroup.Key;
            LookAt lookAt = default;
            lookAt.Position = muzzleLogicAccessor.GetBarrelOriginWorld();
            lookAt.Forward = muzzleLogicAccessor.GetFireDirectionWorld();
            lookAt.Left = muzzleLogicAccessor.GetLeftDirectionWorld();
            lookAt.Up = muzzleLogicAccessor.GetUpDirectionWorld();
            LookAt lookAt2 = lookAt;
            IEnumerator<TargetTankNode> enumerator = targetTankNodes.GetEnumerator();

            while (enumerator.MoveNext()) {
                TargetTankNode current = enumerator.Current;

                if (!ownerTankNode.Entity.Equals(current.Entity) && (!flag || num != current.Entity.GetComponent<TeamGroupComponent>().Key)) {
                    BoxCollider boxCollider = (BoxCollider)current.tankColliders.TankToTankCollider;
                    LookTo lookTo = default;
                    lookTo.Position = boxCollider.bounds.center;
                    lookTo.Radius = CalculateTankMinimalRadius(lookAt2.Forward, boxCollider);
                    LookTo lookTo2 = lookTo;
                    AddTargetSector(lookAt2, lookTo2, e.TargetingCone, e.TargetSectors, e.VAllowableAngleAcatter, e.HAllowableAngleAcatter);
                }
            }
        }

        void AddTargetSector(LookAt lookAt, LookTo lookTo, TargetingCone targetingCone, ICollection<TargetSector> sectors, float vDelta, float hDelta) {
            Vector3 lhs = lookTo.Position - lookAt.Position;
            float magnitude = lhs.magnitude;

            if (magnitude > targetingCone.Distance || magnitude < lookTo.Radius) {
                return;
            }

            float num = (float)(Math.Asin(lookTo.Radius / magnitude) * 180.0 / Math.PI);
            float num2 = num + vDelta;
            float num3 = num + hDelta;
            float num4 = Vector3.Dot(lhs, lookAt.Left);
            float num5 = Vector3.Dot(lhs, lookAt.Forward);
            float num6 = Vector3.Dot(lhs, lookAt.Up);
            float num7 = (float)(Math.Atan2(num4, num5) * 180.0 / Math.PI);

            if (!(num7 < 0f - (num3 + targetingCone.HAngle)) && !(num7 > targetingCone.HAngle + num3)) {
                float num8 = (float)(Math.Atan2(num6, num5) * 180.0 / Math.PI);
                float num9 = Math.Max(num8 - num2, 0f - targetingCone.VAngleDown);
                float num10 = Math.Min(num8 + num2, targetingCone.VAngleUp);

                if (num9 < num10) {
                    TargetSector targetSector = default;
                    targetSector.Down = num9;
                    targetSector.Up = num10;
                    targetSector.Distance = magnitude;
                    TargetSector item = targetSector;
                    sectors.Add(item);
                }
            }
        }

        float CalculateTankMinimalRadius(Vector3 forward, BoxCollider collider) => collider.size.magnitude * 0.5f;

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public WeaponInstanceComponent weaponInstance;
        }

        public class OwnerTankNode : Node {
            public TankComponent tank;
        }

        public class TargetTankNode : Node {
            public RigidbodyComponent rigidbody;
            public TankActiveStateComponent tankActiveState;

            public TankCollidersComponent tankColliders;
        }

        public class TeamNode : Node {
            public TeamComponent team;

            public TeamGroupComponent teamGroup;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct LookAt {
            public Vector3 Position { get; set; }

            public Vector3 Left { get; set; }

            public Vector3 Forward { get; set; }

            public Vector3 Up { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct LookTo {
            public Vector3 Position { get; set; }

            public float Radius { get; set; }
        }
    }
}