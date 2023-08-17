using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones {
    public class CandidatesComparer : Comparer<Submesh> {
        public Vector3 center;

        public override int Compare(Submesh a, Submesh b) {
            if (a == null) {
                if (b == null) {
                    return 0;
                }

                return -1;
            }

            if (b == null) {
                return 1;
            }

            float sqrMagnitude = (a.bounds.center - center).sqrMagnitude;
            float sqrMagnitude2 = (b.bounds.center - center).sqrMagnitude;
            float sqrMagnitude3 = a.bounds.extents.sqrMagnitude;
            float sqrMagnitude4 = b.bounds.extents.sqrMagnitude;
            float num = sqrMagnitude + sqrMagnitude3;
            float value = sqrMagnitude2 + sqrMagnitude4;
            return num.CompareTo(value);
        }
    }
}