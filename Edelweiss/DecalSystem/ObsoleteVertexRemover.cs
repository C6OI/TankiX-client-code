using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    class ObsoleteVertexRemover {
        readonly List<int> m_NeighboringTriangles = new();

        readonly List<int> m_SortedNeighboringVertices = new();

        readonly List<int> m_UnusedNeighboringTriangles = new();
        Vector3 m_ReferenceTriangleNormal;

        bool m_SortedNeighboringVerticesInitialized;

        internal void RemoveObsoleteInternalVertex(DecalsMesh a_DecalsMesh, int a_VertexIndex,
            List<OptimizeEdge> a_RedundantInternalEdges, out bool a_SuccessfulRemoval) {
            InitializeNeighboringTriangles(a_DecalsMesh, a_VertexIndex);
            InitializeSortedNeighboringInternalVertices(a_DecalsMesh, a_VertexIndex);

            if (m_SortedNeighboringVerticesInitialized) {
                RemoveUnneededTriangles(a_DecalsMesh);
                AddNewTriangles(a_DecalsMesh);
                DecalProjectorBase activeDecalProjector = a_DecalsMesh.ActiveDecalProjector;
                activeDecalProjector.DecalsMeshUpperTriangleIndex = a_DecalsMesh.Triangles.Count - 1;
                a_SuccessfulRemoval = true;
            } else {
                a_SuccessfulRemoval = false;
            }

            Clear();
        }

        internal void RemoveObsoleteExternalVertex(DecalsMesh a_DecalsMesh, int a_VertexIndex,
            out bool a_SuccessfulRemoval) {
            InitializeNeighboringTriangles(a_DecalsMesh, a_VertexIndex);
            InitializeSortedNeighboringExternalVertices(a_DecalsMesh, a_VertexIndex);

            if (m_SortedNeighboringVerticesInitialized) {
                RemoveUnneededTriangles(a_DecalsMesh);
                AddNewTriangles(a_DecalsMesh);
                DecalProjectorBase activeDecalProjector = a_DecalsMesh.ActiveDecalProjector;
                activeDecalProjector.DecalsMeshUpperTriangleIndex = a_DecalsMesh.Triangles.Count - 1;
                a_SuccessfulRemoval = true;
            } else {
                a_SuccessfulRemoval = false;
            }

            Clear();
        }

        void InitializeNeighboringTriangles(DecalsMesh a_DecalsMesh, int a_VertexIndex) {
            DecalProjectorBase activeDecalProjector = a_DecalsMesh.ActiveDecalProjector;
            bool flag = false;

            for (int i = activeDecalProjector.DecalsMeshLowerTriangleIndex; i < a_DecalsMesh.Triangles.Count; i += 3) {
                int num = a_DecalsMesh.Triangles[i];
                int num2 = a_DecalsMesh.Triangles[i + 1];
                int num3 = a_DecalsMesh.Triangles[i + 2];

                if (num != a_VertexIndex && num2 != a_VertexIndex && num3 != a_VertexIndex) {
                    continue;
                }

                m_NeighboringTriangles.Add(i);

                if (!flag) {
                    Vector3 a_Vertex = a_DecalsMesh.Vertices[num];
                    Vector3 a_Vertex2 = a_DecalsMesh.Vertices[num2];
                    Vector3 a_Vertex3 = a_DecalsMesh.Vertices[num3];
                    m_ReferenceTriangleNormal = GeometryUtilities.TriangleNormal(a_Vertex, a_Vertex2, a_Vertex3);

                    if (!Vector3Extension.Approximately(m_ReferenceTriangleNormal,
                            Vector3.zero,
                            DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError,
                            DecalsMeshMinimizer.s_CurrentMaximumRelativeError)) {
                        flag = true;
                    }
                }
            }
        }

        void InitializeSortedNeighboringInternalVertices(DecalsMesh a_DecalsMesh, int a_VertexIndex) {
            m_SortedNeighboringVerticesInitialized = true;
            m_UnusedNeighboringTriangles.AddRange(m_NeighboringTriangles);
            int num = m_UnusedNeighboringTriangles[m_UnusedNeighboringTriangles.Count - 1];
            int a_InnerTriangleVertexIndex = InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num, a_VertexIndex);
            a_InnerTriangleVertexIndex = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex);
            m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + a_InnerTriangleVertexIndex]);
            a_InnerTriangleVertexIndex = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex);
            m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + a_InnerTriangleVertexIndex]);
            m_UnusedNeighboringTriangles.RemoveAt(m_UnusedNeighboringTriangles.Count - 1);

            while (m_UnusedNeighboringTriangles.Count > 1) {
                bool flag = false;
                int num2 = m_SortedNeighboringVertices[m_SortedNeighboringVertices.Count - 1];

                for (int i = 0; i < m_UnusedNeighboringTriangles.Count; i++) {
                    int num3 = m_UnusedNeighboringTriangles[i];
                    int a_InnerTriangleVertexIndex2 = InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num3, a_VertexIndex);
                    a_InnerTriangleVertexIndex2 = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex2);
                    int num4 = a_DecalsMesh.Triangles[num3 + a_InnerTriangleVertexIndex2];

                    if (num4 == num2) {
                        a_InnerTriangleVertexIndex2 = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex2);
                        int item = a_DecalsMesh.Triangles[num3 + a_InnerTriangleVertexIndex2];
                        m_SortedNeighboringVertices.Add(item);
                        m_UnusedNeighboringTriangles.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }

                if (!flag) {
                    m_SortedNeighboringVerticesInitialized = false;
                    break;
                }
            }

            if (m_SortedNeighboringVerticesInitialized) {
                int num5 = m_SortedNeighboringVertices[m_SortedNeighboringVertices.Count - 1];
                int num6 = m_UnusedNeighboringTriangles[0];
                int a_InnerTriangleVertexIndex3 = InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num6, a_VertexIndex);
                a_InnerTriangleVertexIndex3 = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex3);

                if (num5 != a_DecalsMesh.Triangles[num6 + a_InnerTriangleVertexIndex3]) {
                    throw new InvalidOperationException("The last triangle doesn't match the previous ones.");
                }

                a_InnerTriangleVertexIndex3 = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex3);
                int num7 = m_SortedNeighboringVertices[0];

                if (num7 != a_DecalsMesh.Triangles[num6 + a_InnerTriangleVertexIndex3]) {
                    throw new InvalidOperationException("The last triangle doesn't match to the first one.");
                }
            }
        }

        void InitializeSortedNeighboringExternalVertices(DecalsMesh a_DecalsMesh, int a_VertexIndex) {
            m_SortedNeighboringVerticesInitialized = true;
            m_UnusedNeighboringTriangles.AddRange(m_NeighboringTriangles);

            if (m_UnusedNeighboringTriangles.Count <= 0) {
                m_SortedNeighboringVerticesInitialized = false;
                return;
            }

            int num = m_UnusedNeighboringTriangles[m_UnusedNeighboringTriangles.Count - 1];
            int a_InnerTriangleVertexIndex = InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num, a_VertexIndex);
            a_InnerTriangleVertexIndex = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex);
            m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + a_InnerTriangleVertexIndex]);
            a_InnerTriangleVertexIndex = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex);
            m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + a_InnerTriangleVertexIndex]);
            m_UnusedNeighboringTriangles.RemoveAt(m_UnusedNeighboringTriangles.Count - 1);

            while (m_UnusedNeighboringTriangles.Count != 0) {
                bool flag = false;
                int num2 = m_SortedNeighboringVertices[0];
                int num3 = m_SortedNeighboringVertices[m_SortedNeighboringVertices.Count - 1];

                for (int i = 0; i < m_UnusedNeighboringTriangles.Count; i++) {
                    int num4 = m_UnusedNeighboringTriangles[i];
                    int a_InnerTriangleVertexIndex2 = InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num4, a_VertexIndex);
                    a_InnerTriangleVertexIndex2 = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex2);
                    int num5 = a_DecalsMesh.Triangles[num4 + a_InnerTriangleVertexIndex2];

                    if (num5 == num3) {
                        a_InnerTriangleVertexIndex2 = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex2);
                        int item = a_DecalsMesh.Triangles[num4 + a_InnerTriangleVertexIndex2];
                        m_SortedNeighboringVertices.Add(item);
                        m_UnusedNeighboringTriangles.RemoveAt(i);
                        flag = true;
                        break;
                    }

                    int num6 = a_InnerTriangleVertexIndex2;
                    a_InnerTriangleVertexIndex2 = SuccessorInnerTriangleVertexIndex(a_InnerTriangleVertexIndex2);
                    num5 = a_DecalsMesh.Triangles[num4 + a_InnerTriangleVertexIndex2];

                    if (num5 == num2) {
                        int item2 = a_DecalsMesh.Triangles[num4 + num6];
                        m_SortedNeighboringVertices.Insert(0, item2);
                        m_UnusedNeighboringTriangles.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }

                if (!flag) {
                    m_SortedNeighboringVerticesInitialized = false;
                    break;
                }
            }
        }

        int InnerTriangleIndexOfVertexIndex(DecalsMesh a_DecalsMesh, int a_TriangleIndex, int a_VertexIndex) {
            if (a_DecalsMesh.Triangles[a_TriangleIndex] == a_VertexIndex) {
                return 0;
            }

            if (a_DecalsMesh.Triangles[a_TriangleIndex + 1] == a_VertexIndex) {
                return 1;
            }

            if (a_DecalsMesh.Triangles[a_TriangleIndex + 2] == a_VertexIndex) {
                return 2;
            }

            throw new InvalidOperationException("The vertex index argument is not in the provided triangle.");
        }

        int SuccessorInnerTriangleVertexIndex(int a_InnerTriangleVertexIndex) {
            int num = a_InnerTriangleVertexIndex + 1;

            if (num == 3) {
                num = 0;
            }

            return num;
        }

        void RemoveUnneededTriangles(DecalsMesh a_DecalsMesh) {
            m_NeighboringTriangles.Sort();

            for (int num = m_NeighboringTriangles.Count - 1; num >= 0; num--) {
                int num2 = m_NeighboringTriangles[num];
                int num3 = 1;

                while (num > 0 && num2 - 3 == m_NeighboringTriangles[num - 1]) {
                    num2 -= 3;
                    num3++;
                    num--;
                }

                a_DecalsMesh.RemoveTrianglesAt(num2, num3);
            }
        }

        void AddNewTriangles(DecalsMesh a_DecalsMesh) {
            while (m_SortedNeighboringVertices.Count >= 3) {
                bool flag = false;

                for (int i = 0; i < m_SortedNeighboringVertices.Count; i++) {
                    int index = i;
                    int num = i + 1;
                    int num2 = i + 2;

                    if (num >= m_SortedNeighboringVertices.Count) {
                        num -= m_SortedNeighboringVertices.Count;
                        num2 -= m_SortedNeighboringVertices.Count;
                    } else if (num2 >= m_SortedNeighboringVertices.Count) {
                        num2 -= m_SortedNeighboringVertices.Count;
                    }

                    int num3 = m_SortedNeighboringVertices[index];
                    int num4 = m_SortedNeighboringVertices[num];
                    int num5 = m_SortedNeighboringVertices[num2];
                    Vector3 a_Vertex = a_DecalsMesh.Vertices[num3];
                    Vector3 a_Vertex2 = a_DecalsMesh.Vertices[num4];
                    Vector3 a_Vertex3 = a_DecalsMesh.Vertices[num5];
                    Vector3 rhs = GeometryUtilities.TriangleNormal(a_Vertex, a_Vertex2, a_Vertex3);
                    float num6 = Vector3.Dot(m_ReferenceTriangleNormal, rhs);

                    if (num6 >= 0f) {
                        a_DecalsMesh.Triangles.Add(num3);
                        a_DecalsMesh.Triangles.Add(num4);
                        a_DecalsMesh.Triangles.Add(num5);
                        m_SortedNeighboringVertices.RemoveAt(num);
                        flag = true;
                        break;
                    }
                }

                if (!flag) {
                    break;
                }
            }
        }

        void Clear() {
            m_NeighboringTriangles.Clear();
            m_SortedNeighboringVertices.Clear();
            m_UnusedNeighboringTriangles.Clear();
        }
    }
}