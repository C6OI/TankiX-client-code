using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.API {
    public class MyShadowSystem : MonoBehaviour {
        public List<CharacterShadowComponent> characters;

        public Camera camera;

        readonly List<CharacterShadowComponent> visibleCharacters = new();

        CharacterShadowCommonSettingsComponent shadowCommonSettings;

        public void Start() {
            shadowCommonSettings = GetComponent<CharacterShadowCommonSettingsComponent>();

            foreach (CharacterShadowComponent character in characters) {
                Init(character);
            }
        }

        public void Update() {
            UpdateBoundsAndCullCharacters(visibleCharacters, characters, this.camera, shadowCommonSettings);
            UpdateShadowMapSize(shadowCommonSettings, visibleCharacters, characters);
            RenderTexture shadowMap = shadowCommonSettings.shadowMap;

            if (shadowMap != null) {
                Camera camera = shadowCommonSettings.Camera;
                CalculateProjectionData(shadowCommonSettings, visibleCharacters, camera);
                GenerateDrawCommandBuffer(shadowCommonSettings, visibleCharacters);
                camera.Render();
            }

            visibleCharacters.Clear();
        }

        public void Init(CharacterShadowComponent characterShadow) {
            MyShadowInternal myShadowInternal = characterShadow.gameObject.AddComponent<MyShadowInternal>();
            myShadowInternal.Projector = CreateProjector(shadowCommonSettings);
            myShadowInternal.BaseAlpha = characterShadow.color.a;
            myShadowInternal.CasterMaterial = new Material(shadowCommonSettings.casterShader);
            Debug.Log("Shadow System Init " + shadowCommonSettings.casterShader.name);
        }

        Projector CreateProjector(CharacterShadowCommonSettingsComponent shadowCommonSettings) {
            GameObject gameObject = new("CharacterShadowProjector");
            gameObject.transform.parent = shadowCommonSettings.transform;
            Projector projector = gameObject.AddComponent<Projector>();
            projector.orthographic = true;
            projector.ignoreLayers = shadowCommonSettings.ignoreLayers;
            projector.material = new Material(shadowCommonSettings.receiverShader);
            projector.material.mainTexture = shadowCommonSettings.shadowMap;
            return projector;
        }

        void UpdateShadowMapSize(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowComponent> visibleCharacters,
            ICollection<CharacterShadowComponent> allCharacters) {
            int num = !(settings.shadowMap == null) ? settings.shadowMap.width : 0;
            bool flag = false;
            int num2 = Mathf.CeilToInt(Mathf.Sqrt(visibleCharacters.Count));
            int num3 = Mathf.NextPowerOfTwo(settings.textureSize * num2);
            int num4 = Mathf.CeilToInt(Mathf.Sqrt(allCharacters.Count));
            int num5 = Mathf.NextPowerOfTwo(settings.textureSize * num4);
            flag = flag || num3 > num;

            if (!(flag || 2 * num5 < num)) {
                return;
            }

            Destroy(settings.shadowMap);
            settings.shadowMap = null;

            if (num3 > 0) {
                settings.shadowMap = new RenderTexture(num3, num3, 0, settings.ShadowMapTextureFormat, RenderTextureReadWrite.Linear);
                settings.shadowMap.isPowerOfTwo = true;
                settings.shadowMap.filterMode = FilterMode.Bilinear;
                settings.shadowMap.useMipMap = false;
            }

            foreach (CharacterShadowComponent allCharacter in allCharacters) {
                allCharacter.GetComponent<MyShadowInternal>().Projector.material.mainTexture = settings.shadowMap;
            }
        }

        void UpdateBoundsAndCullCharacters(ICollection<CharacterShadowComponent> collector, ICollection<CharacterShadowComponent> characters, Camera camera,
            CharacterShadowCommonSettingsComponent settings) {
            Matrix4x4 localToWorldMatrix = settings.virtualLight.localToWorldMatrix;
            Matrix4x4 worldToLocalMatrix = settings.virtualLight.worldToLocalMatrix;
            Matrix4x4 worldToProjectionMatrix = camera.projectionMatrix * camera.worldToCameraMatrix * localToWorldMatrix;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix);
            int num = 0;
            int maxCharactersCountInAtlas = settings.MaxCharactersCountInAtlas;

            foreach (CharacterShadowComponent character in characters) {
                bool flag = num < maxCharactersCountInAtlas;
                Bounds boundsInLightSpace = BoundsUtils.TransformBounds(character.GetComponent<MyShadowCaster>().BoundsInWorldSpace, worldToLocalMatrix);
                CharacterShadowComponent characterShadowComponent = character;
                MyShadowInternal component = character.GetComponent<MyShadowInternal>();
                component.ProjectionBoundInLightSpace = CalculateProjectionBoundInLightSpace(settings, boundsInLightSpace, characterShadowComponent.offset);
                Bounds projectionBoundInLightSpace = component.ProjectionBoundInLightSpace;
                projectionBoundInLightSpace.max += new Vector3(0f, 0f, characterShadowComponent.attenuation);
                flag &= GeometryUtility.TestPlanesAABB(planes, projectionBoundInLightSpace);
                component.Projector.enabled = flag;

                if (flag) {
                    collector.Add(character);
                }

                num++;
            }
        }

        Bounds CalculateProjectionBoundInLightSpace(CharacterShadowCommonSettingsComponent settings, Bounds boundsInLightSpace, float offset) {
            int num = settings.textureSize - settings.blurSize - settings.blurSize - 1 - 1;
            float num2 = Mathf.Max(boundsInLightSpace.size.x, boundsInLightSpace.size.y);
            float num3 = num2 / num;
            float num4 = 2 * (1 + settings.blurSize) * num3;
            float num5 = boundsInLightSpace.size.x + num4;
            float num6 = boundsInLightSpace.size.y + num4;

            if (num5 > num6) {
                float num7 = (Mathf.Ceil((num6 - 0.01f) / (num3 + num3)) * (num3 + num3) - num6) * 0.5f;
                num6 += num7;
            } else {
                float num8 = (Mathf.Ceil((num5 - 0.01f) / (num3 + num3)) * (num3 + num3) - num5) * 0.5f;
                num5 += num8;
            }

            Bounds result = default;
            result.size = new Vector3(num5, num6, boundsInLightSpace.size.z - offset);
            result.center = new Vector3(boundsInLightSpace.center.x, boundsInLightSpace.center.y, boundsInLightSpace.max.z - result.extents.z);
            return result;
        }

        void CalculateProjectionData(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowComponent> characters, Camera camera) {
            Quaternion rotation = settings.virtualLight.rotation;
            Matrix4x4 localToWorldMatrix = settings.virtualLight.localToWorldMatrix;
            Matrix4x4 inverse = Matrix4x4.TRS(Vector3.zero, rotation, new Vector3(1f, 1f, -1f)).inverse;
            Matrix4x4 matrix4x = inverse * camera.cameraToWorldMatrix;
            int num = settings.shadowMap.width / settings.textureSize;
            int num2 = settings.shadowMap.height / settings.textureSize;
            int num3 = 0;

            foreach (CharacterShadowComponent character in characters) {
                int num4 = num3 / num;
                int num5 = num3 - num4 * num;
                Rect atlasData = CalculateLocationInAtlas(num, num2, num5, num4);
                MyShadowInternal component = character.GetComponent<MyShadowInternal>();
                Bounds projectionBoundInLightSpace = component.ProjectionBoundInLightSpace;
                SetProjectorData(component, localToWorldMatrix.MultiplyPoint3x4(projectionBoundInLightSpace.center), rotation, character, atlasData);
                Vector3 center = projectionBoundInLightSpace.center;
                Matrix4x4 identity = Matrix4x4.identity;
                identity.SetColumn(3, new Vector4(0f - center.x, 0f - center.y, center.z, 1f));
                Matrix4x4 matrix4x2 = identity * matrix4x;
                float num6 = Mathf.Max(projectionBoundInLightSpace.extents.x, projectionBoundInLightSpace.extents.y);
                float z = projectionBoundInLightSpace.extents.z;
                float left = (-num5 * 2 - 1) * num6;
                float right = (-num5 * 2 - 1 + num * 2) * num6;
                float bottom = (-num4 * 2 - 1) * num6;
                float top = (-num4 * 2 - 1 + num2 * 2) * num6;
                Matrix4x4 matrix4x3 = Matrix4x4.Ortho(left, right, bottom, top, 0f - z, z);
                component.CasterMaterial.SetMatrix("ViewToAtlas", GL.GetGPUProjectionMatrix(matrix4x3 * matrix4x2, true));
                num3++;
            }
        }

        static Rect CalculateLocationInAtlas(int countX, int countY, int xIndex, int yIndex) {
            float num = 1f / countX;
            float num2 = 1f / countY;
            float x = 2 * xIndex * num * 0.5f;
            float y = 2 * yIndex * num2 * 0.5f;
            return new Rect(new Rect(new Vector2(x, y), new Vector2(num, num2)));
        }

        void SetProjectorData(MyShadowInternal characterInternal, Vector3 position, Quaternion rotation, CharacterShadowComponent characterShadow, Rect atlasData) {
            Projector projector = characterInternal.Projector;
            Bounds projectionBoundInLightSpace = characterInternal.ProjectionBoundInLightSpace;
            float num = projectionBoundInLightSpace.size.x / projectionBoundInLightSpace.size.y;
            projector.transform.position = position;
            projector.transform.rotation = rotation;
            projector.orthographicSize = Mathf.Max(projectionBoundInLightSpace.extents.x, projectionBoundInLightSpace.extents.y);
            projector.aspectRatio = num;
            projector.nearClipPlane = 0f - projectionBoundInLightSpace.extents.z;
            projector.farClipPlane = projectionBoundInLightSpace.extents.z + characterShadow.attenuation;
            float num2 = projectionBoundInLightSpace.size.z + 0.01f;
            float x = num;
            float y = (num2 + characterShadow.attenuation) / num2;
            float z = !(characterShadow.attenuation > 0f) ? 100000f : 1f / (characterShadow.attenuation / num2);
            float w = !(characterShadow.backFadeRange > 0f) ? 100000f : 1f / (characterShadow.backFadeRange / num2);
            Material material = projector.material;
            material.SetVector("Params", new Vector4(x, y, z, w));
            material.mainTextureOffset = atlasData.position;
            material.mainTextureScale = atlasData.size;
            material.color = characterShadow.color;
        }

        void GenerateDrawCommandBuffer(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowComponent> characters) {
            RenderTexture shadowMap = settings.shadowMap;
            CommandBuffer commandBuffer = settings.CommandBuffer;
            commandBuffer.Clear();
            commandBuffer.GetTemporaryRT(settings.RawShadowMapNameId, shadowMap.width, shadowMap.height, 16, FilterMode.Point, shadowMap.format, RenderTextureReadWrite.Linear);
            commandBuffer.SetRenderTarget(settings.RawShadowMapId);
            commandBuffer.ClearRenderTarget(true, true, Color.black, 0f);

            foreach (CharacterShadowComponent character in characters) {
                GenerateDrawCharacterCommands(commandBuffer, character.GetComponent<MyShadowCaster>(), character.GetComponent<MyShadowInternal>());
            }

            commandBuffer.GetTemporaryRT(settings.BlurredShadowMapNameId, shadowMap.width, shadowMap.height, 0, FilterMode.Point, shadowMap.format, RenderTextureReadWrite.Linear);
            commandBuffer.Blit(settings.RawShadowMapId, settings.BlurredShadowMapId, settings.HorizontalBlurMaterial, settings.blurSize);
            commandBuffer.ReleaseTemporaryRT(settings.RawShadowMapNameId);
            commandBuffer.Blit(settings.BlurredShadowMapId, new RenderTargetIdentifier(shadowMap), settings.VerticalBlurMaterial, settings.blurSize);
            commandBuffer.ReleaseTemporaryRT(settings.BlurredShadowMapNameId);
        }

        void GenerateDrawCharacterCommands(CommandBuffer commandBuffer, MyShadowCaster casters, MyShadowInternal shadowInternal) {
            List<Renderer> renderers = casters.Renderers;

            for (int i = 0; i < renderers.Count; i++) {
                commandBuffer.DrawRenderer(renderers[i], shadowInternal.CasterMaterial);
            }
        }
    }
}