using System;
using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics {
    public class MeshBuffersCache {
        const string SURFACE_TYPE_PROPERTY = "_SurfaceType";

        readonly Dictionary<Mesh, Vector3[]> normalsCache = new();

        readonly Dictionary<Mesh, SurfaceType[]> surfaceTypesCache = new();

        readonly Dictionary<Mesh, Vector4[]> tangentsCache = new();

        readonly Dictionary<Mesh, Vector3[]> triangleMidlesCache = new();

        readonly Dictionary<Mesh, float[]> triangleRadiusCache = new();

        readonly Dictionary<Mesh, int[]> trianglesCache = new();

        readonly Dictionary<Mesh, Vector3[]> verticesCache = new();

        public int[] GetTriangles(Mesh mesh) {
            int[] value;

            if (!trianglesCache.TryGetValue(mesh, out value)) {
                value = mesh.triangles;
                trianglesCache.Add(mesh, value);
            }

            return value;
        }

        public Vector3[] GetVertices(Mesh mesh) {
            Vector3[] value;

            if (!verticesCache.TryGetValue(mesh, out value)) {
                value = mesh.vertices;
                verticesCache.Add(mesh, value);
            }

            return value;
        }

        public Vector3[] GetNormals(Mesh mesh) {
            Vector3[] value;

            if (!normalsCache.TryGetValue(mesh, out value)) {
                value = mesh.normals;
                normalsCache.Add(mesh, value);
            }

            return value;
        }

        public Vector4[] GetTangents(Mesh mesh) {
            Vector4[] value;

            if (!tangentsCache.TryGetValue(mesh, out value)) {
                value = mesh.tangents;
                tangentsCache.Add(mesh, value);
            }

            return value;
        }

        public void GetTriangleRadius(Mesh mesh, out float[] triangleRadius, out Vector3[] triangleMidles) {
            if (!triangleRadiusCache.TryGetValue(mesh, out triangleRadius)) {
                int[] triangles = GetTriangles(mesh);
                Vector3[] vertices = GetVertices(mesh);
                triangleRadius = new float[triangles.Length / 3];
                triangleMidles = new Vector3[triangles.Length / 3];

                for (int i = 0; i < triangleRadius.Length; i++) {
                    int num = triangles[i * 3];
                    int num2 = triangles[i * 3 + 1];
                    int num3 = triangles[i * 3 + 2];
                    Vector3 vector = vertices[num];
                    Vector3 vector2 = vertices[num2];
                    Vector3 vector3 = vertices[num3];
                    Vector3 vector4 = (vector + vector2 + vector3) / 3f;

                    triangleRadius[i] = Mathf.Max(Mathf.Max((vector4 - vector).magnitude, (vector4 - vector2).magnitude),
                        (vector4 - vector3).magnitude);

                    triangleMidles[i] = vector4;
                }

                triangleMidlesCache.Add(mesh, triangleMidles);
                triangleRadiusCache.Add(mesh, triangleRadius);
            } else {
                triangleMidles = triangleMidlesCache[mesh];
            }
        }

        float CalculateTriangleSphere(Vector3 v1, Vector3 v2, Vector3 v3) {
            Vector3 vector = (v1 + v2 + v3) / 3f;
            return Mathf.Max(Mathf.Max((vector - v1).sqrMagnitude, (vector - v2).sqrMagnitude), (vector - v3).sqrMagnitude);
        }

        public SurfaceType[] GetSurfaceTypes(Mesh mesh, Renderer renderer) {
            SurfaceType[] value;

            if (!surfaceTypesCache.TryGetValue(mesh, out value)) {
                value = CollectSurfaceTypes(mesh, renderer);
                surfaceTypesCache.Add(mesh, value);
            }

            return value;
        }

        SurfaceType[] CollectSurfaceTypes(Mesh mesh, Renderer renderer) {
            SurfaceType[] array = new SurfaceType[GetVertices(mesh).Length];
            int num = Math.Min(renderer.sharedMaterials.Length, mesh.subMeshCount);

            for (int i = 0; i < num; i++) {
                int[] triangles = mesh.GetTriangles(i);

                for (int j = 0; j < triangles.Length; j++) {
                    Material material = renderer.sharedMaterials[i];
                    SurfaceType surfaceType = SurfaceType.UNDEFINED;

                    if (material.HasProperty("_SurfaceType")) {
                        surfaceType = (SurfaceType)material.GetInt("_SurfaceType");
                    }

                    array[triangles[j]] = surfaceType;
                }
            }

            return array;
        }
    }
}