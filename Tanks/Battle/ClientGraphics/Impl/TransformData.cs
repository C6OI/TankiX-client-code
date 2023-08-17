using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public struct TransformData : IEquatable<TransformData> {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public bool Equals(TransformData data) => Position == data.Position && Rotation == data.Rotation;

        public override bool Equals(object obj) {
            if (obj is TransformData) {
                return Equals((TransformData)obj);
            }

            return false;
        }

        public override int GetHashCode() => Position.GetHashCode() ^ Rotation.GetHashCode();

        public static bool operator ==(TransformData transformData1, TransformData transformData2) =>
            transformData1.Equals(transformData2);

        public static bool operator !=(TransformData transformData1, TransformData transformData2) =>
            !transformData1.Equals(transformData2);
    }
}