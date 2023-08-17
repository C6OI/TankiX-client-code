using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    public class DecalsMeshMinimizer {
        internal static float s_CurrentMaximumAbsoluteError = 0.0001f;

        internal static float s_CurrentMaximumRelativeError = 0.0001f;

        readonly SortedDictionary<int, int> m_NonInternalVertexIndices = new();

        readonly List<int> m_ObsoleteExternalVertexIndices = new();

        readonly List<int> m_ObsoleteInternalVertexIndices = new();

        readonly ObsoleteVertexFinder m_ObsoleteVertexFinder = new();

        readonly ObsoleteVertexRemover m_ObsoleteVertexRemover = new();

        readonly OptimizeEdges m_OverusedInternalEdges = new();

        readonly OptimizeEdges m_RedundantExternalEdges = new();

        readonly OptimizeEdges m_RedundantInternalEdges = new();

        readonly RemovedIndices m_RemovedIndices = new();

        public void MinimizeActiveProjectorOfDecalsMesh(DecalsMesh a_DecalsMesh) {
            if (a_DecalsMesh == null) {
                throw new ArgumentNullException("Decals mesh argument is not allowed to be null.");
            }

            float meshMinimizerMaximumAbsoluteError = a_DecalsMesh.Decals.MeshMinimizerMaximumAbsoluteError;
            float meshMinimizerMaximumRelativeError = a_DecalsMesh.Decals.MeshMinimizerMaximumRelativeError;

            MinimizeActiveProjectorOfDecalsMesh(a_DecalsMesh,
                meshMinimizerMaximumAbsoluteError,
                meshMinimizerMaximumRelativeError);
        }

        public void MinimizeActiveProjectorOfDecalsMesh(DecalsMesh a_DecalsMesh, float a_MaximumAbsoluteError,
            float a_MaximumRelativeError) {
            if (a_DecalsMesh == null) {
                throw new ArgumentNullException("Decals mesh argument is not allowed to be null.");
            }

            if (a_DecalsMesh.ActiveDecalProjector == null) {
                throw new ArgumentNullException("Active decal projector of decals mesh is not allowed to be null.");
            }

            if (a_MaximumAbsoluteError < 0f) {
                throw new ArgumentOutOfRangeException("The maximum absolute error has to be greater than zero.");
            }

            if (a_MaximumRelativeError < 0f || a_MaximumRelativeError > 1f) {
                throw new ArgumentOutOfRangeException("The maximum relative error has to be within [0.0f, 1.0f].");
            }

            ClearAll();
            s_CurrentMaximumAbsoluteError = a_MaximumAbsoluteError;
            s_CurrentMaximumRelativeError = a_MaximumRelativeError;
            ComputePotentialObsoleteVertices(a_DecalsMesh);
            ComputeObsoleteInternalVertices(a_DecalsMesh);
            ComputeObsoleteExternalVertices(a_DecalsMesh);
            RemoveObsoleteVertices(a_DecalsMesh);
            ClearAll();
        }

        void ComputePotentialObsoleteVertices(DecalsMesh a_DecalsMesh) {
            DecalProjectorBase activeDecalProjector = a_DecalsMesh.ActiveDecalProjector;
            ClearAll();

            for (int i = activeDecalProjector.DecalsMeshLowerTriangleIndex;
                 i <= activeDecalProjector.DecalsMeshUpperTriangleIndex;
                 i += 3) {
                int num = a_DecalsMesh.Triangles[i];
                int num2 = a_DecalsMesh.Triangles[i + 1];
                int num3 = a_DecalsMesh.Triangles[i + 2];
                OptimizeEdge a_Edge = new(num, num2, i);
                OptimizeEdge a_Edge2 = new(num2, num3, i);
                OptimizeEdge a_Edge3 = new(num3, num, i);
                AddEdge(a_Edge);
                AddEdge(a_Edge2);
                AddEdge(a_Edge3);
            }

            List<OptimizeEdge> list = m_RedundantExternalEdges.OptimizedEdgeList();

            foreach (OptimizeEdge item in list) {
                if (!m_NonInternalVertexIndices.ContainsKey(item.vertex1Index)) {
                    m_NonInternalVertexIndices.Add(item.vertex1Index, item.vertex1Index);
                }

                if (!m_NonInternalVertexIndices.ContainsKey(item.vertex2Index)) {
                    m_NonInternalVertexIndices.Add(item.vertex2Index, item.vertex2Index);
                }
            }

            foreach (OptimizeEdge item2 in m_OverusedInternalEdges.OptimizedEdgeList()) {
                if (!m_NonInternalVertexIndices.ContainsKey(item2.vertex1Index)) {
                    m_NonInternalVertexIndices.Add(item2.vertex1Index, item2.vertex1Index);
                }

                if (!m_NonInternalVertexIndices.ContainsKey(item2.vertex2Index)) {
                    m_NonInternalVertexIndices.Add(item2.vertex2Index, item2.vertex2Index);
                }
            }

            for (int j = activeDecalProjector.DecalsMeshLowerVertexIndex;
                 j <= activeDecalProjector.DecalsMeshUpperVertexIndex;
                 j++) {
                if (!m_NonInternalVertexIndices.ContainsKey(j)) {
                    m_ObsoleteInternalVertexIndices.Add(j);
                }
            }

            for (int k = 0; k < list.Count; k++) {
                OptimizeEdge a_Edge4 = list[k];

                for (int l = k + 1; l < m_RedundantExternalEdges.Count; l++) {
                    OptimizeEdge a_Edge5 = list[l];

                    if (a_Edge4.vertex1Index == a_Edge5.vertex1Index || a_Edge4.vertex1Index == a_Edge5.vertex2Index) {
                        if (!m_ObsoleteExternalVertexIndices.Contains(a_Edge4.vertex1Index) &&
                            AreEdgesParallelOrIsAtLeastOneAPoint(a_DecalsMesh, a_Edge4, a_Edge5)) {
                            m_ObsoleteExternalVertexIndices.Add(a_Edge4.vertex1Index);
                        }
                    } else if
                        ((a_Edge4.vertex2Index == a_Edge5.vertex1Index || a_Edge4.vertex2Index == a_Edge5.vertex2Index) &&
                         !m_ObsoleteExternalVertexIndices.Contains(a_Edge4.vertex2Index) &&
                         AreEdgesParallelOrIsAtLeastOneAPoint(a_DecalsMesh, a_Edge4, a_Edge5)) {
                        m_ObsoleteExternalVertexIndices.Add(a_Edge4.vertex2Index);
                    }
                }
            }

            ClearTemporaryCollections();
        }

        void AddEdge(OptimizeEdge a_Edge) {
            if (m_OverusedInternalEdges.HasEdge(a_Edge)) {
                return;
            }

            if (!m_RedundantInternalEdges.HasEdge(a_Edge)) {
                if (!m_RedundantExternalEdges.HasEdge(a_Edge)) {
                    m_RedundantExternalEdges.AddEdge(a_Edge);
                    return;
                }

                OptimizeEdge a_OptimizeEdge = m_RedundantExternalEdges[a_Edge];
                m_RedundantExternalEdges.RemoveEdge(a_Edge);
                a_OptimizeEdge.triangle2Index = a_Edge.triangle1Index;
                m_RedundantInternalEdges.AddEdge(a_OptimizeEdge);
            } else {
                m_RedundantInternalEdges.RemoveEdge(a_Edge);
                m_OverusedInternalEdges.AddEdge(a_Edge);
            }
        }

        void ComputeObsoleteInternalVertices(DecalsMesh a_DecalsMesh) {
            List<OptimizeEdge> a_RedundantInternalEdges = m_RedundantInternalEdges.OptimizedEdgeList();

            for (int num = m_ObsoleteInternalVertexIndices.Count - 1; num >= 0; num--) {
                int a_VertexIndex = m_ObsoleteInternalVertexIndices[num];

                if (!m_ObsoleteVertexFinder.IsInternalVertexObsolete(a_DecalsMesh,
                        a_VertexIndex,
                        a_RedundantInternalEdges)) {
                    m_ObsoleteInternalVertexIndices.RemoveAt(num);
                }
            }
        }

        void ComputeObsoleteExternalVertices(DecalsMesh a_DecalsMesh) {
            for (int num = m_ObsoleteExternalVertexIndices.Count - 1; num >= 0; num--) {
                int a_VertexIndex = m_ObsoleteExternalVertexIndices[num];

                if (!m_ObsoleteVertexFinder.IsExternalVertexObsolete(a_DecalsMesh, a_VertexIndex)) {
                    m_ObsoleteExternalVertexIndices.RemoveAt(num);
                }
            }
        }

        void RemoveObsoleteVertices(DecalsMesh a_DecalsMesh) {
            DecalProjectorBase activeDecalProjector = a_DecalsMesh.ActiveDecalProjector;
            m_ObsoleteInternalVertexIndices.Sort();
            List<OptimizeEdge> a_RedundantInternalEdges = m_RedundantInternalEdges.OptimizedEdgeList();

            for (int num = m_ObsoleteInternalVertexIndices.Count - 1; num >= 0; num--) {
                int a_VertexIndex = m_ObsoleteInternalVertexIndices[num];
                bool a_SuccessfulRemoval;

                m_ObsoleteVertexRemover.RemoveObsoleteInternalVertex(a_DecalsMesh,
                    a_VertexIndex,
                    a_RedundantInternalEdges,
                    out a_SuccessfulRemoval);

                if (!a_SuccessfulRemoval) {
                    m_ObsoleteInternalVertexIndices.RemoveAt(num);
                }
            }

            for (int num2 = m_ObsoleteExternalVertexIndices.Count - 1; num2 >= 0; num2--) {
                int a_VertexIndex2 = m_ObsoleteExternalVertexIndices[num2];
                bool a_SuccessfulRemoval2;
                m_ObsoleteVertexRemover.RemoveObsoleteExternalVertex(a_DecalsMesh, a_VertexIndex2, out a_SuccessfulRemoval2);

                if (!a_SuccessfulRemoval2) {
                    m_ObsoleteExternalVertexIndices.RemoveAt(num2);
                }
            }

            m_ObsoleteInternalVertexIndices.AddRange(m_ObsoleteExternalVertexIndices);
            m_ObsoleteInternalVertexIndices.Sort();

            foreach (int obsoleteInternalVertexIndex in m_ObsoleteInternalVertexIndices) {
                m_RemovedIndices.AddRemovedIndex(obsoleteInternalVertexIndex);
            }

            a_DecalsMesh.RemoveAndAdjustIndices(activeDecalProjector.DecalsMeshLowerTriangleIndex, m_RemovedIndices);
            activeDecalProjector.IsUV1ProjectionCalculated = false;
            activeDecalProjector.IsUV2ProjectionCalculated = false;
            activeDecalProjector.IsTangentProjectionCalculated = false;
        }

        static bool AreEdgesParallelOrIsAtLeastOneAPoint(DecalsMesh a_DecalsMesh, OptimizeEdge a_Edge1,
            OptimizeEdge a_Edge2) {
            bool result = false;
            Vector3 vector = a_DecalsMesh.Vertices[a_Edge1.vertex1Index];
            Vector3 vector2 = a_DecalsMesh.Vertices[a_Edge1.vertex2Index];
            Vector3 vector3 = a_DecalsMesh.Vertices[a_Edge2.vertex1Index];
            Vector3 vector4 = a_DecalsMesh.Vertices[a_Edge2.vertex2Index];
            Vector3 vector5 = vector2 - vector;
            Vector3 vector6 = vector4 - vector3;
            vector5.Normalize();
            vector6.Normalize();

            if (Vector3Extension.Approximately(vector5,
                    Vector3.zero,
                    s_CurrentMaximumAbsoluteError,
                    s_CurrentMaximumRelativeError) ||
                Vector3Extension.Approximately(vector6,
                    Vector3.zero,
                    s_CurrentMaximumAbsoluteError,
                    s_CurrentMaximumRelativeError)) {
                result = true;
            } else {
                float f = Vector3.Dot(vector5, vector6);
                f = Mathf.Abs(f);

                if (MathfExtension.Approximately(f, 1f, s_CurrentMaximumAbsoluteError, s_CurrentMaximumRelativeError)) {
                    result = true;
                }
            }

            return result;
        }

        void ClearAll() {
            m_RedundantInternalEdges.Clear();
            m_ObsoleteInternalVertexIndices.Clear();
            m_ObsoleteExternalVertexIndices.Clear();
            m_RemovedIndices.Clear();
            ClearTemporaryCollections();
        }

        void ClearTemporaryCollections() {
            m_RedundantExternalEdges.Clear();
            m_OverusedInternalEdges.Clear();
            m_NonInternalVertexIndices.Clear();
        }
    }
}