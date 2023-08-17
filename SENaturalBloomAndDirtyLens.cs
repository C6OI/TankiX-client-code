using UnityEngine;

[AddComponentMenu("Image Effects/Sonic Ether/SE Natural Bloom and Dirty Lens")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class SENaturalBloomAndDirtyLens : MonoBehaviour {
    [Range(0f, 0.4f)] public float bloomIntensity = 0.05f;

    public Shader shader;

    public Texture2D lensDirtTexture;

    [Range(0f, 0.95f)] public float lensDirtIntensity = 0.05f;

    public bool inputIsHDR;

    readonly float blurSize = 4f;

    bool isSupported;

    Material material;

    void Start() {
        isSupported = true;

        if (!material) {
            material = new Material(shader);
        }

        if (!SystemInfo.supportsImageEffects ||
            !SystemInfo.supportsRenderTextures ||
            !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)) {
            isSupported = false;
        }
    }

    void OnDisable() {
        if ((bool)material) {
            DestroyImmediate(material);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (!isSupported) {
            Graphics.Blit(source, destination);
            return;
        }

        if (!material) {
            material = new Material(shader);
        }

        material.hideFlags = HideFlags.HideAndDontSave;
        material.SetFloat("_BloomIntensity", Mathf.Exp(bloomIntensity) - 1f);
        material.SetFloat("_LensDirtIntensity", Mathf.Exp(lensDirtIntensity) - 1f);
        source.filterMode = FilterMode.Bilinear;
        RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
        Graphics.Blit(source, temporary, material, 4);
        int num = source.width / 2;
        int num2 = source.height / 2;
        RenderTexture source2 = temporary;
        float num3 = 1f;
        int num4 = 2;

        for (int i = 0; i < 6; i++) {
            RenderTexture renderTexture = RenderTexture.GetTemporary(num, num2, 0, source.format);
            renderTexture.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source2, renderTexture, material, 1);
            source2 = renderTexture;
            num3 = i <= 1 ? 0.5f : 1f;

            if (i == 2) {
                num3 = 0.75f;
            }

            for (int j = 0; j < num4; j++) {
                material.SetFloat("_BlurSize", (blurSize * 0.5f + j) * num3);
                RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2, 0, source.format);
                temporary2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(renderTexture, temporary2, material, 2);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary2;
                temporary2 = RenderTexture.GetTemporary(num, num2, 0, source.format);
                temporary2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(renderTexture, temporary2, material, 3);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary2;
            }

            switch (i) {
                case 0:
                    material.SetTexture("_Bloom0", renderTexture);
                    break;

                case 1:
                    material.SetTexture("_Bloom1", renderTexture);
                    break;

                case 2:
                    material.SetTexture("_Bloom2", renderTexture);
                    break;

                case 3:
                    material.SetTexture("_Bloom3", renderTexture);
                    break;

                case 4:
                    material.SetTexture("_Bloom4", renderTexture);
                    break;

                case 5:
                    material.SetTexture("_Bloom5", renderTexture);
                    break;
            }

            RenderTexture.ReleaseTemporary(renderTexture);
            num /= 2;
            num2 /= 2;
        }

        material.SetTexture("_LensDirt", lensDirtTexture);
        Graphics.Blit(temporary, destination, material, 0);
        RenderTexture.ReleaseTemporary(temporary);
    }
}