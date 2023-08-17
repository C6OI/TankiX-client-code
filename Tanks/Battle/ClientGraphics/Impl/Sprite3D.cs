using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class Sprite3D : MonoBehaviour {
        static Mesh _planeMesh;

        public Material material;

        public float width = 100f;

        public float height = 100f;

        public float scale = 1f;

        public float originX = 0.5f;

        public float originY = 0.5f;

        public bool receiveShadows = true;

        public bool castShadows = true;

        public bool useOwnRotation;

        public bool useInstanceMaterial;

        public float offsetToCamera;

        public float minDistanceToCamera;

        protected Material assetMaterial;
        bool currentMaterialInstance;

        Material instanceMaterial;

        protected void Awake() {
            assetMaterial = material;
            UpdateMaterial();
        }

        protected void Start() => _planeMesh = CreatePlane();

        void LateUpdate() => Draw();

        protected void OnDestroy() {
            if (useInstanceMaterial) {
                Destroy(instanceMaterial);
            }
        }

        public virtual void Draw() {
            Camera main = Camera.main;

            if (!(main == null)) {
                Matrix4x4 matrix4x = default;
                matrix4x.m00 = 1f;
                matrix4x.m11 = 1f;
                matrix4x.m22 = 1f;
                matrix4x.m33 = 1f;
                matrix4x.m03 = 100f * (originX - 0.5f);
                matrix4x.m13 = 100f * (originY - 0.5f);

                Quaternion q = !useOwnRotation ? Quaternion.LookRotation(-main.transform.forward)
                                   : gameObject.transform.rotation;

                Vector3 s = new(scale * width / 100f, scale * height / 100f, 1f);
                Matrix4x4 matrix4x2 = default;
                Vector3 vector = main.transform.position - transform.position;
                float b = Mathf.Max(0f, vector.magnitude - minDistanceToCamera);
                b = Mathf.Min(offsetToCamera, b);
                Vector3 pos = transform.position + vector.normalized * b;
                matrix4x2.SetTRS(pos, q, s);
                UpdateMaterialIfNeeded();

                Graphics.DrawMesh(_planeMesh,
                    matrix4x2 * matrix4x,
                    material,
                    gameObject.layer,
                    null,
                    0,
                    null,
                    castShadows,
                    receiveShadows);
            }
        }

        protected void UpdateMaterialIfNeeded() {
            if (currentMaterialInstance != useInstanceMaterial) {
                UpdateMaterial();
            }
        }

        protected void UpdateMaterial() {
            currentMaterialInstance = useInstanceMaterial;

            if (useInstanceMaterial) {
                instanceMaterial = Instantiate(assetMaterial);
                material = instanceMaterial;
                return;
            }

            material = assetMaterial;

            if (instanceMaterial != null) {
                Destroy(instanceMaterial);
            }
        }

        static Mesh CreatePlane() {
            Mesh mesh = new();

            mesh.vertices = new Vector3[4] {
                new(50f, -50f, 0f),
                new(-50f, -50f, 0f),
                new(-50f, 50f, 0f),
                new(50f, 50f, 0f)
            };

            mesh.triangles = new int[6] { 0, 3, 2, 1, 0, 2 };

            mesh.uv = new Vector2[4] {
                new(0f, 0f),
                new(1f, 0f),
                new(1f, 1f),
                new(0f, 1f)
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            return mesh;
        }
    }
}