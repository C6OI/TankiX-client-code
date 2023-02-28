using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    internal class ObsoleteVertexFinder {
        readonly List<int> m_NeighboringTriangles = new();

        readonly List<float> m_NeighboringVertexDistances = new();

        readonly List<int> m_NeighboringVertexIndices = new();

        readonly List<float> m_NeighboringVertexWeight = new();
        readonly OptimizeTriangleNormals m_TriangleNormals = new();

        public bool IsInternalVertexObsolete(DecalsMesh a_DecalsMesh, int a_VertexIndex, List<OptimizeEdge> a_RedundantInternalEdges) {
            ClearAll();
            ComputeNeighbors(a_DecalsMesh, a_VertexIndex, a_RedundantInternalEdges);
            return IsVertexObsolete(a_DecalsMesh, a_VertexIndex);
        }

        public bool IsExternalVertexObsolete(DecalsMesh a_DecalsMesh, int a_VertexIndex) {
            ClearAll();
            ComputeNeighbors(a_DecalsMesh, a_VertexIndex);
            return IsVertexObsolete(a_DecalsMesh, a_VertexIndex);
        }

        void ComputeNeighbors(DecalsMesh a_DecalsMesh, int a_VertexIndex, List<OptimizeEdge> a_RedundantInternalEdges) {
            ClearNeighbors();

            foreach (OptimizeEdge a_RedundantInternalEdge in a_RedundantInternalEdges) {
                AddNeighborTriangleIfNeeded(a_DecalsMesh, a_VertexIndex, a_RedundantInternalEdge.triangle1Index);
                AddNeighborTriangleIfNeeded(a_DecalsMesh, a_VertexIndex, a_RedundantInternalEdge.triangle2Index);
            }
        }

        void ComputeNeighbors(DecalsMesh a_DecalsMesh, int a_VertexIndex) {
            ClearNeighbors();

            for (int i = a_DecalsMesh.ActiveDecalProjector.DecalsMeshLowerTriangleIndex; i < a_DecalsMesh.Triangles.Count; i += 3) {
                AddNeighborTriangleIfNeeded(a_DecalsMesh, a_VertexIndex, i);
            }
        }

        void AddNeighborTriangleIfNeeded(DecalsMesh a_DecalsMesh, int a_VertexIndex, int a_TriangleIndex) {
            int num = a_DecalsMesh.Triangles[a_TriangleIndex];
            int num2 = a_DecalsMesh.Triangles[a_TriangleIndex + 1];
            int num3 = a_DecalsMesh.Triangles[a_TriangleIndex + 2];
            int num4 = 0;
            int num5 = 0;
            bool flag = true;

            if (a_VertexIndex == num) {
                num4 = num2;
                num5 = num3;
            } else if (a_VertexIndex == num2) {
                num4 = num;
                num5 = num3;
            } else if (a_VertexIndex == num3) {
                num4 = num;
                num5 = num2;
            } else {
                flag = false;
            }

            if (flag && !m_NeighboringTriangles.Contains(a_TriangleIndex)) {
                m_NeighboringTriangles.Add(a_TriangleIndex);

                if (!m_NeighboringVertexIndices.Contains(num4)) {
                    Vector3 a = a_DecalsMesh.Vertices[a_VertexIndex];
                    Vector3 b = a_DecalsMesh.Vertices[num4];
                    float item = Vector3.Distance(a, b);
                    m_NeighboringVertexIndices.Add(num4);
                    m_NeighboringVertexDistances.Add(item);
                }

                if (!m_NeighboringVertexIndices.Contains(num5)) {
                    Vector3 a2 = a_DecalsMesh.Vertices[a_VertexIndex];
                    Vector3 b2 = a_DecalsMesh.Vertices[num5];
                    float item2 = Vector3.Distance(a2, b2);
                    m_NeighboringVertexIndices.Add(num5);
                    m_NeighboringVertexDistances.Add(item2);
                }

                AddTriangleNormal(a_DecalsMesh, a_TriangleIndex);
            }
        }

        void AddTriangleNormal(DecalsMesh a_DecalsMesh, int a_TriangleIndex) {
            if (!m_TriangleNormals.HasTriangleNormal(a_TriangleIndex)) {
                int index = a_DecalsMesh.Triangles[a_TriangleIndex];
                int index2 = a_DecalsMesh.Triangles[a_TriangleIndex + 1];
                int index3 = a_DecalsMesh.Triangles[a_TriangleIndex + 2];
                Vector3 a_Vertex = a_DecalsMesh.Vertices[index];
                Vector3 a_Vertex2 = a_DecalsMesh.Vertices[index2];
                Vector3 a_Vertex3 = a_DecalsMesh.Vertices[index3];
                Vector3 a_TriangleNormal = GeometryUtilities.TriangleNormal(a_Vertex, a_Vertex2, a_Vertex3);
                m_TriangleNormals.AddTriangleNormal(a_TriangleIndex, a_TriangleNormal);
            }
        }

        bool IsVertexObsolete(DecalsMesh a_DecalsMesh, int a_VertexIndex) {
            bool flag = false;

            if (m_NeighboringTriangles.Count > 0) {
                flag = true;
                bool flag2 = false;
                Vector3 vector = Vector3.zero;

                foreach (int neighboringTriangle in m_NeighboringTriangles) {
                    vector = m_TriangleNormals[neighboringTriangle];

                    if (vector != Vector3.zero) {
                        flag2 = true;
                        break;
                    }
                }

                if (flag2) {
                    foreach (int neighboringTriangle2 in m_NeighboringTriangles) {
                        Vector3 vector2 = m_TriangleNormals[neighboringTriangle2];

                        if (vector2 != Vector3.zero) {
                            float a_Float = Vector3.Dot(vector, vector2);

                            if (!MathfExtension.Approximately(a_Float, 1f, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError)) {
                                flag = false;
                                break;
                            }
                        }
                    }
                }

                if (flag) {
                    float num = 0f;

                    for (int i = 0; i < m_NeighboringVertexIndices.Count; i++) {
                        float num2 = m_NeighboringVertexDistances[i];
                        num += num2;
                    }

                    float num3 = 0f;

                    if (!MathfExtension.Approximately(num, 0f, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError)) {
                        num3 = 1f / num;
                    }

                    for (int j = 0; j < m_NeighboringVertexIndices.Count; j++) {
                        float num4 = m_NeighboringVertexDistances[j];

                        if (MathfExtension.Approximately(num4, 0f, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError)) {
                            m_NeighboringVertexWeight.Add(0f);
                            int a_VertexIndex2 = m_NeighboringVertexIndices[j];
                            flag = AreVertexPropertiesIdentical(a_DecalsMesh, a_VertexIndex, a_VertexIndex2);

                            if (!flag) {
                                break;
                            }
                        } else {
                            float item = 1f - num4 / num3;
                            m_NeighboringVertexWeight.Add(item);
                        }
                    }

                    if (flag) {
                        flag = AreWeightedVertexPropertiesApproximatelyVertexProperties(a_DecalsMesh, a_VertexIndex);
                    }
                }
            }

            return flag;
        }

        bool AreVertexPropertiesIdentical(DecalsMesh a_DecalsMesh, int a_VertexIndex1, int a_VertexIndex2) {
            Decals decals = a_DecalsMesh.Decals;
            Vector3 a_Vector = a_DecalsMesh.Vertices[a_VertexIndex1];
            Vector3 a_Vector2 = a_DecalsMesh.Vertices[a_VertexIndex2];
            bool flag = Vector3Extension.Approximately(a_Vector, a_Vector2, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);

            if (flag && decals.CurrentNormalsMode == NormalsMode.Target) {
                Vector3 a_Vector3 = a_DecalsMesh.Normals[a_VertexIndex1];
                Vector3 a_Vector4 = a_DecalsMesh.Normals[a_VertexIndex2];
                flag = Vector3Extension.Approximately(a_Vector3, a_Vector4, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            if (flag && decals.CurrentTangentsMode == TangentsMode.Target) {
                Vector4 vector = a_DecalsMesh.Tangents[a_VertexIndex1];
                Vector4 vector2 = a_DecalsMesh.Tangents[a_VertexIndex2];
                flag = Vector3Extension.Approximately(vector, vector2, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            if (flag && (decals.CurrentUVMode == UVMode.TargetUV || decals.CurrentUVMode == UVMode.TargetUV2)) {
                Vector2 a_Vector5 = a_DecalsMesh.UVs[a_VertexIndex1];
                Vector2 a_Vector6 = a_DecalsMesh.UVs[a_VertexIndex2];
                flag = Vector2Extension.Approximately(a_Vector5, a_Vector6, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            if (flag && (decals.CurrentUV2Mode == UV2Mode.TargetUV || decals.CurrentUV2Mode == UV2Mode.TargetUV2)) {
                Vector2 a_Vector7 = a_DecalsMesh.UV2s[a_VertexIndex1];
                Vector2 a_Vector8 = a_DecalsMesh.UV2s[a_VertexIndex2];
                flag = Vector2Extension.Approximately(a_Vector7, a_Vector8, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            return flag;
        }

        bool AreWeightedVertexPropertiesApproximatelyVertexProperties(DecalsMesh a_DecalsMesh, int a_VertexIndex) {
            bool flag = true;
            Decals decals = a_DecalsMesh.Decals;

            if (flag) {
                Vector3 a_Vector = a_DecalsMesh.Vertices[a_VertexIndex];
                Vector3 zero = Vector3.zero;

                for (int i = 0; i < m_NeighboringVertexIndices.Count; i++) {
                    int index = m_NeighboringVertexIndices[i];
                    float num = m_NeighboringVertexWeight[i];
                    Vector3 vector = a_DecalsMesh.Vertices[index];
                    zero += num * vector;
                }

                flag = flag && Vector3Extension.Approximately(a_Vector, zero, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            if (flag && decals.CurrentNormalsMode == NormalsMode.Target) {
                Vector3 a_Vector2 = a_DecalsMesh.Normals[a_VertexIndex];
                Vector3 zero2 = Vector3.zero;

                for (int j = 0; j < m_NeighboringVertexIndices.Count; j++) {
                    int index2 = m_NeighboringVertexIndices[j];
                    float num2 = m_NeighboringVertexWeight[j];
                    Vector3 vector2 = a_DecalsMesh.Normals[index2];
                    zero2 += num2 * vector2;
                    zero2.Normalize();
                }

                flag = flag && Vector3Extension.Approximately(a_Vector2, zero2, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            if (flag && decals.CurrentTangentsMode == TangentsMode.Target) {
                Vector4 a_Vector3 = a_DecalsMesh.Tangents[a_VertexIndex];
                Vector4 a_Vector4 = Vector3.zero;

                for (int k = 0; k < m_NeighboringVertexIndices.Count; k++) {
                    int index3 = m_NeighboringVertexIndices[k];
                    float num3 = m_NeighboringVertexWeight[k];
                    Vector4 vector3 = a_DecalsMesh.Tangents[index3];
                    a_Vector4 += num3 * vector3;
                    a_Vector4.Normalize();
                }

                flag = flag && Vector4Extension.Approximately(a_Vector3, a_Vector4, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            if (flag && (decals.CurrentUVMode == UVMode.TargetUV || decals.CurrentUVMode == UVMode.TargetUV2)) {
                Vector2 a_Vector5 = a_DecalsMesh.UVs[a_VertexIndex];
                Vector2 a_Vector6 = Vector3.zero;

                for (int l = 0; l < m_NeighboringVertexIndices.Count; l++) {
                    int index4 = m_NeighboringVertexIndices[l];
                    float num4 = m_NeighboringVertexWeight[l];
                    Vector2 vector4 = a_DecalsMesh.UVs[index4];
                    a_Vector6 += num4 * vector4;
                }

                flag = flag && Vector2Extension.Approximately(a_Vector5, a_Vector6, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            if (flag && (decals.CurrentUV2Mode == UV2Mode.TargetUV || decals.CurrentUV2Mode == UV2Mode.TargetUV2)) {
                Vector2 a_Vector7 = a_DecalsMesh.UV2s[a_VertexIndex];
                Vector2 a_Vector8 = Vector3.zero;

                for (int m = 0; m < m_NeighboringVertexIndices.Count; m++) {
                    int index5 = m_NeighboringVertexIndices[m];
                    float num5 = m_NeighboringVertexWeight[m];
                    Vector2 vector5 = a_DecalsMesh.UV2s[index5];
                    a_Vector8 += num5 * vector5;
                }

                flag = flag && Vector2Extension.Approximately(a_Vector7, a_Vector8, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }

            return true;
        }

        public void ClearAll() {
            m_TriangleNormals.Clear();
            ClearNeighbors();
        }

        void ClearNeighbors() {
            m_NeighboringTriangles.Clear();
            m_NeighboringVertexIndices.Clear();
            m_NeighboringVertexDistances.Clear();
            m_NeighboringVertexWeight.Clear();
        }
    }
}