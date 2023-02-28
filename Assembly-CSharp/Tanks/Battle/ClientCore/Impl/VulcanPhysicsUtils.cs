using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public static class VulcanPhysicsUtils {
        public static void ApplyVulcanForce(Vector3 force, Rigidbody body, Vector3 pos, TankFallingComponent tankFalling, TrackComponent tracks) {
            body.AddForceAtPositionSafe(force, pos);

            if (!tankFalling.IsGrounded) {
                int num = tracks.LeftTrack.numContacts + tracks.RightTrack.numContacts;

                if (num <= 0) {
                    body.AddForceSafe(-force);
                }
            }
        }
    }
}