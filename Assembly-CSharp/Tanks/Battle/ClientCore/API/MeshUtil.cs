using System.Collections.Generic;
using System.Linq;
using MIConvexHull;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public static class MeshUtil {
        public static Mesh CreateConvexMesh(Vector3[] vertices) {
            IList<IVertex> data = ConvertToMIConvexHullVertices(vertices);
            ConvexHull<IVertex, DefaultConvexFace<IVertex>> convexHull = ConvexHull.Create(data);
            Vector3[] vertices2 = ConvertToUnityVertices(convexHull);
            int[] triangles = ConvertToUnityTriangles(convexHull);
            Mesh mesh = new();
            mesh.vertices = vertices2;
            mesh.triangles = triangles;
            return mesh;
        }

        static IList<IVertex> ConvertToMIConvexHullVertices(Vector3[] vertices) {
            IList<IVertex> list = new List<IVertex>(vertices.Length);

            for (int i = 0; i < vertices.Length; i++) {
                Vertex item = new(vertices[i]);
                list.Add(item);
            }

            return list;
        }

        static int[] ConvertToUnityTriangles(ConvexHull<IVertex, DefaultConvexFace<IVertex>> convexHull) {
            IEnumerable<DefaultConvexFace<IVertex>> faces = convexHull.Faces;
            DefaultConvexFace<IVertex>[] array = faces.ToArray();
            int[] array2 = new int[array.Length * 3];

            for (int i = 0; i < array.Length; i++) {
                for (int j = 0; j < 3; j++) {
                    array2[i * 3 + j] = ((Vertex)array[i].Vertices[j]).Index;
                }
            }

            return array2;
        }

        static Vector3[] ConvertToUnityVertices(ConvexHull<IVertex, DefaultConvexFace<IVertex>> convexHull) {
            IVertex[] array = convexHull.Points.ToArray();
            Vector3[] array2 = new Vector3[array.Length];

            for (int i = 0; i < array.Length; i++) {
                Vertex vertex = (Vertex)array[i];
                array2[i] = vertex.UnityVertex;
                vertex.Index = i;
            }

            return array2;
        }
    }
}