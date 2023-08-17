using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones {
    public sealed class Zone {
        public Bounds bounds;
        public List<Submesh> contents;

        public Material material => contents[0].material;

        public int lightmapIndex => contents[0].lightmapIndex;
    }
}