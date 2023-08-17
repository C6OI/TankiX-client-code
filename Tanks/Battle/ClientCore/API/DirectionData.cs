using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class DirectionData {
        public DirectionData() => Targets = new List<TargetData>();

        public float Priority { get; set; }

        public Vector3 Origin { get; set; }

        public Vector3 Dir { get; set; }

        public float Angle { get; set; }

        public bool Extra { get; set; }

        public List<TargetData> Targets { get; set; }

        public StaticHit StaticHit { get; set; }

        public DirectionData Init() => Init(Vector3.zero, Vector3.zero, 0f);

        public DirectionData Init(Vector3 origin, Vector3 dir, float angle) {
            Priority = 0f;
            Origin = origin;
            Dir = dir;
            Angle = angle;
            Extra = false;
            Targets.Clear();
            StaticHit = null;
            return this;
        }

        public bool HasAnyHit() => HasTargetHit() || HasStaticHit();

        public bool HasTargetHit() => Targets.Count > 0;

        public bool HasStaticHit() => StaticHit != null;

        public override string ToString() => string.Format(
            "Priority: {0}, Origin: {1}, Dir: {2}, Angle: {3}, Targets: {4}, StaticHit: {5}, Extra: {6}",
            Priority,
            Origin,
            Dir,
            Angle,
            string.Join(",", Targets.Select(t => t.ToString()).ToArray()),
            StaticHit,
            Extra);
    }
}