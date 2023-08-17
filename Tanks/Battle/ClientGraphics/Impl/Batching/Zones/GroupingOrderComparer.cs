using System.Collections.Generic;

namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones {
    public class GroupingOrderComparer : Comparer<Submesh> {
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

            return -a.nearValue.CompareTo(b.nearValue);
        }
    }
}