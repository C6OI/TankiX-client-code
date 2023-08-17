using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones {
    public sealed class Submesh {
        public Bounds bounds;

        public Material material;
        public bool merged;

        public Mesh mesh;

        public int nearValue;

        public MeshRenderer renderer;

        public int submesh;

        public int lightmapIndex => renderer.lightmapIndex;
    }
}