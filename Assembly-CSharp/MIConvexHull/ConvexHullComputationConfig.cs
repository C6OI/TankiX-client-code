using System;

namespace MIConvexHull {
    public class ConvexHullComputationConfig {
        public ConvexHullComputationConfig() {
            PlaneDistanceTolerance = 1E-05;
            PointTranslationType = PointTranslationType.None;
            PointTranslationGenerator = null;
        }

        public double PlaneDistanceTolerance { get; set; }

        public PointTranslationType PointTranslationType { get; set; }

        public Func<double> PointTranslationGenerator { get; set; }

        static Func<double> Closure(double radius, Random rnd) {
            return () => radius * (rnd.NextDouble() - 0.5);
        }

        public static Func<double> RandomShiftByRadius(double radius = 1E-06, int? randomSeed = null) {
            Random rnd = !randomSeed.HasValue ? new Random() : new Random(randomSeed.Value);
            return Closure(radius, rnd);
        }
    }
}