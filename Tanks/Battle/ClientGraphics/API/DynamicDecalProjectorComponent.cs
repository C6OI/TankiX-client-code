using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class DynamicDecalProjectorComponent : MonoBehaviour, Component {
        [SerializeField] Material material;

        [SerializeField] Color color = new(0.5f, 0.5f, 0.5f, 0.5f);

        [SerializeField] float lifeTime = 20f;

        [SerializeField] float halfSize = 1f;

        [SerializeField] float randomKoef = 0.1f;

        [SerializeField] bool randomRotation = true;

        [SerializeField] int atlasHTilesCount = 1;

        [SerializeField] int atlasVTilesCount = 1;

        [SerializeField] float distance = 100f;

        [HideInInspector] [SerializeField] int[] surfaceAtlasPositions = new int[5];

        public Material Material => material;

        public Color Color => color;

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