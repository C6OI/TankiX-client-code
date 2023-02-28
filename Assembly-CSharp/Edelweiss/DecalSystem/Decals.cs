using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    public abstract class Decals : GenericDecals<Decals, DecalProjectorBase, DecalsMesh> {
        [SerializeField] MeshMinimizerMode m_MeshMinimizerMode;

        [SerializeField] float m_MeshMinimizerMaximumAbsoluteError = 0.0001f;

        [SerializeField] float m_MeshMinimizerMaximumRelativeError = 0.0001f;

        readonly List<DecalsMeshRenderer> m_DecalsMeshRenderers = new();

        public MeshMinimizerMode CurrentMeshMinimizerMode {
            get => m_MeshMinimizerMode;
            set => m_MeshMinimizerMode = value;
        }

        public float MeshMinimizerMaximumAbsoluteError {
            get => m_MeshMinimizerMaximumAbsoluteError;
            set {
                if (value < 0f) {
                    throw new ArgumentOutOfRangeException("The maximum absolute error has to be greater than zero.");
                }

                m_MeshMinimizerMaximumAbsoluteError = value;
            }
        }

        public float MeshMinimizerMaximumRelativeError {
            get => m_MeshMinimizerMaximumRelativeError;
            set {
                if (value < 0f || value > 1f) {
                    throw new ArgumentOutOfRangeException("The maximum relative error has to be within [0.0f, 1.0f].");
                }

                m_MeshMinimizerMaximumRelativeError = value;
            }
        }

        public override bool CastShadows {
            get => DecalsMeshRenderers[0].MeshRenderer.castShadows;
            set {
                DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;

                foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers) {
                    decalsMeshRenderer.MeshRenderer.castShadows = value;
                }
            }
        }

        public override bool ReceiveShadows {
            get => DecalsMeshRenderers[0].MeshRenderer.receiveShadows;
            set {
                DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;

                foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers) {
                    decalsMeshRenderer.MeshRenderer.receiveShadows = value;
                }
            }
        }

        public override bool UseLightProbes {
            get => DecalsMeshRenderers[0].MeshRenderer.useLightProbes;
            set {
                DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;

                foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers) {
                    decalsMeshRenderer.MeshRenderer.useLightProbes = value;
                }
            }
        }

        public override Transform LightProbeAnchor {
            get => DecalsMeshRenderers[0].MeshRenderer.probeAnchor;
            set {
                DecalsMeshRenderer[] decalsMeshRenderers = DecalsMeshRenderers;

                foreach (DecalsMeshRenderer decalsMeshRenderer in decalsMeshRenderers) {
                    decalsMeshRenderer.MeshRenderer.probeAnchor = value;
                }
            }
        }

        public DecalsMeshRenderer[] DecalsMeshRenderers => m_DecalsMeshRenderers.ToArray();

        public override void OnEnable() {
            InitializeDecalsMeshRenderers();

            if (m_DecalsMeshRenderers.Count == 0) {
                PushDecalsMeshRenderer();
            }
        }

        public bool IsDecalsMeshRenderer(MeshRenderer a_MeshRenderer) {
            bool result = false;

            foreach (DecalsMeshRenderer decalsMeshRenderer in m_DecalsMeshRenderers) {
                if (a_MeshRenderer == decalsMeshRenderer.MeshRenderer) {
                    return true;
                }
            }

            return result;
        }

        public override void ApplyMaterialToMeshRenderers() {
            base.ApplyMaterialToMeshRenderers();
            Material material = null;

            if (CurrentTextureAtlasType == TextureAtlasType.Builtin) {
                material = CurrentMaterial;
            } else if (CurrentTextureAtlasType == TextureAtlasType.Reference && CurrentTextureAtlasAsset != null) {
                material = CurrentTextureAtlasAsset.material;
            }

            foreach (DecalsMeshRenderer decalsMeshRenderer in m_DecalsMeshRenderers) {
                decalsMeshRenderer.MeshRenderer.material = material;
            }
        }

        public override void ApplyRenderersEditability() {
            base.ApplyRenderersEditability();
            HideFlags hideFlags = HideFlags.None;

            if (!AreRenderersEditable) {
                hideFlags = HideFlags.NotEditable;
            }

            foreach (DecalsMeshRenderer decalsMeshRenderer in m_DecalsMeshRenderers) {
                decalsMeshRenderer.gameObject.hideFlags = hideFlags;
            }
        }

        public override void InitializeDecalsMeshRenderers() {
            m_DecalsMeshRenderers.Clear();
            Transform cachedTransform = CachedTransform;

            for (int i = 0; i < cachedTransform.childCount; i++) {
                Transform child = cachedTransform.GetChild(i);
                DecalsMeshRenderer component = child.GetComponent<DecalsMeshRenderer>();

                if (component != null) {
                    m_DecalsMeshRenderers.Add(component);
                }
            }
        }

        public override void UpdateDecalsMeshes(DecalsMesh a_DecalsMesh) {
            base.UpdateDecalsMeshes(a_DecalsMesh);

            if (a_DecalsMesh.Vertices.Count <= 65535) {
                if (m_DecalsMeshRenderers.Count == 0) {
                    PushDecalsMeshRenderer();
                } else if (m_DecalsMeshRenderers.Count > 1) {
                    while (m_DecalsMeshRenderers.Count > 1) {
                        PopDecalsMeshRenderer();
                    }
                }

                DecalsMeshRenderer a_DecalsMeshRenderer = m_DecalsMeshRenderers[0];
                ApplyToDecalsMeshRenderer(a_DecalsMeshRenderer, a_DecalsMesh);
            } else {
                int num = 0;

                for (int i = 0; i < a_DecalsMesh.Projectors.Count; i++) {
                    GenericDecalProjectorBase a_FirstProjector = a_DecalsMesh.Projectors[i];
                    GenericDecalProjectorBase a_LastProjector = a_DecalsMesh.Projectors[i];

                    if (num >= m_DecalsMeshRenderers.Count) {
                        PushDecalsMeshRenderer();
                    }

                    DecalsMeshRenderer a_DecalsMeshRenderer2 = m_DecalsMeshRenderers[num];
                    int num2 = 0;
                    int num3 = i;
                    GenericDecalProjectorBase genericDecalProjectorBase = a_DecalsMesh.Projectors[i];

                    while (i < a_DecalsMesh.Projectors.Count && num2 + genericDecalProjectorBase.DecalsMeshVertexCount <= 65535) {
                        a_LastProjector = genericDecalProjectorBase;
                        num2 += genericDecalProjectorBase.DecalsMeshVertexCount;
                        i++;

                        if (i < a_DecalsMesh.Projectors.Count) {
                            genericDecalProjectorBase = a_DecalsMesh.Projectors[i];
                        }
                    }

                    if (num3 != i) {
                        ApplyToDecalsMeshRenderer(a_DecalsMeshRenderer2, a_DecalsMesh, a_FirstProjector, a_LastProjector);
                        num++;
                    }
                }

                while (num + 1 < m_DecalsMeshRenderers.Count) {
                    PopDecalsMeshRenderer();
                }
            }

            SetDecalsMeshesAreNotOptimized();
        }

        public override void UpdateVertexColors(DecalsMesh a_DecalsMesh) {
            base.UpdateVertexColors(a_DecalsMesh);
            int a_LowerListIndex = 0;
            int num = 0;

            for (int i = 0; i < m_DecalsMeshRenderers.Count; i++) {
                DecalsMeshRenderer decalsMeshRenderer = m_DecalsMeshRenderers[i];
                Mesh mesh = !Application.isPlaying ? decalsMeshRenderer.MeshFilter.sharedMesh : decalsMeshRenderer.MeshFilter.mesh;
                num = num + mesh.vertexCount - 1;
                Color[] a_Array = a_DecalsMesh.PreservedVertexColorArrays[i];
                CopyListRangeToArray(ref a_Array, a_DecalsMesh.VertexColors, a_LowerListIndex, num);
                mesh.colors = a_Array;
                a_LowerListIndex = num;
            }
        }

        public override void UpdateProjectedUVs(DecalsMesh a_DecalsMesh) {
            base.UpdateProjectedUVs(a_DecalsMesh);
            int a_LowerListIndex = 0;
            int num = 0;

            for (int i = 0; i < m_DecalsMeshRenderers.Count; i++) {
                DecalsMeshRenderer decalsMeshRenderer = m_DecalsMeshRenderers[i];
                Mesh mesh = !Application.isPlaying ? decalsMeshRenderer.MeshFilter.sharedMesh : decalsMeshRenderer.MeshFilter.mesh;
                num = num + mesh.vertexCount - 1;
                Vector2[] a_Array = a_DecalsMesh.PreservedProjectedUVArrays[i];
                CopyListRangeToArray(ref a_Array, a_DecalsMesh.UVs, a_LowerListIndex, num);
                mesh.uv = a_Array;
                a_LowerListIndex = num;
            }
        }

        public override void UpdateProjectedUV2s(DecalsMesh a_DecalsMesh) {
            base.UpdateProjectedUV2s(a_DecalsMesh);
            int a_LowerListIndex = 0;
            int num = 0;

            for (int i = 0; i < m_DecalsMeshRenderers.Count; i++) {
                DecalsMeshRenderer decalsMeshRenderer = m_DecalsMeshRenderers[i];
                Mesh mesh = !Application.isPlaying ? decalsMeshRenderer.MeshFilter.sharedMesh : decalsMeshRenderer.MeshFilter.mesh;
                num = num + mesh.vertexCount - 1;
                Vector2[] a_Array = a_DecalsMesh.PreservedProjectedUV2Arrays[i];
                CopyListRangeToArray(ref a_Array, a_DecalsMesh.UV2s, a_LowerListIndex, num);
                mesh.uv2 = a_Array;
                a_LowerListIndex = num;
            }
        }

        void PushDecalsMeshRenderer() {
            GameObject gameObject = new("Decals Mesh Renderer");
            Transform transform = gameObject.transform;
            transform.parent = this.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            DecalsMeshRenderer decalsMeshRenderer = AddDecalsMeshRendererComponentToGameObject(gameObject);
            decalsMeshRenderer.MeshRenderer.material = CurrentMaterial;
            m_DecalsMeshRenderers.Add(decalsMeshRenderer);
        }

        void PopDecalsMeshRenderer() {
            DecalsMeshRenderer decalsMeshRenderer = m_DecalsMeshRenderers[m_DecalsMeshRenderers.Count - 1];

            if (Application.isPlaying) {
                Destroy(decalsMeshRenderer.MeshFilter.mesh);
                Destroy(decalsMeshRenderer.gameObject);
            }

            m_DecalsMeshRenderers.RemoveAt(m_DecalsMeshRenderers.Count - 1);
        }

        void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh) {
            Mesh mesh = MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
            mesh.Clear();

            if (!Edition.IsDX11) {
                mesh.MarkDynamic();
            }

            mesh.vertices = a_DecalsMesh.Vertices.ToArray();

            if (CurrentNormalsMode != 0) {
                mesh.normals = a_DecalsMesh.Normals.ToArray();
            } else {
                mesh.normals = null;
            }

            if (CurrentTangentsMode != 0) {
                mesh.tangents = a_DecalsMesh.Tangents.ToArray();
            } else {
                mesh.tangents = null;
            }

            if (UseVertexColors) {
                Color[] array = a_DecalsMesh.VertexColors.ToArray();

                if (a_DecalsMesh.PreserveVertexColorArrays) {
                    a_DecalsMesh.PreservedVertexColorArrays.Add(array);
                }

                mesh.colors = array;
            } else {
                mesh.colors = null;
            }

            Vector2[] array2 = a_DecalsMesh.UVs.ToArray();

            if (a_DecalsMesh.PreserveProjectedUVArrays) {
                a_DecalsMesh.PreservedProjectedUVArrays.Add(array2);
            }

            mesh.uv = array2;

            if (CurrentUV2Mode != 0) {
                Vector2[] array3 = a_DecalsMesh.UV2s.ToArray();

                if (a_DecalsMesh.PreserveProjectedUV2Arrays) {
                    a_DecalsMesh.PreservedProjectedUV2Arrays.Add(array3);
                }

                mesh.uv2 = array3;
            } else {
                mesh.uv2 = null;
            }

            mesh.triangles = a_DecalsMesh.Triangles.ToArray();
        }

        void ApplyToDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer, DecalsMesh a_DecalsMesh, GenericDecalProjectorBase a_FirstProjector,
            GenericDecalProjectorBase a_LastProjector) {
            int decalsMeshLowerVertexIndex = a_FirstProjector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = a_LastProjector.DecalsMeshUpperVertexIndex;
            int decalsMeshLowerTriangleIndex = a_FirstProjector.DecalsMeshLowerTriangleIndex;
            int decalsMeshUpperTriangleIndex = a_LastProjector.DecalsMeshUpperTriangleIndex;
            Mesh mesh = MeshOfDecalsMeshRenderer(a_DecalsMeshRenderer);
            mesh.Clear();

            if (!Edition.IsDX11) {
                mesh.MarkDynamic();
            }

            Vector3[] a_Array = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
            CopyListRangeToArray(ref a_Array, a_DecalsMesh.Vertices, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
            mesh.vertices = a_Array;
            int[] a_Array2 = new int[decalsMeshUpperTriangleIndex - decalsMeshLowerTriangleIndex + 1];
            CopyListRangeToArray(ref a_Array2, a_DecalsMesh.Triangles, decalsMeshLowerTriangleIndex, decalsMeshUpperTriangleIndex);

            for (int i = 0; i < a_Array2.Length; i++) {
                a_Array2[i] -= decalsMeshLowerVertexIndex;
            }

            mesh.triangles = a_Array2;
            Vector2[] a_Array3 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
            CopyListRangeToArray(ref a_Array3, a_DecalsMesh.UVs, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);

            if (a_DecalsMesh.PreserveProjectedUVArrays) {
                a_DecalsMesh.PreservedProjectedUVArrays.Add(a_Array3);
            }

            mesh.uv = a_Array3;

            if (CurrentUV2Mode != 0 && CurrentUV2Mode != UV2Mode.Lightmapping) {
                Vector2[] a_Array4 = new Vector2[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
                CopyListRangeToArray(ref a_Array4, a_DecalsMesh.UV2s, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);

                if (a_DecalsMesh.PreserveProjectedUV2Arrays) {
                    a_DecalsMesh.PreservedProjectedUV2Arrays.Add(a_Array4);
                }

                mesh.uv2 = a_Array4;
            } else {
                mesh.uv2 = null;
            }

            if (CurrentNormalsMode != 0) {
                Vector3[] a_Array5 = new Vector3[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
                CopyListRangeToArray(ref a_Array5, a_DecalsMesh.Normals, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                mesh.normals = a_Array5;
            } else {
                mesh.normals = null;
            }

            if (CurrentTangentsMode != 0) {
                Vector4[] a_Array6 = new Vector4[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
                CopyListRangeToArray(ref a_Array6, a_DecalsMesh.Tangents, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);
                mesh.tangents = a_Array6;
            } else {
                mesh.tangents = null;
            }

            if (UseVertexColors) {
                Color[] a_Array7 = new Color[decalsMeshUpperVertexIndex - decalsMeshLowerVertexIndex + 1];
                CopyListRangeToArray(ref a_Array7, a_DecalsMesh.VertexColors, decalsMeshLowerVertexIndex, decalsMeshUpperVertexIndex);

                if (a_DecalsMesh.PreserveVertexColorArrays) {
                    a_DecalsMesh.PreservedVertexColorArrays.Add(a_Array7);
                }

                mesh.colors = a_Array7;
            } else {
                mesh.colors = null;
            }
        }

        static void CopyListRangeToArray<T>(ref T[] a_Array, List<T> a_List, int a_LowerListIndex, int a_UpperListIndex) {
            int num = 0;

            for (int i = a_LowerListIndex; i <= a_UpperListIndex; i++) {
                a_Array[num] = a_List[i];
                num++;
            }
        }

        Mesh MeshOfDecalsMeshRenderer(DecalsMeshRenderer a_DecalsMeshRenderer) {
            Mesh mesh;

            if (Application.isPlaying) {
                if (a_DecalsMeshRenderer.MeshFilter.mesh == null) {
                    mesh = new Mesh();
                    mesh.name = "Decal Mesh";
                    a_DecalsMeshRenderer.MeshFilter.mesh = mesh;
                } else {
                    mesh = a_DecalsMeshRenderer.MeshFilter.mesh;
                    mesh.Clear();
                }
            } else if (a_DecalsMeshRenderer.MeshFilter.sharedMesh == null) {
                mesh = new Mesh();
                mesh.name = "Decal Mesh";
                a_DecalsMeshRenderer.MeshFilter.sharedMesh = mesh;
            } else {
                mesh = a_DecalsMeshRenderer.MeshFilter.sharedMesh;
                mesh.Clear();
            }

            return mesh;
        }

        public override void OptimizeDecalsMeshes() {
            base.OptimizeDecalsMeshes();

            foreach (DecalsMeshRenderer decalsMeshRenderer in m_DecalsMeshRenderers) {
                if (Application.isPlaying) {
                    if (decalsMeshRenderer.MeshFilter != null && !(decalsMeshRenderer.MeshFilter.mesh != null)) { }
                } else if (decalsMeshRenderer.MeshFilter != null && !(decalsMeshRenderer.MeshFilter.sharedMesh != null)) { }
            }
        }

        protected abstract DecalsMeshRenderer AddDecalsMeshRendererComponentToGameObject(GameObject a_GameObject);
    }
}