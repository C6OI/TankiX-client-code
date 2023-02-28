using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MaskedTerrain {
        readonly Mask mask;

        readonly List<MeshCollider> meshColliders;

        int counter;

        readonly float pixelLengthInWorld;

        readonly float pixelWidthInWorld;

        Bounds terrainBounds;

        public MaskedTerrain(Terrain terrain, Mask mask) {
            meshColliders = terrain.MeshColliders;
            terrainBounds = terrain.Bounds;
            this.mask = mask;
            pixelWidthInWorld = terrainBounds.size.x / mask.Width;
            pixelLengthInWorld = terrainBounds.size.z / mask.Height;
        }

        public bool FindPosition(int pixelX, int pixelY, out GrassPosition position) {
            float x = terrainBounds.min.x + pixelX * pixelWidthInWorld;
            float num = terrainBounds.max.y + 10f;
            float z = terrainBounds.min.z + pixelY * pixelLengthInWorld;
            Ray ray = new(new Vector3(x, num + 1f, z), new Vector3(0f, -1f, 0f));
            float maxDistance = terrainBounds.size.y + 100f;
            position = default;

            foreach (MeshCollider meshCollider in meshColliders) {
                Renderer component = meshCollider.GetComponent<Renderer>();
                RaycastHit hitInfo;

                if (!meshCollider.Raycast(ray, out hitInfo, maxDistance)) {
                    continue;
                }

                position.position = hitInfo.point;
                RaycastHit hitInfo2;

                if (Physics.Raycast(position.position + Vector3.up * 2f, -Vector3.up, out hitInfo2, 2f) && hitInfo2.point.y > position.position.y + 0.01f &&
                    hitInfo2.collider != hitInfo.collider) {
                    continue;
                }

                int lightmapIndex = component.lightmapIndex;

                if (lightmapIndex >= 0) {
                    Vector2 lightmapCoord = hitInfo.lightmapCoord;
                    position.lightmapUV = lightmapCoord;
                    position.lightmapId = lightmapIndex;
                    return true;
                }

                position.lightmapId = lightmapIndex;
                return true;
            }

            position.position = ray.origin;
            return false;
        }

        public List<GrassPosition> FindPositions(List<Vector2> pixelsCoords) {
            List<GrassPosition> list = new();

            for (int i = 0; i < pixelsCoords.Count; i++) {
                Vector2 vector = pixelsCoords[i];
                GrassPosition position;

                if (FindPosition((int)vector.x, (int)vector.y, out position)) {
                    list.Add(position);
                }
            }

            return list;
        }

        public float CalculateMarkedSquare() {
            float num = pixelWidthInWorld * pixelLengthInWorld;
            return (int)(num * mask.MarkedPixels.Count);
        }
    }
}