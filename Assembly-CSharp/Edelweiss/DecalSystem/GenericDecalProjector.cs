namespace Edelweiss.DecalSystem {
    public abstract class GenericDecalProjector<D, P, DM> : GenericDecalProjectorBase where D : GenericDecals<D, P, DM>
        where P : GenericDecalProjector<D, P, DM>
        where DM : GenericDecalsMesh<D, P, DM> {
        public DM DecalsMesh { get; internal set; }

        internal void ResetDecalsMesh() {
            DecalsMesh = null;
            IsActiveProjector = false;
            DecalsMeshLowerVertexIndex = 0;
            DecalsMeshUpperVertexIndex = 0;
            IsUV1ProjectionCalculated = false;
            IsUV2ProjectionCalculated = false;
            IsTangentProjectionCalculated = false;
        }
    }
}