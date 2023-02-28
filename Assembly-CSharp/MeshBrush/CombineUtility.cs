using UnityEngine;

namespace MeshBrush {
    public class CombineUtility {
        public const string combinedMeshName = "Combined Mesh";

        static int vertexCount;

        static int triangleCount;

        static int stripCount;

        static int curStripCount;

        static Vector3[] vertices;

        static Vector3[] normals;

        static Vector4[] tangents;

        static Vector2[] uv;

        static Vector2[] uv1;

        static Color[] colors;

        static int[] triangles;

        static int[] strip;

        static int offset;

        static int triangleOffset;

        static int stripOffset;

        static int vertexOffset;

        static Matrix4x4 invTranspose;

        static Vector4 p4;

        static Vector3 p;

        public static Mesh Combine(MeshInstance[] combines, bool generateStrips) {
            vertexCount = 0;
            triangleCount = 0;
            stripCount = 0;

            for (int i = 0; i < combines.Length; i++) {
                MeshInstance meshInstance = combines[i];

                if (!meshInstance.mesh) {
                    continue;
                }

                vertexCount += meshInstance.mesh.vertexCount;

                if (!generateStrips) {
                    continue;
                }

                curStripCount = meshInstance.mesh.GetTriangles(meshInstance.subMeshIndex).Length;

                if (curStripCount != 0) {
                    if (stripCount != 0) {
                        if ((stripCount & 1) == 1) {
                            stripCount += 3;
                        } else {
                            stripCount += 2;
                        }
                    }

                    stripCount += curStripCount;
                } else {
                    generateStrips = false;
                }
            }

            if (!generateStrips) {
                for (int j = 0; j < combines.Length; j++) {
                    MeshInstance meshInstance2 = combines[j];

                    if ((bool)meshInstance2.mesh) {
                        triangleCount += meshInstance2.mesh.GetTriangles(meshInstance2.subMeshIndex).Length;
                    }
                }
            }

            vertices = new Vector3[vertexCount];
            normals = new Vector3[vertexCount];
            tangents = new Vector4[vertexCount];
            uv = new Vector2[vertexCount];
            uv1 = new Vector2[vertexCount];
            colors = new Color[vertexCount];
            triangles = new int[triangleCount];
            strip = new int[stripCount];
            offset = 0;

            for (int k = 0; k < combines.Length; k++) {
                MeshInstance meshInstance3 = combines[k];

                if ((bool)meshInstance3.mesh) {
                    Copy(meshInstance3.mesh.vertexCount, meshInstance3.mesh.vertices, vertices, ref offset, meshInstance3.transform);
                }
            }

            offset = 0;

            for (int l = 0; l < combines.Length; l++) {
                MeshInstance meshInstance4 = combines[l];

                if ((bool)meshInstance4.mesh) {
                    invTranspose = meshInstance4.transform;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyNormal(meshInstance4.mesh.vertexCount, meshInstance4.mesh.normals, normals, ref offset, invTranspose);
                }
            }

            offset = 0;

            for (int m = 0; m < combines.Length; m++) {
                MeshInstance meshInstance5 = combines[m];

                if ((bool)meshInstance5.mesh) {
                    invTranspose = meshInstance5.transform;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyTangents(meshInstance5.mesh.vertexCount, meshInstance5.mesh.tangents, tangents, ref offset, invTranspose);
                }
            }

            offset = 0;

            for (int n = 0; n < combines.Length; n++) {
                MeshInstance meshInstance6 = combines[n];

                if ((bool)meshInstance6.mesh) {
                    Copy(meshInstance6.mesh.vertexCount, meshInstance6.mesh.uv, uv, ref offset);
                }
            }

            offset = 0;

            for (int num = 0; num < combines.Length; num++) {
                MeshInstance meshInstance7 = combines[num];

                if ((bool)meshInstance7.mesh) {
                    Copy(meshInstance7.mesh.vertexCount, meshInstance7.mesh.uv2, uv1, ref offset);
                }
            }

            offset = 0;

            for (int num2 = 0; num2 < combines.Length; num2++) {
                MeshInstance meshInstance8 = combines[num2];

                if ((bool)meshInstance8.mesh) {
                    CopyColors(meshInstance8.mesh.vertexCount, meshInstance8.mesh.colors, colors, ref offset);
                }
            }

            triangleOffset = 0;
            stripOffset = 0;
            vertexOffset = 0;

            for (int num3 = 0; num3 < combines.Length; num3++) {
                MeshInstance meshInstance9 = combines[num3];

                if (!meshInstance9.mesh) {
                    continue;
                }

                if (generateStrips) {
                    int[] array = meshInstance9.mesh.GetTriangles(meshInstance9.subMeshIndex);

                    if (stripOffset != 0) {
                        if ((stripOffset & 1) == 1) {
                            strip[stripOffset] = strip[stripOffset - 1];
                            strip[stripOffset + 1] = array[0] + vertexOffset;
                            strip[stripOffset + 2] = array[0] + vertexOffset;
                            stripOffset += 3;
                        } else {
                            strip[stripOffset] = strip[stripOffset - 1];
                            strip[stripOffset + 1] = array[0] + vertexOffset;
                            stripOffset += 2;
                        }
                    }

                    for (int num4 = 0; num4 < array.Length; num4++) {
                        strip[num4 + stripOffset] = array[num4] + vertexOffset;
                    }

                    stripOffset += array.Length;
                } else {
                    int[] array2 = meshInstance9.mesh.GetTriangles(meshInstance9.subMeshIndex);

                    for (int num5 = 0; num5 < array2.Length; num5++) {
                        triangles[num5 + triangleOffset] = array2[num5] + vertexOffset;
                    }

                    triangleOffset += array2.Length;
                }

                vertexOffset += meshInstance9.mesh.vertexCount;
            }

            Mesh mesh = new();
            mesh.name = "Combined Mesh";
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors = colors;
            mesh.uv = uv;
            mesh.uv2 = uv1;
            mesh.tangents = tangents;

            if (generateStrips) {
                mesh.SetTriangles(strip, 0);
            } else {
                mesh.triangles = triangles;
            }

            return mesh;
        }

        static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform) {
            for (int i = 0; i < src.Length; i++) {
                dst[i + offset] = transform.MultiplyPoint(src[i]);
            }

            offset += vertexcount;
        }

        static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform) {
            for (int i = 0; i < src.Length; i++) {
                dst[i + offset] = transform.MultiplyVector(src[i]).normalized;
            }

            offset += vertexcount;
        }

        static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset) {
            for (int i = 0; i < src.Length; i++) {
                dst[i + offset] = src[i];
            }

            offset += vertexcount;
        }

        static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset) {
            for (int i = 0; i < src.Length; i++) {
                dst[i + offset] = src[i];
            }

            offset += vertexcount;
        }

        static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform) {
            for (int i = 0; i < src.Length; i++) {
                p4 = src[i];
                p = new Vector3(p4.x, p4.y, p4.z);
                p = transform.MultiplyVector(p).normalized;
                dst[i + offset] = new Vector4(p.x, p.y, p.z, p4.w);
            }

            offset += vertexcount;
        }

        public struct MeshInstance {
            public Mesh mesh;

            public int subMeshIndex;

            public Matrix4x4 transform;
        }
    }
}