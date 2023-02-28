using System;
using System.Collections.Generic;
using System.Linq;

namespace MIConvexHull {
    public static class ConvexHull {
        public static ConvexHull<TVertex, TFace> Create<TVertex, TFace>(IList<TVertex> data, ConvexHullComputationConfig config = null)
            where TVertex : IVertex where TFace : ConvexFace<TVertex, TFace>, new() => ConvexHull<TVertex, TFace>.Create(data, config);

        public static ConvexHull<TVertex, DefaultConvexFace<TVertex>> Create<TVertex>(IList<TVertex> data, ConvexHullComputationConfig config = null) where TVertex : IVertex =>
            ConvexHull<TVertex, DefaultConvexFace<TVertex>>.Create(data, config);

        public static ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>> Create(IList<double[]> data, ConvexHullComputationConfig config = null) {
            List<DefaultVertex> data2 = data.Select(p => new DefaultVertex {
                Position = p.ToArray()
            }).ToList();

            return ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>>.Create(data2, config);
        }
    }

    public class ConvexHull<TVertex, TFace> where TVertex : IVertex where TFace : ConvexFace<TVertex, TFace>, new() {
        internal ConvexHull() { }

        public IEnumerable<TVertex> Points { get; internal set; }

        public IEnumerable<TFace> Faces { get; internal set; }

        public static ConvexHull<TVertex, TFace> Create(IList<TVertex> data, ConvexHullComputationConfig config) {
            if (data == null) {
                throw new ArgumentNullException("data");
            }

            return ConvexHullInternal.GetConvexHull<TVertex, TFace>(data, config);
        }
    }
}