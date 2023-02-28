using System;
using UnityEngine;

namespace Edelweiss.DecalSystem {
    public abstract class GenericDecals<D, P, DM> : GenericDecalsBase where D : GenericDecals<D, P, DM> where P : GenericDecalProjector<D, P, DM> where DM : GenericDecalsMesh<D, P, DM> {
        public virtual void UpdateDecalsMeshes(DM a_DecalsMesh) {
            if (a_DecalsMesh == null) {
                throw new NullReferenceException("The passed decals mesh is null.");
            }

            if (a_DecalsMesh.Decals != this) {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }

            a_DecalsMesh.PreservedVertexColorArrays.Clear();
            a_DecalsMesh.PreservedProjectedUVArrays.Clear();
            a_DecalsMesh.PreservedProjectedUV2Arrays.Clear();
        }

        public virtual void UpdateVertexColors(DM a_DecalsMesh) {
            if (Edition.IsDecalSystemFree) {
                throw new InvalidOperationException("This function is only supported in Decal System Pro.");
            }

            if (a_DecalsMesh == null) {
                throw new NullReferenceException("The passed decals mesh is null.");
            }

            if (a_DecalsMesh.Decals != this) {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }

            if (!UseVertexColors) {
                throw new InvalidOperationException("Vertex colors are not used.");
            }

            if (!a_DecalsMesh.PreserveVertexColorArrays) {
                throw new InvalidOperationException("Vertex colors can only be updated if the decals mesh preserves them.");
            }

            int num = 0;

            foreach (Color[] preservedVertexColorArray in a_DecalsMesh.PreservedVertexColorArrays) {
                num = preservedVertexColorArray.Length;
            }

            if (num != a_DecalsMesh.VertexColors.Count) {
                throw new InvalidOperationException(
                    "The preserved vertex color count doesn't match the one from the decals mesh. Avoid changes on the decals mesh which affect the number of vertices, after it was used to update the decals instance as preserving arrays are used.");
            }
        }

        public virtual void UpdateProjectedUVs(DM a_DecalsMesh) {
            if (Edition.IsDecalSystemFree) {
                throw new InvalidOperationException("This function is only supported in Decal System Pro.");
            }

            if (a_DecalsMesh == null) {
                throw new NullReferenceException("The passed decals mesh is null.");
            }

            if (a_DecalsMesh.Decals != this) {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }

            if (CurrentUVMode != 0 && CurrentUVMode != UVMode.ProjectWrapped) {
                throw new InvalidOperationException("The current uv mode is not projecting.");
            }

            if (!a_DecalsMesh.PreserveProjectedUVArrays) {
                throw new InvalidOperationException("Projected UVs can only be updated if the decals mesh preserves them.");
            }

            int num = 0;

            foreach (Vector2[] preservedProjectedUVArray in a_DecalsMesh.PreservedProjectedUVArrays) {
                num = preservedProjectedUVArray.Length;
            }

            if (num != a_DecalsMesh.UVs.Count) {
                throw new InvalidOperationException(
                    "The preserved UV count doesn't match the one from the decals mesh. Avoid changes on the decals mesh which affect the number of vertices, after it was used to update the decals instance as preserving arrays are used.");
            }
        }

        public virtual void UpdateProjectedUV2s(DM a_DecalsMesh) {
            if (Edition.IsDecalSystemFree) {
                throw new InvalidOperationException("This function is only supported in Decal System Pro.");
            }

            if (a_DecalsMesh == null) {
                throw new NullReferenceException("The passed decals mesh is null.");
            }

            if (a_DecalsMesh.Decals != this) {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }

            if (CurrentUV2Mode != UV2Mode.Project && CurrentUV2Mode != UV2Mode.ProjectWrapped) {
                throw new InvalidOperationException("The current uv2 mode is not projecting.");
            }

            if (!a_DecalsMesh.PreserveProjectedUV2Arrays) {
                throw new InvalidOperationException("Projected UV2s can only be updated if the decals mesh preserves them.");
            }

            int num = 0;

            foreach (Vector2[] preservedProjectedUV2Array in a_DecalsMesh.PreservedProjectedUV2Arrays) {
                num = preservedProjectedUV2Array.Length;
            }

            if (num != a_DecalsMesh.UV2s.Count) {
                throw new InvalidOperationException(
                    "The preserved UV2 count doesn't match the one from the decals mesh. Avoid changes on the decals mesh which affect the number of vertices, after it was used to update the decals instance as preserving arrays are used.");
            }
        }
    }
}