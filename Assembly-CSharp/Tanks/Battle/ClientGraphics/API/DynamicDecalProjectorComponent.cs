using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class DynamicDecalProjectorComponent : BehaviourComponent {
        [SerializeField] Material material;

        [SerializeField] Color color = new(0.5f, 0.5f, 0.5f, 0.5f);

        [SerializeField] bool emit;

        [SerializeField] float lifeTime = 20f;

        [SerializeField] float halfSize = 1f;

        [SerializeField] float randomKoef = 0.1f;

        [SerializeField] bool randomRotation = true;

        [SerializeField] int atlasHTilesCount = 1;

        [SerializeField] int atlasVTilesCount = 1;

        [SerializeField] float distance = 100f;

        [SerializeField] [HideInInspector] int[] surfaceAtlasPositions = new int[5];

        public Material Material {
            get => material;
            set => material = value;
        }

        public Color Color => color;

        public bool Emmit => emit;

        public float LifeTime => lifeTime;

        public float HalfSize => halfSize + Random.Range(0f, halfSize * randomKoef);

        public Vector3 Up => !randomRotation ? Vector3.up : Random.insideUnitSphere;

        public int AtlasHTilesCount => atlasHTilesCount;

        public int AtlasVTilesCount => atlasVTilesCount;

        public float Distance => distance;

        public int[] SurfaceAtlasPositions {
            get => surfaceAtlasPositions;
            set => surfaceAtlasPositions = value;
        }
    }
}