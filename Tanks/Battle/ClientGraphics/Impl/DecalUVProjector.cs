using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DecalUVProjector {
        readonly float DEPTH_TO_OFFSET_KOEF = 0.6f;
        readonly float NORMAL_TO_OFFSET_TRESHOLD = 0.4f;

        public void Project(MeshBuilder builder, DecalProjection projection) {
            List<Vector3> normals = builder.Normals;
            List<Vector3> vertices = builder.Vertices;
            List<int> triangles = builder.Triangles;
            List<SurfaceType> surfaceTypes = builder.SurfaceTypes;
            float num = projection.HalfSize * 1.1f;
            float num2 = num * 2f * projection.AtlasHTilesCount;
            float num3 = num * 2f * projection.AtlasVTilesCount;

            Vector3 projectDirectionAsMiddleNormal =
                GetProjectDirectionAsMiddleNormal(projection, triangles, vertices, normals);

            Quaternion rotation = Quaternion.LookRotation(projectDirectionAsMiddleNormal, projection.Up);
            Quaternion quaternion = Quaternion.Inverse(rotation);

            for (int i = 0; i < vertices.Count; i++) {
                int position = projection.SurfaceAtlasPositions[(int)surfaceTypes[i]];
                Vector2 atlasOffset = GetAtlasOffset(projection.AtlasHTilesCount, projection.AtlasVTilesCount, position);
                Vector3 vector = vertices[i];
                Vector3 vector2 = normals[i];
                Vector3 vector3 = quaternion * (vector - projection.ProjectionHit.point);
                Vector3 vector4 = quaternion * vector2;
                vector4.x = Mathf.Abs(vector4.x);
                vector4.y = Mathf.Abs(vector4.y);
                float num4 = Mathf.Abs(vector3.z) * DEPTH_TO_OFFSET_KOEF;
                float num5 = vector3.x;
                float num6 = vector3.y;

                if (vector4.x > NORMAL_TO_OFFSET_TRESHOLD) {
                    num5 += Mathf.Sign(vector3.x) * vector4.x * num4;
                }

                if (vector4.y > NORMAL_TO_OFFSET_TRESHOLD) {
                    num6 += Mathf.Sign(vector3.y) * vector4.y * num4;
                }

                builder.AddUV(new Vector2(num5 / num2 + 0.5f + atlasOffset.x, num6 / num3 + 0.5f + atlasOffset.y));
            }
        }

        Vector2 GetAtlasOffset(int tilesX, int tilesY, int position) {
            Vector2 vector = new(1f / tilesX, 1f / tilesY);
            int num = tilesX * tilesY;
            position %= num;
            int num2 = position % tilesX;
            int num3 = position / tilesY;
            return new Vector2((-tilesX * 0.5f + 0.5f + num2) * vector.x, (tilesY * 0.5f - 0.5f - num3) * vector.y);
        }

        Vector3 GetProjectDirectionAsMiddleNormal(DecalProjection projection, List<int> triangles, List<Vector3> vertices,
            List<Vector3> normals) {
            Vector3 zero = Vector3.zero;

            for (int i = 0; i < triangles.Count; i += 3) {
                int index = triangles[i];
                int index2 = triangles[i + 1];
                int index3 = triangles[i + 2];
                Vector3 v = vertices[index];
                Vector3 v2 = vertices[index2];
                Vector3 v3 = vertices[index3];
                zero += normals[index] * GetTriangleSquare(v, v2, v3);
            }

            if (zero.Equals(Vector3.zero)) {
                return projection.Ray.direction;
            }

            return -zero.normalized;
        }

        float GetTriangleSquare(Vector3 v0, Vector3 v1, Vector3 v2) {
            float magnitude = (v0 - v1).magnitude;
            float magnitude2 = (v0 - v2).magnitude;
            float magnitude3 = (v1 - v2).magnitude;
            float num = (magnitude + magnitude2 + magnitude3) * 0.5f;
            return Mathf.Sqrt(num * (num - magnitude) * (num - magnitude2) * (num - magnitude3));
        }
    }
}