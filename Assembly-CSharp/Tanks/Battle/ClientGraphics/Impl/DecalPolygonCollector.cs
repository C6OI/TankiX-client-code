using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DecalPolygonCollector {
        readonly MeshBuffersCache cache;

        public DecalPolygonCollector(MeshBuffersCache meshBuffersCache) => cache = meshBuffersCache;

        public bool Collect(DecalProjection projection, MeshBuilder meshBuilder) {
            Collider[] colliders = GetColliders(projection);

            foreach (Collider collider in colliders) {
                Mesh mesh;

                if (!GetMeshByCollider(collider, out mesh)) {
                    continue;
                }

                meshBuilder.ClearWeldHashing();
                Transform transform = collider.transform;
                Renderer renderer;

                if (!TryGetRenderer(collider.gameObject, out renderer) || renderer.gameObject.CompareTag("IgnoreDecals")) {
                    continue;
                }

                int[] triangles = cache.GetTriangles(mesh);
                Vector3[] vertices = cache.GetVertices(mesh);
                Vector3[] normals = cache.GetNormals(mesh);
                SurfaceType[] surfaceTypes = cache.GetSurfaceTypes(mesh, renderer);
                float[] triangleRadius;
                Vector3[] triangleMidles;
                cache.GetTriangleRadius(mesh, out triangleRadius, out triangleMidles);
                Vector3 vector = transform.InverseTransformDirection(projection.Ray.direction);
                Vector3 vector2 = transform.InverseTransformPoint(projection.ProjectionHit.point);

                for (int j = 0; j < triangles.Length; j += 3) {
                    int num = j / 3;
                    float sqrMagnitude = (triangleMidles[num] - vector2).sqrMagnitude;
                    float num2 = triangleRadius[num] + projection.HalfSize;

                    if (!(sqrMagnitude > num2 * num2)) {
                        int num3 = triangles[j];
                        Vector3 lhs = normals[num3];
                        float num4 = Vector3.Dot(lhs, -vector);

                        if (!(num4 < 0f)) {
                            int num5 = triangles[j + 1];
                            int num6 = triangles[j + 2];
                            Vector3 vertex = vertices[num3];
                            Vector3 vertex2 = vertices[num5];
                            Vector3 vertex3 = vertices[num6];
                            Vector3 normal = normals[num3];
                            Vector3 normal2 = normals[num5];
                            Vector3 normal3 = normals[num6];
                            SurfaceType surfaceType = surfaceTypes[num3];
                            SurfaceType surfaceType2 = surfaceTypes[num5];
                            SurfaceType surfaceType3 = surfaceTypes[num6];
                            num3 = meshBuilder.AddVertexWeldAndTransform(num3, new VertexData(vertex, normal, surfaceType), transform);
                            num5 = meshBuilder.AddVertexWeldAndTransform(num5, new VertexData(vertex2, normal2, surfaceType2), transform);
                            num6 = meshBuilder.AddVertexWeldAndTransform(num6, new VertexData(vertex3, normal3, surfaceType3), transform);
                            meshBuilder.AddTriangle(num3, num5, num6);
                        }
                    }
                }
            }

            return meshBuilder.Triangles.Count > 0;
        }

        bool TryGetRenderer(GameObject gameObject, out Renderer renderer) {
            renderer = null;
            ParentRendererBehaviour component = gameObject.GetComponent<ParentRendererBehaviour>();

            if (component != null) {
                renderer = component.ParentRenderer;
            }

            return renderer != null;
        }

        Collider[] GetColliders(DecalProjection projection) => Physics.OverlapSphere(projection.ProjectionHit.point, projection.HalfSize);

        bool TriangleSphereIntersectSphere(Vector3 triangleV0, Vector3 triangleV1, Vector3 triangleV2, Vector3 spherePosition, float radius) {
            bool flag = false;
            Vector3 vector = (triangleV0 + triangleV1 + triangleV2) / 3f;
            float num = (vector - spherePosition).magnitude - radius;

            if (num < 0f) {
                return true;
            }

            float num2 = num * num;
            return (vector - triangleV0).sqrMagnitude > num2 || (vector - triangleV1).sqrMagnitude > num2 || (vector - triangleV2).sqrMagnitude > num2;
        }

        bool GetMeshByCollider(Collider collider, out Mesh mesh) {
            mesh = null;
            MeshCollider meshCollider = collider as MeshCollider;

            if (meshCollider == null) {
                return false;
            }

            mesh = meshCollider.sharedMesh;

            if (mesh == null) {
                return false;
            }

            return true;
        }
    }
}