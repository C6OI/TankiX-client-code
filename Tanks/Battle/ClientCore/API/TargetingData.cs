using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class TargetingData {
        public TargetingData() => Directions = new List<DirectionData>(10);

        public Vector3 Origin { get; set; }

        public Vector3 Dir { get; set; }

        public float FullDistance { get; set; }

        public float MaxAngle { get; set; }

        public DirectionData BestDirection { get; set; }

        public int BestDirectionIndex { get; set; }

        public List<DirectionData> Directions { get; set; }

        public TargetingData Init() {
            Directions.Clear();
            Origin = Vector3.zero;
            Dir = Vector3.zero;
            FullDistance = 0f;
            MaxAngle = 0f;
            BestDirection = null;
            BestDirectionIndex = 0;
            return this;
        }

        public bool HasAnyHit() => Directions.Any(direction => direction.HasAnyHit());

        public bool HasTargetHit() => Directions.Any(direction => direction.HasTargetHit());

        public bool HasBaseStaticHit() => Directions.First().HasStaticHit();

        public bool HasStaticHit() => Directions.Any(direction => direction.HasStaticHit());
    }
}