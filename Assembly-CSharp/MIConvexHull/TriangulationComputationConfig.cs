namespace MIConvexHull {
    public class TriangulationComputationConfig : ConvexHullComputationConfig {
        public TriangulationComputationConfig() => ZeroCellVolumeTolerance = 1E-05;

        public double ZeroCellVolumeTolerance { get; set; }
    }
}