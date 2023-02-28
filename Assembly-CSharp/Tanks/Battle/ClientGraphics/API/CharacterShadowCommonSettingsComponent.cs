using UnityEngine;
using UnityEngine.Rendering;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class CharacterShadowCommonSettingsComponent : MonoBehaviour, Component {
        public Shader blurShader;

        public Shader casterShader;

        public Shader receiverShader;

        public LayerMask ignoreLayers = 0;

        public int maxShadowMapAtlasSize = 2048;

        public int textureSize = 128;

        public int blurSize = 5;

        public Transform virtualLight;

        public RenderTexture shadowMap;

        public RenderTextureFormat ShadowMapTextureFormat {
            get {
                if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8)) {
                    return RenderTextureFormat.R8;
                }

                return RenderTextureFormat.ARGB32;
            }
        }

        public Camera Camera { get; private set; }

        public Material HorizontalBlurMaterial { get; private set; }

        public Material VerticalBlurMaterial { get; private set; }

        public CommandBuffer CommandBuffer { get; private set; }

        public int RawShadowMapNameId { get; private set; }

        public RenderTargetIdentifier RawShadowMapId { get; private set; }

        public int BlurredShadowMapNameId { get; private set; }

        public RenderTargetIdentifier BlurredShadowMapId { get; private set; }

        public int MaxCharactersCountInAtlas {
            get {
                int num = maxShadowMapAtlasSize / textureSize;
                return num * num;
            }
        }

        public void Awake() {
            if (!ShadowsSupported()) {
                Destroy(gameObject);
                return;
            }

            CommandBuffer = new CommandBuffer();
            RawShadowMapNameId = Shader.PropertyToID("rawShadowMapNameId");
            RawShadowMapId = new RenderTargetIdentifier(RawShadowMapNameId);
            BlurredShadowMapNameId = Shader.PropertyToID("blurredShadowMapNameId");
            BlurredShadowMapId = new RenderTargetIdentifier(BlurredShadowMapNameId);
            HorizontalBlurMaterial = new Material(blurShader);
            VerticalBlurMaterial = new Material(blurShader);
            HorizontalBlurMaterial.SetVector("Direction", new Vector2(1f, 0f));
            VerticalBlurMaterial.SetVector("Direction", new Vector2(0f, 1f));
            Camera = CreateCamera();
            Camera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, CommandBuffer);
        }

        public bool ShadowsSupported() {
            if (SystemInfo.supportsRenderTextures) {
                if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8)) {
                    return true;
                }

                if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32)) {
                    return true;
                }
            }

            return false;
        }

        Camera CreateCamera() {
            GameObject gameObject = new("CharacterShadowCamera");
            gameObject.transform.parent = transform;
            Camera camera = gameObject.AddComponent<Camera>();
            camera.depth = -10f;
            camera.renderingPath = RenderingPath.VertexLit;
            camera.clearFlags = CameraClearFlags.Nothing;
            camera.backgroundColor = new Color(1f, 1f, 1f, 1f);
            camera.cullingMask = 0;
            camera.orthographic = true;
            camera.enabled = false;
            return camera;
        }
    }
}