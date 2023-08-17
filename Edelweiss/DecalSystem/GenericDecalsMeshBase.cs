using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    public abstract class GenericDecalsMeshBase {
        protected List<Vector3> m_Normals = new();

        internal RemovedIndices m_RemovedIndices = new();

        protected List<Vector4> m_Tangents = new();

        protected List<Color> m_TargetVertexColors = new();

        protected List<int> m_Triangles = new();

        protected List<Vector2> m_UV2s = new();
        protected List<Vector2> m_UVs = new();

        protected List<Color> m_VertexColors = new();

        protected List<Vector3> m_Vertices = new();

        public List<Vector2> UVs {
            get {
                RecalculateUVs();
                return m_UVs;
            }
        }

        public List<Vector2> UV2s {
            get {
                if (Application.isPlaying && HasUV2LightmappingMode()) {
                    throw new InvalidOperationException(
                        "The lightmap for the UV2s can not be recalculated if the application is playing!");
                }

                RecalculateUV2s();
                return m_UV2s;
            }
        }

        public List<Vector3> Vertices => m_Vertices;

        public List<Vector3> Normals => m_Normals;

        public List<Vector4> Tangents {
            get {
                RecalculateTangents();
                return m_Tangents;
            }
        }

        public List<Color> TargetVertexColors => m_TargetVertexColors;

        public List<Color> VertexColors => m_VertexColors;

        public List<int> Triangles => m_Triangles;

        protected abstract void RecalculateUVs();

        protected abstract bool HasUV2LightmappingMode();

        protected abstract void RecalculateUV2s();

        protected abstract void RecalculateTangents();
    }
}