using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    internal class TrackMarksRenderComponent : Component {
        public bool dirty;
        public Mesh mesh;

        public Color[] meshColors;

        public Vector3[] meshNormals;

        public Vector3[] meshPositions;

        public int[] meshTris;

        public Vector2[] meshUVs;

        public TrackRenderData[] trackRenderDatas;

        public void Clear() {
            mesh.Clear();

            for (int i = 0; i < trackRenderDatas.Length; i++) {
                trackRenderDatas[i].Reset();
            }

            for (int j = 0; j < meshPositions.Length; j++) {
                meshPositions[j] = Vector3.zero;
                meshUVs[j] = Vector2.zero;
                meshNormals[j] = Vector3.zero;
                meshColors[j] = Color.white;
            }

            for (int k = 0; k < meshTris.Length; k++) {
                meshTris[k] = 0;
            }
        }
    }
}