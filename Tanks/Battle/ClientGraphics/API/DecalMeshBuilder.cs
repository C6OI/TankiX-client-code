using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class DecalMeshBuilder {
        readonly DecalMeshClipper decalMeshClipper;

        readonly DecalPolygonCollector decalPolygonCollector;

        readonly DecalUVProjector decalUVProjector;

        readonly MeshBuffersCache meshBuffersCache;
        public MeshBuilder collectedPolygonsMeshBuilder;

        public MeshBuilder meshBuilder;

        public DecalMeshBuilder() {
            collectedPolygonsMeshBuilder = new MeshBuilder();
            meshBuilder = new MeshBuilder();
            meshBuffersCache = new MeshBuffersCache();
            decalPolygonCollector = new DecalPolygonCollector(meshBuffersCache);
            decalMeshClipper = new DecalMeshClipper();
            decalUVProjector = new DecalUVProjector();
        }

        public void WarmupMeshCaches(GameObject root) {
            MeshCollider[] componentsInChildren = root.GetComponentsInChildren<MeshCollider>();

            foreach (MeshCollider meshCollider in componentsInChildren) {
                Mesh sharedMesh = meshCollider.sharedMesh;

                if (sharedMesh != null && meshCollider.gameObject.GetComponent<ParentRendererBehaviour>() != null) {
                    meshBuffersCache.GetTriangles(sharedMesh);
                    meshBuffersCache.GetVertices(sharedMesh);
                    meshBuffersCache.GetNormals(sharedMesh);
                    float[] triangleRadius;
                    Vector3[] triangleMidles;
                    meshBuffersCache.GetTriangleRadius(sharedMesh, out triangleRadius, out triangleMidles);
                }
            }
        }

        public bool Build(DecalProjection decalProjection, ref Mesh mesh) {
            Clean();

            if (!CompleteProjectionByRaycast(decalProjection)) {
                return false;
            }

            if (CollectPolygons(decalProjection)) {
                BuilldDecalFromCollectedPolygons(decalProjection);
                GetResultToMesh(ref mesh);
                return true;
            }

            return false;
        }

        public void Clean() {
            CleanCollectedPolygons();
            CleanResult();
        }

        public void CleanCollectedPolygons() => collectedPolygonsMeshBuilder.Clear();

        public void CleanResult() => meshBuilder.Clear();

        public bool CollectPolygons(DecalProjection decalProjection) =>
            decalPolygonCollector.Collect(decalProjection, collectedPolygonsMeshBuilder);

        public bool BuilldDecalFromCollectedPolygons(DecalProjection decalProjection) {
            CleanResult();
            decalMeshClipper.Clip(decalProjection, collectedPolygonsMeshBuilder, meshBuilder);
            decalUVProjector.Project(meshBuilder, decalProjection);
            return true;
        }

        public bool GetResultToMesh(ref Mesh mesh) {
            if (mesh == null) {
                mesh = new Mesh();
                mesh.MarkDynamic();
            }

            return meshBuilder.BuildToMesh(mesh);
        }

        public bool CompleteProjectionByRaycast(DecalProjection decalProjection) {
            RaycastHit hitInfo;

            if (Physics.Raycast(decalProjection.Ray.origin,
                    decalProjection.Ray.direction,
                    out hitInfo,
                    decalProjection.Distantion,
                    LayerMasks.VISUAL_STATIC)) {
                decalProjection.ProjectionHit = hitInfo;

                if (!hitInfo.normal.Equals(Vector3.zero)) {
                    return true;
                }

                return true;
            }

            return false;
        }
    }
}