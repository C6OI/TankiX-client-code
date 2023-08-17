using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class LineRendererEffectBehaviour : MonoBehaviour {
        [SerializeField] LineRendererEffect[] effects;

        public float duration = 2f;

        public bool invertAlpha;

        float elapsed;

        Material[] materials;

        public float[] LastScale { get; private set; }

        void Awake() => LastScale = new float[effects.Length];

        void Update() {
            elapsed = (elapsed + Time.deltaTime) % duration;
            float time = elapsed / duration;

            for (int i = 0; i < effects.Length; i++) {
                float num = effects[i].width.Evaluate(time);
                float num2 = effects[i].widthEnd.Evaluate(time);
                Color color = effects[i].color.Evaluate(time);
                Color color2 = effects[i].colorEnd.Evaluate(time);

                if (invertAlpha) {
                    effects[i].lineRenderer.SetWidth(num2, num);
                    effects[i].lineRenderer.SetColors(color2, color);
                } else {
                    effects[i].lineRenderer.SetWidth(num, num2);
                    effects[i].lineRenderer.SetColors(color, color2);
                }

                Vector2 mainTextureOffset = effects[i].lineRenderer.sharedMaterial.mainTextureOffset;

                mainTextureOffset.x =
                    (mainTextureOffset.x + effects[i].textureOffset.Evaluate(elapsed) * Time.deltaTime) % 1f;

                effects[i].lineRenderer.sharedMaterial.mainTextureOffset = mainTextureOffset;
            }
        }

        public void Init(float[] offsets, params Vector3[] vertices) {
            float num = 0f;

            for (int i = 1; i < vertices.Length; i++) {
                num += Vector3.Distance(vertices[i - 1], vertices[i]);
            }

            materials = new Material[effects.Length];

            for (int j = 0; j < effects.Length; j++) {
                LineRenderer lineRenderer = effects[j].lineRenderer;
                lineRenderer.material = Instantiate(lineRenderer.material);
                materials[j] = lineRenderer.material;
                Vector2 mainTextureOffset = materials[j].mainTextureOffset;

                if (effects[j].adjustTextureScale) {
                    Vector2 mainTextureScale = materials[j].mainTextureScale;
                    mainTextureScale.x = num / effects[j].fragmentLength;
                    materials[j].mainTextureScale = mainTextureScale;
                    LastScale[j] = mainTextureScale.x;
                    mainTextureOffset.x = offsets[j] % 1f;
                }

                materials[j].mainTextureOffset = mainTextureOffset;

                for (int k = 0; k < vertices.Length; k++) {
                    lineRenderer.SetPosition(k, vertices[k]);
                }
            }
        }

        [Serializable]
        class LineRendererEffect {
            public LineRenderer lineRenderer;

            public AnimationCurve width;

            public AnimationCurve widthEnd;

            public Gradient color;

            public Gradient colorEnd;

            public float fragmentLength;

            public AnimationCurve textureOffset;

            public bool adjustTextureScale;
        }
    }
}