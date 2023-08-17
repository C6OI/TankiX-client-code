using UnityEngine;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public struct Movement {
        public Vector3 Position { get; set; }

        public Vector3 Velocity { get; set; }

        public Vector3 AngularVelocity { get; set; }

        public Quaternion Orientation { get; set; }

        public override string ToString() =>
            string.Format("[Movement Position: {0}, Velocity: {1}, AngularVelocity: {2}, Orientation: {3}]",
                Position,
                Velocity,
                AngularVelocity,
                Orientation);
    }
}