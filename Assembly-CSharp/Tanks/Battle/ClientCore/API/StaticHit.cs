using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class StaticHit {
        public Vector3 Position { get; set; }

        public Vector3 Normal { get; set; }

        public override string ToString() => string.Format("Position: {0}, Normal: {1}", Position, Normal);
    }
}