using MIConvexHull;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class Vertex : IVertex {
        public Vertex(Vector3 unityVertex) {
            UnityVertex = unityVertex;
            Position = new double[3] { unityVertex.x, unityVertex.y, unityVertex.z };
        }

        public Vector3 UnityVertex { get; }

        public int Index { get; set; }

        public double[] Position { get; }
    }
}