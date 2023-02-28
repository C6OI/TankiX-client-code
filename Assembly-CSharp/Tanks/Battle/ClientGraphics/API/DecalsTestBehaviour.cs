using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.API {
    public class DecalsTestBehaviour : MonoBehaviour {
        [SerializeField] Material decalMaterial;

        [SerializeField] float projectDistantion = 100f;

        [SerializeField] float projectSize = 1f;

        [SerializeField] int hTilesCount = 1;

        [SerializeField] int vTilesCount = 1;

        [SerializeField] float mouseWheelSenetivity = 0.1f;

        [SerializeField] int[] surfaceAtlasPositions = new int[5];

        [SerializeField] bool updateEveryFrame = true;

        int counter;

        Mesh decalMesh;

        readonly DecalMeshBuilder meshBuilder = new();

        MeshFilter meshFilter;

        Renderer renderer;

        public void Start() {
            CreateMesh();
        }

        public void Update() {
            if (Input.GetMouseButtonUp(0)) {
                CreateMesh();
                UpdateDecalMesh();
            }

            if (!updateEveryFrame) {
                UpdateDecalMesh();
            }
        }

        void UpdateDecalMesh() {
            Camera main = Camera.main;
            DecalProjection decalProjection = new();
            decalProjection.Ray = main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            decalProjection.Distantion = projectDistantion;
            decalProjection.HalfSize = projectSize;
            decalProjection.AtlasHTilesCount = hTilesCount;
            decalProjection.AtlasVTilesCount = vTilesCount;
            decalProjection.SurfaceAtlasPositions = surfaceAtlasPositions;
            DecalProjection decalProjection2 = decalProjection;
            meshBuilder.Build(decalProjection2, ref decalMesh);
        }

        void CreateMesh() {
            GameObject gameObject = new("Decal Mesh");
            decalMesh = new Mesh();
            decalMesh.MarkDynamic();
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = decalMesh;
            renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = new Material(decalMaterial);
            renderer.material.renderQueue = decalMaterial.renderQueue + ++counter;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = true;
            renderer.useLightProbes = true;
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
        }
    }
}