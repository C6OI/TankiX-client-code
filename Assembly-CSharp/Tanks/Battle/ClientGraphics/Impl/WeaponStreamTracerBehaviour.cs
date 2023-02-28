using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(LineRenderer))]
    public class WeaponStreamTracerBehaviour : MonoBehaviour {
        [SerializeField] float speed = 10f;

        [SerializeField] float fragmentLength = 30f;

        LineRenderer lineRenderer;

        float textureOffset;

        public Vector3 TargetPosition { get; set; }

        public float Speed {
            get => speed;
            set => speed = value;
        }

        public float FragmentLength {
            get => fragmentLength;
            set => fragmentLength = value;
        }

        void Awake() {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.sharedMaterial = Instantiate(lineRenderer.material);
        }

        void Update() {
            lineRenderer.SetPosition(1, TargetPosition);
            float magnitude = TargetPosition.magnitude;
            Vector2 mainTextureScale = lineRenderer.sharedMaterial.mainTextureScale;
            mainTextureScale.x = magnitude / fragmentLength;
            lineRenderer.sharedMaterial.mainTextureScale = mainTextureScale;
            Vector2 mainTextureOffset = lineRenderer.sharedMaterial.mainTextureOffset;
            mainTextureOffset.x = (mainTextureOffset.x + speed * Time.deltaTime) % 1f;
            lineRenderer.sharedMaterial.mainTextureOffset = mainTextureOffset;
        }
    }
}