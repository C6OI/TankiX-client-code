namespace MIConvexHull {
    internal sealed class DeferredFace {
        public ConvexFaceInternal Face;

        public int FaceIndex;

        public ConvexFaceInternal OldFace;

        public ConvexFaceInternal Pivot;

        public int PivotIndex;
    }
}