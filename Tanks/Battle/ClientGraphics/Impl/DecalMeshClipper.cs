using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DecalMeshClipper {
        readonly PolygonClipper2D polygonClipper = new();
        List<ClipPointData> polygon = new(10);

        public void Clip(DecalProjection projection, MeshBuilder sourceBuilder, MeshBuilder destBuilder) {
            List<int> triangles = sourceBuilder.Triangles;
            List<Vector3> vertices = sourceBuilder.Vertices;
            List<Vector3> normals = sourceBuilder.Normals;
            List<SurfaceType> surfaceTypes = sourceBuilder.SurfaceTypes;
            Quaternion rotation = Quaternion.LookRotation(-projection.ProjectionHit.normal, projection.Up);
            Quaternion quaternion = Quaternion.Inverse(rotation);
            ClipEdge2D[] clipPlane = CalculateClipPlane(projection.HalfSize, projection.HalfSize);

            for (int i = 0; i < triangles.Count; i += 3) {
                polygon.Clear();

                for (int j = 0; j < 3; j++) {
                    int index = triangles[i + j];
                    Vector3 vector = vertices[index];
                    Vector3 normal = normals[index];
                    SurfaceType surfaceType = surfaceTypes[index];
                    vector = quaternion * (vector - projection.ProjectionHit.point);
                    polygon.Add(new ClipPointData(new VertexData(vector, normal, surfaceType)));
                }

                if (ClipByWidthAndHeight(clipPlane, ref polygon) &&
                    ClipByDepth(clipPlane, projection.HalfSize, ref polygon)) {
                    AddAllPolygonPointsAndTransformToWorld(destBuilder,
                        ref polygon,
                        rotation,
                        projection.ProjectionHit.point);

                    Triangulate(projection, destBuilder, ref polygon);
                }
            }
        }

        bool ClipByWidthAndHeight(ClipEdge2D[] clipPlane, ref List<ClipPointData> polygon) {
            polygon = polygonClipper.GetIntersectedPolygon(polygon, clipPlane);
            return polygon.Count > 2;
        }

        bool ClipByDepth(ClipEdge2D[] clipPlane, float depthTest, ref List<ClipPointData> polygon) {
            bool flag = false;

            for (int i = 0; i < polygon.Count; i++) {
                ClipPointData clipPointData = polygon[i];
                Vector3 vertex = clipPointData.vertexData.vertex;

                if (vertex.z > depthTest || vertex.z < 0f - depthTest) {
                    flag = true;
                }

                polygon[i] = clipPointData.ToDepthSpace();
            }

            if (flag) {
                polygon = polygonClipper.GetIntersectedPolygon(polygon, clipPlane);
            }

            return polygon.Count > 2;
        }

        void AddAllPolygonPointsAndTransformToWorld(MeshBuilder builder, ref List<ClipPointData> polygon,
            Quaternion rotation, Vector3 position) {
            for (int i = 0; i < polygon.Count; i++) {
                ClipPointData value = polygon[i];
                Vector3 vertex = value.vertexData.vertex;
                vertex = rotation * vertex + position;
                value.vertexData.vertex = vertex;
                value.index = builder.AddVertex(value.vertexData);
                polygon[i] = value;
            }
        }

        void Triangulate(DecalProjection projection, MeshBuilder builder, ref List<ClipPointData> polygon) {
            List<Vector3> vertices = builder.Vertices;
            int index = polygon[0].index;
            Vector3 vector = vertices[index];

            for (int i = 2; i < polygon.Count; i++) {
                int index2 = polygon[i - 1].index;
                int index3 = polygon[i].index;
                Vector3 vector2 = vertices[index2];
                Vector3 vector3 = vertices[index3];

                if (Vector3.Dot(-projection.Ray.direction, Vector3.Cross(vector - vector2, vector - vector3).normalized) >
                    0f) {
                    builder.AddTriangle(index, index2, index3);
                } else {
                    builder.AddTriangle(index, index3, index2);
                }
            }
        }

        ClipEdge2D[] CalculateClipPlane(float width, float height) {
            Vector2 vector = new(0f - width, 0f - height);
            Vector2 vector2 = new(0f - width, height);
            Vector2 vector3 = new(width, height);
            Vector2 vector4 = new(width, 0f - height);

            return new ClipEdge2D[4] {
                new(vector, vector2),
                new(vector2, vector3),
                new(vector3, vector4),
                new(vector4, vector)
            };
        }
    }
}