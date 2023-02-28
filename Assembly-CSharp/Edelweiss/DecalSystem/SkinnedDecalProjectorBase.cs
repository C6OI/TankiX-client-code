namespace Edelweiss.DecalSystem {
    public abstract class SkinnedDecalProjectorBase : GenericDecalProjector<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh> {
        public int DecalsMeshLowerBonesIndex { get; internal set; }

        public int DecalsMeshUpperBonesIndex { get; internal set; }

        public int DecalsMeshBonesCount => DecalsMeshUpperBonesIndex - DecalsMeshLowerBonesIndex + 1;
    }
}