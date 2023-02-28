using System;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public static class MovementComparer {
        const float NEAR_DISTANCE = 0.3f;

        const float NEAR_DISTANCE_SQR = 0.09f;

        const float NEAR_VELOCITY = 0.5f;

        const float NEAR_VELOCITY_SQR = 0.25f;

        const float NEAR_ORIENTATION_DEGREES = 4f;

        const float NEAR_ORIENTATION_RADIANS = (float)Math.PI / 45f;

        const float NEAR_ROTATION_DEGREES = 10f;

        const float NEAR_ROTATION_RADIANS = 0.17453292f;

        public static bool IsMovementAlmostEqual(ref Movement movement1, ref Movement movement2) =>
            CheckDistance(ref movement1, ref movement2) && CheckRotation(ref movement1, ref movement2) && CheckLinearVelocity(ref movement1, ref movement2) &&
            CheckAngularVelocity(ref movement1, ref movement2);

        static bool CheckDistance(ref Movement movement1, ref Movement movement2) => Vector3.SqrMagnitude(movement1.Position - movement2.Position) < 0.09f;

        static bool CheckLinearVelocity(ref Movement movement1, ref Movement movement2) => Vector3.SqrMagnitude(movement1.Velocity - movement2.Velocity) < 0.25f;

        static bool CheckRotation(ref Movement movement1, ref Movement movement2) {
            float num = Quaternion.Angle(movement1.Orientation, movement2.Orientation);
            return num < 4f;
        }

        static bool CheckAngularVelocity(ref Movement movement1, ref Movement movement2) => MathUtil.NearlyEqual(movement2.AngularVelocity, movement1.AngularVelocity, 0.17453292f);
    }
}