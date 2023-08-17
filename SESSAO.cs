using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Sonic Ether/SESSAO")]
public class SESSAO : MonoBehaviour {
    public bool visualizeSSAO;

    [Range(0.02f, 5f)] public float radius = 1f;

    [Range(-0.2f, 0.5f)] public float bias = 0.1f;

    [Range(0.1f, 3f)] public float bilateralDepthTolerance = 0.2f;

    [Range(1f, 5f)] public float zThickness = 2.35f;

    [Range(0.5f, 5f)] public float occlusionIntensity = 1.3f;

    [Range(1f, 6f)] public float sampleDistributionCurve = 1.15f;

    [Range(0f, 1f)] public float colorBleedAmount = 1f;

    [Range(0.1f, 3f)] public float brightnessThreshold;

    public float drawDistance = 500f;

    public float drawDistanceFadeSize = 1f;

    public bool reduceSelfBleeding = true;

    public bool useDownsampling;

    public bool halfSampling;

    public bool preserveDetails;

    [HideInInspector] public Camera attachedCamera;

    Texture2D ditherTexture;

    Texture2D ditherTextureSmall;

    object initChecker;
    Material material;

    bool skipThisFrame;

    void Start() => CheckInit();

    void Update() {
        drawDistance = Mathf.Max(0f, drawDistance);
        drawDistanceFadeSize = Mathf.Max(0.001f, drawDistanceFadeSize);
        bilateralDepthTolerance = Mathf.Max(1E-06f, bilateralDepthTolerance);
    }

    void OnEnable() => CheckInit();

    void OnDisable() => Cleanup();

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        CheckInit();

        if (skipThisFrame) {
            Graphics.Blit(source, destination);
            return;
        }

        material.hideFlags = HideFlags.HideAndDontSave;
        material.SetTexture("_DitherTexture", !preserveDetails ? ditherTexture : ditherTextureSmall);
        material.SetInt("PreserveDetails", preserveDetails ? 1 : 0);
        material.SetMatrix("ProjectionMatrixInverse", GetComponent<Camera>().projectionMatrix.inverse);
        RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBHalf);
        RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBHalf);
        RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
        temporary3.wrapMode = TextureWrapMode.Clamp;
        temporary3.filterMode = FilterMode.Bilinear;
        Graphics.Blit(source, temporary3);
        material.SetTexture("_ColorDownsampled", temporary3);
        RenderTexture renderTexture = null;
        material.SetFloat("Radius", radius);
        material.SetFloat("Bias", bias);
        material.SetFloat("DepthTolerance", bilateralDepthTolerance);
        material.SetFloat("ZThickness", zThickness);
        material.SetFloat("Intensity", occlusionIntensity);
        material.SetFloat("SampleDistributionCurve", sampleDistributionCurve);
        material.SetFloat("ColorBleedAmount", colorBleedAmount);
        material.SetFloat("DrawDistance", drawDistance);
        material.SetFloat("DrawDistanceFadeSize", drawDistanceFadeSize);
        material.SetFloat("SelfBleedReduction", !reduceSelfBleeding ? 0f : 1f);
        material.SetFloat("BrightnessThreshold", brightnessThreshold);
        material.SetInt("HalfSampling", halfSampling ? 1 : 0);
        material.SetInt("Orthographic", attachedCamera.orthographic ? 1 : 0);

        if (useDownsampling) {
            renderTexture = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, RenderTextureFormat.ARGBHalf);
            renderTexture.filterMode = FilterMode.Bilinear;
            material.SetInt("Downsamp", 1);
            Graphics.Blit(source, renderTexture, material, colorBleedAmount <= 0.0001f ? 1 : 0);
        } else {
            material.SetInt("Downsamp", 0);
            Graphics.Blit(source, temporary, material, colorBleedAmount <= 0.0001f ? 1 : 0);
        }

        RenderTexture.ReleaseTemporary(temporary3);
        material.SetFloat("BlurDepthTolerance", 0.1f);
        int pass = !attachedCamera.orthographic ? 2 : 6;

        if (attachedCamera.orthographic) {
            material.SetFloat("Near", attachedCamera.nearClipPlane);
            material.SetFloat("Far", attachedCamera.farClipPlane);
        }

        if (useDownsampling) {
            material.SetVector("Kernel", new Vector2(2f, 0f));
            Graphics.Blit(renderTexture, temporary2, material, pass);
            RenderTexture.ReleaseTemporary(renderTexture);
            material.SetVector("Kernel", new Vector2(0f, 2f));
            Graphics.Blit(temporary2, temporary, material, pass);
            material.SetVector("Kernel", new Vector2(2f, 0f));
            Graphics.Blit(temporary, temporary2, material, pass);
            material.SetVector("Kernel", new Vector2(0f, 2f));
            Graphics.Blit(temporary2, temporary, material, pass);
        } else {
            material.SetVector("Kernel", new Vector2(1f, 0f));
            Graphics.Blit(temporary, temporary2, material, pass);
            material.SetVector("Kernel", new Vector2(0f, 1f));
            Graphics.Blit(temporary2, temporary, material, pass);
            material.SetVector("Kernel", new Vector2(1f, 0f));
            Graphics.Blit(temporary, temporary2, material, pass);
            material.SetVector("Kernel", new Vector2(0f, 1f));
            Graphics.Blit(temporary2, temporary, material, pass);
        }

        RenderTexture.ReleaseTemporary(temporary2);
        material.SetTexture("_SSAO", temporary);

        if (!visualizeSSAO) {
            Graphics.Blit(source, destination, material, 3);
        } else {
            Graphics.Blit(source, destination, material, 5);
        }

        RenderTexture.ReleaseTemporary(temporary);
    }

    void CheckInit() {
        if (initChecker == null) {
            Init();
        }
    }

    void Init() {
        skipThisFrame = false;
        Shader shader = Shader.Find("Hidden/SESSAO");

        if (!shader) {
            skipThisFrame = true;
            return;
        }

        material = new Material(shader);
        attachedCamera = GetComponent<Camera>();
        attachedCamera.depthTextureMode |= DepthTextureMode.Depth;
        attachedCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
        SetupDitherTexture();
        SetupDitherTextureSmall();
        initChecker = new object();
    }

    void Cleanup() {
        DestroyImmediate(material);
        initChecker = null;
    }

    void SetupDitherTextureSmall() {
        ditherTextureSmall = new Texture2D(3, 3, TextureFormat.Alpha8, false);
        ditherTextureSmall.filterMode = FilterMode.Point;
        float[] array = new float[9] { 8f, 1f, 6f, 3f, 0f, 4f, 7f, 2f, 5f };

        for (int i = 0; i < 9; i++) {
            Color color = new(0f, 0f, 0f, array[i] / 9f);
            int x = i % 3;
            int y = Mathf.FloorToInt(i / 3f);
            ditherTextureSmall.SetPixel(x, y, color);
        }

        ditherTextureSmall.Apply();
        ditherTextureSmall.hideFlags = HideFlags.HideAndDontSave;
    }

    void SetupDitherTexture() {
        ditherTexture = new Texture2D(5, 5, TextureFormat.Alpha8, false);
        ditherTexture.filterMode = FilterMode.Point;

        float[] array = new float[25] {
            12f, 1f, 10f, 3f, 20f, 5f, 18f, 7f, 16f, 9f,
            24f, 2f, 11f, 6f, 22f, 15f, 8f, 0f, 13f, 19f,
            4f, 21f, 14f, 23f, 17f
        };

        for (int i = 0; i < 25; i++) {
            Color color = new(0f, 0f, 0f, array[i] / 25f);
            int x = i % 5;
            int y = Mathf.FloorToInt(i / 5f);
            ditherTexture.SetPixel(x, y, color);
        }

        ditherTexture.Apply();
        ditherTexture.hideFlags = HideFlags.HideAndDontSave;
    }
}