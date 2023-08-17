using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    class TrackMarksRenderComponent : Component {
        public bool dirty;
        public Mesh mesh;

        public Color[] meshColors;

        public Vector3[] meshNormals;

        public Vector3[] meshPositions;

        public int[] meshTris;

        public Vector2[] meshUVs;

        public TrackRenderData[] tracks;
    }
}