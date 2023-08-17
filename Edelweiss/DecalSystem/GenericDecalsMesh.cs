using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    public abstract class GenericDecalsMesh<D, P, DM> : GenericDecalsMeshBase where D : GenericDecals<D, P, DM>
        where P : GenericDecalProjector<D, P, DM>
        where DM : GenericDecalsMesh<D, P, DM> {
        protected D m_Decals;

        bool m_PreserveProjectedUV2Arrays;

        bool m_PreserveProjectedUVArrays;

        bool m_PreserveVertexColorArrays;

        public D Decals => m_Decals;

        internal List<P> Projectors { get; } = new();

        public P ActiveDecalProjector {
            get {
                P result = null;

                if (Projectors.Count != 0) {
                    result = Projectors[Projectors.Count - 1];
                }

                return result;
            }
        }

        internal List<Color[]> PreservedVertexColorArrays { get; } = new();

        public bool PreserveVertexColorArrays {
            get => m_PreserveVertexColorArrays;
            set {
                if (value) {
                    if (!(m_Decals != null)) {
                        throw new InvalidOperationException(
                            "Unable to set preserving value while no valid decals instance is set.");
                    }

                    if (Edition.IsDecalSystemFree) {
                        throw new InvalidOperationException(
                            "Preserving vertex color arrays is only supported in Decal System Pro.");
                    }

                    if (!m_Decals.UseVertexColors) {
                        throw new InvalidOperationException(
                            "Unable to preserve vertex color arrays for a decals instance that does not use them.");
                    }

                    m_PreserveVertexColorArrays = value;
                } else {
                    m_PreserveVertexColorArrays = value;
                    PreservedVertexColorArrays.Clear();
                }
            }
        }

        internal List<Vector2[]> PreservedProjectedUVArrays { get; } = new();

        public bool PreserveProjectedUVArrays {
            get => m_PreserveProjectedUVArrays;
            set {
                if (value) {
                    if (!(m_Decals != null)) {
                        throw new InvalidOperationException(
                            "Unable to set preserving value while no valid decals instance is set.");
                    }

                    if (Edition.IsDecalSystemFree) {
                        throw new InvalidOperationException("Preserving uv arrays is only supported in Decal System Pro.");
                    }

                    if (m_Decals.CurrentUVMode != 0 && m_Decals.CurrentUVMode != UVMode.ProjectWrapped) {
                        throw new InvalidOperationException(
                            "Unalble to preserve uv arrays for a decals instance that does not use them.");
                    }

                    m_PreserveProjectedUVArrays = value;
                } else {
                    m_PreserveProjectedUVArrays = value;
                    PreservedProjectedUVArrays.Clear();
                }
            }
        }

        internal List<Vector2[]> PreservedProjectedUV2Arrays { get; } = new();

        public bool PreserveProjectedUV2Arrays {
            get => m_PreserveProjectedUV2Arrays;
            set {
                if (value) {
                    if (!(m_Decals != null)) {
                        throw new InvalidOperationException(
                            "Unable to set preserving value while no valid decals instance is set.");
                    }

                    if (Edition.IsDecalSystemFree) {
                        throw new InvalidOperationException("Preserving uv arrays is only supported in Decal System Pro.");
                    }

                    if (m_Decals.CurrentUV2Mode != UV2Mode.Project && m_Decals.CurrentUV2Mode != UV2Mode.ProjectWrapped) {
                        throw new InvalidOperationException(
                            "Unable to preserve uv arrays for a decals instance that does not use them.");
                    }

                    m_PreserveProjectedUV2Arrays = value;
                } else {
                    m_PreserveProjectedUV2Arrays = value;
                    PreservedProjectedUV2Arrays.Clear();
                }
            }
        }

        public void Initialize(D a_Decals) {
            m_Decals = a_Decals;
            PreserveVertexColorArrays = false;
            PreserveProjectedUVArrays = false;
            PreserveProjectedUV2Arrays = false;
            ClearAll();
        }

        protected override void RecalculateUVs() {
            if (!(m_Decals != null) || m_Decals.CurrentUVMode != 0 && m_Decals.CurrentUVMode != UVMode.ProjectWrapped) {
                return;
            }

            while (m_UVs.Count > m_Vertices.Count) {
                m_UVs.RemoveAt(m_UVs.Count - 1);
            }

            foreach (P projector in Projectors) {
                P current = projector;

                if (!current.IsUV1ProjectionCalculated) {
                    CalculateProjectedUV(current);
                }
            }
        }

        protected override bool HasUV2LightmappingMode() {
            bool result = false;

            if (m_Decals != null && m_Decals.CurrentUV2Mode == UV2Mode.Lightmapping) {
                result = true;
            }

            return result;
        }

        protected override void RecalculateUV2s() {
            if (!(m_Decals != null) ||
                m_Decals.CurrentUV2Mode != UV2Mode.Project && m_Decals.CurrentUV2Mode != UV2Mode.ProjectWrapped) {
                return;
            }

            while (m_UV2s.Count > m_Vertices.Count) {
                m_UV2s.RemoveAt(m_UV2s.Count - 1);
            }

            foreach (P projector in Projectors) {
                if (!projector.IsUV2ProjectionCalculated) {
                    CalculateProjectedUV2(projector);
                }
            }
        }

        protected override void RecalculateTangents() {
            if (!(m_Decals != null) || m_Decals.CurrentTangentsMode != TangentsMode.Project) {
                return;
            }

            while (m_Tangents.Count > m_Vertices.Count) {
                m_Tangents.RemoveAt(m_Tangents.Count - 1);
            }

            foreach (P projector in Projectors) {
                if (!projector.IsTangentProjectionCalculated) {
                    CalculateProjectedTangents(projector);
                }
            }
        }

        public void UpdateVertexColors(P a_Projector) {
            if (m_Decals == null) {
                throw new InvalidOperationException("Decals mesh is not initialized with a decals instance.");
            }

            if (!Projectors.Contains(a_Projector)) {
                throw new ArgumentException("Projector is not part of the decals mesh.");
            }

            Color vertexColorTint = m_Decals.VertexColorTint;
            Color color = (1f - a_Projector.VertexColorBlending) * a_Projector.VertexColor;

            for (int i = a_Projector.DecalsMeshLowerVertexIndex; i <= a_Projector.DecalsMeshUpperVertexIndex; i++) {
                m_VertexColors[i] = vertexColorTint * (color + a_Projector.VertexColorBlending * m_TargetVertexColors[i]);
            }
        }

        public void CalculateProjectedUV1ForActiveProjector() => CalculateProjectedUV(ActiveDecalProjector);

        public void CalculateProjectedUV2ForActiveProjector() => CalculateProjectedUV2(ActiveDecalProjector);

        public void UpdateProjectedUV(P a_Projector) {
            if (m_Decals == null) {
                throw new InvalidOperationException("Decals mesh is not initialized with a decals instance.");
            }

            if (m_Decals.CurrentUvRectangles == null) {
                throw new InvalidOperationException("Empty uv rectangles.");
            }

            if (m_Decals.CurrentUVMode != 0 && m_Decals.CurrentUVMode != UVMode.ProjectWrapped) {
                throw new InvalidOperationException("UV mode of the decals is not projected!");
            }

            if (!Projectors.Contains(a_Projector)) {
                throw new ArgumentException("Projector is not part of the decals mesh.");
            }

            CalculateProjectedUV(a_Projector);
        }

        public void UpdateProjectedUV2(P a_Projector) {
            if (m_Decals == null) {
                throw new InvalidOperationException("Decals mesh is not initialized with a decals instance.");
            }

            if (m_Decals.CurrentUvRectangles == null) {
                throw new InvalidOperationException("Empty uv rectangles.");
            }

            if (m_Decals.CurrentUV2Mode != UV2Mode.Project && m_Decals.CurrentUV2Mode != UV2Mode.ProjectWrapped) {
                throw new InvalidOperationException("UV2 mode of the decals is not projected!");
            }

            if (!Projectors.Contains(a_Projector)) {
                throw new ArgumentException("Projector is not part of the decals mesh.");
            }

            CalculateProjectedUV2(a_Projector);
        }

        void CalculateProjectedUV(GenericDecalProjectorBase a_Projector) {
            Matrix4x4 a_DecalsToProjectorMatrix =
                a_Projector.WorldToProjectorMatrix * m_Decals.CachedTransform.localToWorldMatrix;

            UVRectangle a_UVRectangle = m_Decals.CurrentUvRectangles[a_Projector.UV1RectangleIndex];
            List<Vector2> uVs = m_UVs;
            int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;

            if (m_Decals.CurrentUVMode == UVMode.Project) {
                CalculateProjectedUV(a_DecalsToProjectorMatrix,
                    a_UVRectangle,
                    uVs,
                    decalsMeshLowerVertexIndex,
                    decalsMeshUpperVertexIndex);
            } else {
                CalculateWrappedProjectionUV(a_DecalsToProjectorMatrix,
                    a_UVRectangle,
                    uVs,
                    decalsMeshLowerVertexIndex,
                    decalsMeshUpperVertexIndex);
            }

            a_Projector.IsUV1ProjectionCalculated = true;
        }

        void CalculateProjectedUV2(GenericDecalProjectorBase a_Projector) {
            Matrix4x4 a_DecalsToProjectorMatrix =
                a_Projector.WorldToProjectorMatrix * m_Decals.CachedTransform.localToWorldMatrix;

            UVRectangle a_UVRectangle = m_Decals.CurrentUv2Rectangles[a_Projector.UV2RectangleIndex];
            List<Vector2> uV2s = m_UV2s;
            int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;

            if (m_Decals.CurrentUVMode == UVMode.Project) {
                CalculateProjectedUV(a_DecalsToProjectorMatrix,
                    a_UVRectangle,
                    uV2s,
                    decalsMeshLowerVertexIndex,
                    decalsMeshUpperVertexIndex);
            } else {
                CalculateWrappedProjectionUV(a_DecalsToProjectorMatrix,
                    a_UVRectangle,
                    uV2s,
                    decalsMeshLowerVertexIndex,
                    decalsMeshUpperVertexIndex);
            }

            a_Projector.IsUV2ProjectionCalculated = true;
        }

        void CalculateProjectedUV(Matrix4x4 a_DecalsToProjectorMatrix, UVRectangle a_UVRectangle, List<Vector2> a_UVs,
            int a_LowerIndex, int a_UpperIndex) {
            Vector2 lowerLeftUV = a_UVRectangle.lowerLeftUV;
            Vector2 size = a_UVRectangle.Size;

            while (a_UVs.Count < a_LowerIndex) {
                a_UVs.Add(Vector2.zero);
            }

            for (int i = a_LowerIndex; i <= a_UpperIndex; i++) {
                Vector3 v = Vertices[i];
                v = a_DecalsToProjectorMatrix.MultiplyPoint3x4(v);
                Vector2 vector = new(v.x, v.z);
                vector.x = lowerLeftUV.x + (vector.x + 0.5f) * size.x;
                vector.y = lowerLeftUV.y + (vector.y + 0.5f) * size.y;

                if (i < a_UVs.Count) {
                    a_UVs[i] = vector;
                } else {
                    a_UVs.Add(vector);
                }
            }
        }

        void CalculateWrappedProjectionUV(Matrix4x4 a_DecalsToProjectorMatrix, UVRectangle a_UVRectangle,
            List<Vector2> a_UVs, int a_LowerIndex, int a_UpperIndex) {
            Vector2 lowerLeftUV = a_UVRectangle.lowerLeftUV;
            Vector2 size = a_UVRectangle.Size;

            while (a_UVs.Count < a_LowerIndex) {
                a_UVs.Add(Vector2.zero);
            }

            for (int i = a_LowerIndex; i <= a_UpperIndex; i++) {
                Vector3 vector = a_DecalsToProjectorMatrix.MultiplyPoint3x4(Vertices[i]);
                Vector3 vector2 = a_DecalsToProjectorMatrix.MultiplyVector(Normals[i]);
                Vector2 vector3 = new(vector.x, vector.z);
                vector3 -= vector.y * new Vector2(vector2.x, vector2.z);
                vector3.x = Mathf.Clamp(vector3.x, -0.5f, 0.5f);
                vector3.y = Mathf.Clamp(vector3.y, -0.5f, 0.5f);
                vector3.x = lowerLeftUV.x + (vector3.x + 0.5f) * size.x;
                vector3.y = lowerLeftUV.y + (vector3.y + 0.5f) * size.y;

                if (i < a_UVs.Count) {
                    a_UVs[i] = vector3;
                } else {
                    a_UVs.Add(vector3);
                }
            }
        }

        void CalculateProjectedTangents(GenericDecalProjectorBase a_Projector) {
            while (m_Tangents.Count < a_Projector.DecalsMeshLowerVertexIndex) {
                m_Tangents.Add(Vector4.zero);
            }

            Matrix4x4 transpose = (a_Projector.WorldToProjectorMatrix * m_Decals.CachedTransform.localToWorldMatrix).inverse
                .transpose;

            Matrix4x4 transpose2 = (m_Decals.CachedTransform.worldToLocalMatrix * a_Projector.ProjectorToWorldMatrix).inverse
                .transpose;

            for (int i = a_Projector.DecalsMeshLowerVertexIndex; i <= a_Projector.DecalsMeshUpperVertexIndex; i++) {
                Vector3 v = Normals[i];
                v = transpose.MultiplyVector(v);
                v.z = 0f;

                if (Mathf.Approximately(v.x, 0f) && Mathf.Approximately(v.y, 0f)) {
                    v = new Vector3(0f, 1f, 0f);
                }

                v = new Vector3(v.y, 0f - v.x, v.z);
                v = transpose2.MultiplyVector(v);
                v.Normalize();
                Vector4 vector = new(v.x, v.y, v.z, -1f);

                if (i < m_Tangents.Count) {
                    m_Tangents[i] = vector;
                } else {
                    m_Tangents.Add(vector);
                }
            }
        }

        public virtual void ClearAll() {
            foreach (P projector in Projectors) {
                P current = projector;
                current.ResetDecalsMesh();
            }

            Projectors.Clear();
            Vertices.Clear();
            Normals.Clear();
            Tangents.Clear();
            TargetVertexColors.Clear();
            VertexColors.Clear();
            UVs.Clear();
            UV2s.Clear();
            Triangles.Clear();

            if (m_Decals != null) {
                m_Decals.LinkedDecalsMesh = null;
            }
        }

        public bool ContainsProjector(P a_Projector) => Projectors.Contains(a_Projector);

        public void AddProjector(P a_Projector) {
            if (a_Projector == null) {
                throw new ArgumentNullException("Projector parameter is not allowed to be null!");
            }

            if (a_Projector.DecalsMesh != null) {
                throw new InvalidOperationException("Projector is already used in this or another instance!");
            }

            if (m_Decals == null) {
                throw new NullReferenceException("Projectors can only be added if the decals is not null!");
            }

            if (m_Decals.LinkedDecalsMesh != null && m_Decals.LinkedDecalsMesh != this) {
                throw new InvalidOperationException("The decals instance is already linked to another decals mesh.");
            }

            P activeDecalProjector = ActiveDecalProjector;

            if (activeDecalProjector != null) {
                activeDecalProjector.IsActiveProjector = false;
            }

            Projectors.Add(a_Projector);
            a_Projector.DecalsMesh = this as DM;
            a_Projector.IsActiveProjector = true;
            SetRangesForAddedProjector(a_Projector);
            m_Decals.LinkedDecalsMesh = this;
        }

        internal virtual void SetRangesForAddedProjector(P a_Projector) {
            a_Projector.DecalsMeshLowerVertexIndex = m_Vertices.Count;
            a_Projector.DecalsMeshUpperVertexIndex = m_Vertices.Count - 1;
            a_Projector.DecalsMeshLowerTriangleIndex = m_Triangles.Count;
            a_Projector.DecalsMeshUpperTriangleIndex = m_Triangles.Count - 1;
        }

        public void RemoveProjector(P a_Projector) {
            if (a_Projector.DecalsMesh != this) {
                throw new InvalidOperationException("Unable to remove a projector that is not part of this instance.");
            }

            int decalsMeshLowerVertexIndex = a_Projector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_Projector.DecalsMeshUpperVertexIndex;
            int decalsMeshVertexCount = a_Projector.DecalsMeshVertexCount;
            int decalsMeshLowerTriangleIndex = a_Projector.DecalsMeshLowerTriangleIndex;
            int decalsMeshTriangleCount = a_Projector.DecalsMeshTriangleCount;

            if (decalsMeshTriangleCount > 0) {
                m_Triangles.RemoveRange(decalsMeshLowerTriangleIndex, decalsMeshTriangleCount);
            }

            for (int i = decalsMeshLowerTriangleIndex; i < m_Triangles.Count; i++) {
                int num = m_Triangles[i];

                if (num > decalsMeshUpperVertexIndex) {
                    m_Triangles[i] = num - decalsMeshVertexCount;
                }
            }

            RemoveRangesOfVertexAlignedLists(decalsMeshLowerVertexIndex, decalsMeshVertexCount);
            int a_BoneIndexOffset = BoneIndexOffset(a_Projector);
            RemoveBonesAndAdjustBoneWeightIndices(a_Projector);

            if (a_Projector.IsActiveProjector) {
                Projectors.RemoveAt(Projectors.Count - 1);
            } else {
                int num2 = Projectors.IndexOf(a_Projector);

                for (int j = num2 + 1; j < Projectors.Count; j++) {
                    P a_Projector2 = Projectors[j];
                    AdjustProjectorIndices(a_Projector2, decalsMeshVertexCount, decalsMeshTriangleCount, a_BoneIndexOffset);
                }

                Projectors.RemoveAt(num2);
            }

            a_Projector.ResetDecalsMesh();

            if (Projectors.Count == 0) {
                m_Decals.LinkedDecalsMesh = null;
            }
        }

        internal virtual void RemoveRangesOfVertexAlignedLists(int a_LowerIndex, int a_Count) {
            m_Vertices.RemoveRange(a_LowerIndex, a_Count);

            if (a_LowerIndex < m_Normals.Count) {
                m_Normals.RemoveRange(a_LowerIndex, a_Count);
            }

            if (a_LowerIndex < m_UVs.Count) {
                m_UVs.RemoveRange(a_LowerIndex, a_Count);
            }

            if (a_LowerIndex < m_UV2s.Count) {
                m_UV2s.RemoveRange(a_LowerIndex, a_Count);
            }

            if (a_LowerIndex < m_Tangents.Count) {
                m_Tangents.RemoveRange(a_LowerIndex, a_Count);
            }

            if (a_LowerIndex < m_TargetVertexColors.Count) {
                m_TargetVertexColors.RemoveRange(a_LowerIndex, a_Count);
            }

            if (a_LowerIndex < m_VertexColors.Count) {
                m_VertexColors.RemoveRange(a_LowerIndex, a_Count);
            }
        }

        internal virtual int BoneIndexOffset(P a_Projector) => 0;

        internal virtual void RemoveBonesAndAdjustBoneWeightIndices(P a_Projector) { }

        internal virtual void AdjustProjectorIndices(P a_Projector, int a_VertexIndexOffset, int a_TriangleIndexOffset,
            int a_BoneIndexOffset) {
            a_Projector.DecalsMeshLowerVertexIndex -= a_VertexIndexOffset;
            a_Projector.DecalsMeshUpperVertexIndex -= a_VertexIndexOffset;
            a_Projector.DecalsMeshLowerTriangleIndex -= a_TriangleIndexOffset;
            a_Projector.DecalsMeshUpperTriangleIndex -= a_TriangleIndexOffset;
        }

        public abstract void OffsetActiveProjectorVertices();

        public void SmoothActiveProjectorNormals(float a_SmoothFactor) {
            P activeDecalProjector = ActiveDecalProjector;

            if (activeDecalProjector == null) {
                throw new InvalidOperationException("There is no active projector.");
            }

            if (a_SmoothFactor < 0f || a_SmoothFactor > 1f) {
                throw new ArgumentOutOfRangeException("The smooth factor has to be in [0.0f, 1.0f].");
            }

            int decalsMeshLowerVertexIndex = activeDecalProjector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = activeDecalProjector.DecalsMeshUpperVertexIndex;
            Vector3 b = activeDecalProjector.Rotation * Vector3.up;

            for (int i = decalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++) {
                Vector3 a = m_Normals[i];
                a = Vector3.Lerp(a, b, a_SmoothFactor);
                a.Normalize();
                m_Normals[i] = a;
            }
        }

        internal void RemoveTriangleAt(int a_Index) => m_Triangles.RemoveRange(a_Index, 3);

        internal void RemoveTrianglesAt(int a_Index, int a_Count) => m_Triangles.RemoveRange(a_Index, 3 * a_Count);

        internal void RemoveAndAdjustIndices(int a_LowerTriangleIndex, RemovedIndices a_RemovedIndices) {
            AdjustTriangleIndices(a_LowerTriangleIndex, a_RemovedIndices);
            RemoveIndices(a_RemovedIndices);
        }

        void AdjustTriangleIndices(int a_LowerTriangleIndex, RemovedIndices a_RemovedIndices) {
            for (int i = a_LowerTriangleIndex; i < m_Triangles.Count; i++) {
                m_Triangles[i] = a_RemovedIndices.AdjustedIndex(m_Triangles[i]);
            }

            P activeDecalProjector = ActiveDecalProjector;
            activeDecalProjector.DecalsMeshUpperTriangleIndex = m_Triangles.Count - 1;
        }

        internal void RemoveIndices(RemovedIndices a_RemovedIndices) {
            int num = -1;
            int num2 = 0;

            for (int num3 = m_Vertices.Count - 1; num3 >= 0; num3--) {
                bool flag = a_RemovedIndices.IsRemovedIndex(num3);

                if (flag) {
                    if (num == -1) {
                        num = num3;
                        num2 = 1;
                    } else {
                        num = num3;
                        num2++;
                    }
                }

                if ((!flag || num3 == 0) && num != -1) {
                    RemoveRange(num, num2);
                    num = -1;
                    num2 = 0;
                }
            }

            P activeDecalProjector = ActiveDecalProjector;
            activeDecalProjector.DecalsMeshUpperVertexIndex = m_Vertices.Count - 1;
        }

        internal abstract void RemoveRange(int a_StartIndex, int a_Count);
    }
}