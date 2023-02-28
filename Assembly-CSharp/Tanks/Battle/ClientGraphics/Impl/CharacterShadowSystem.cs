using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientLogger.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CharacterShadowSystem : ECSSystem {
        readonly List<CharacterShadowNode> visibleCharacters = new();

        [OnEventFire]
        public void Init(NodeAddedEvent evt, NotInitedCharacterShadowNode node, [JoinAll] SingleNode<CharacterShadowCommonSettingsComponent> characterShadowCommonSettings) {
            if (!node.characterShadowCasters.HasBounds) {
                LoggerProvider.GetLogger(this).Error("Incorrect character shadow elements on gameObject " + node.characterShadow.gameObject.name);
                return;
            }

            CharacterShadowInternalComponent characterShadowInternalComponent = new();
            CalculateShadowInternalData(characterShadowInternalComponent, node.characterShadow, characterShadowCommonSettings.component);
            node.Entity.AddComponent(characterShadowInternalComponent);
        }

        [OnEventFire]
        public void CleanInternalShadowData(NodeRemoveEvent evt, CharacterShadowNode node) {
            CharacterShadowInternalComponent characterShadowInternal = node.characterShadowInternal;
            GameObject gameObject = characterShadowInternal.Projector.gameObject;
            Object.Destroy(characterShadowInternal.Projector.material);
            Object.Destroy(characterShadowInternal.Projector);
            Object.Destroy(characterShadowInternal.CasterMaterial);
            Object.Destroy(gameObject);
            characterShadowInternal.Projector = null;
            characterShadowInternal.CasterMaterial = null;
        }

        [OnEventFire]
        public void UpdateFadeAlpha(TimeUpdateEvent e, FadeShadowNode fadeNode, [JoinByTank] SingleNode<TankDeadStateComponent> deadTank,
            [JoinAll] SingleNode<CharacterShadowCommonSettingsComponent> shadowHelper) {
            fadeNode.characterShadow.color.a = fadeNode.transparencyTransition.CurrentAlpha * fadeNode.characterShadowInternal.BaseAlpha;
        }

        [OnEventComplete]
        public void UpdateCharacterShadows(TimeUpdateEvent evt, SingleNode<CharacterShadowCommonSettingsComponent> characterShadowCommonSettingsNode,
            [JoinAll] ICollection<CharacterShadowNode> characters, [JoinAll] BattleCameraNode battleCamera) {
            CharacterShadowCommonSettingsComponent component = characterShadowCommonSettingsNode.component;
            UpdateBoundsAndCullCharacters(visibleCharacters, characters, battleCamera.camera, component);
            UpdateShadowMapSize(component, visibleCharacters, characters);

            if (component.shadowMap != null) {
                Camera camera = component.Camera;
                CalculateProjectionData(component, visibleCharacters, camera);
                GenerateDrawCommandBuffer(component, visibleCharacters);
                camera.Render();
            }

            visibleCharacters.Clear();
        }

        void CalculateShadowInternalData(CharacterShadowInternalComponent shadowInternalData, CharacterShadowComponent characterShadow,
            CharacterShadowCommonSettingsComponent shadowCommonSettings) {
            Projector projector = CreateProjector(shadowCommonSettings);
            shadowInternalData.Projector = projector;
            shadowInternalData.BaseAlpha = characterShadow.color.a;
            shadowInternalData.CasterMaterial = new Material(shadowCommonSettings.casterShader);
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

        void UpdateBoundsAndCullCharacters(ICollection<CharacterShadowNode> collector, ICollection<CharacterShadowNode> characters, CameraComponent cameraComponent,
            CharacterShadowCommonSettingsComponent settings) {
            Matrix4x4 localToWorldMatrix = settings.virtualLight.localToWorldMatrix;
            Matrix4x4 worldToLocalMatrix = settings.virtualLight.worldToLocalMatrix;
            Matrix4x4 worldToProjectionMatrix = cameraComponent.ProjectionMatrix * cameraComponent.WorldToCameraMatrix * localToWorldMatrix;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix);
            int num = 0;
            int maxCharactersCountInAtlas = settings.MaxCharactersCountInAtlas;

            foreach (CharacterShadowNode character in characters) {
                bool flag = num < maxCharactersCountInAtlas;
                Bounds boundsInLightSpace = BoundsUtils.TransformBounds(character.characterShadowCasters.BoundsInWorldSpace, worldToLocalMatrix);
                CharacterShadowComponent characterShadow = character.characterShadow;
                CharacterShadowInternalComponent characterShadowInternal = character.characterShadowInternal;
                characterShadowInternal.ProjectionBoundInLightSpace = CalculateProjectionBoundInLightSpace(settings, boundsInLightSpace, characterShadow.offset);
                Bounds projectionBoundInLightSpace = characterShadowInternal.ProjectionBoundInLightSpace;
                projectionBoundInLightSpace.max += new Vector3(0f, 0f, characterShadow.attenuation);
                flag &= GeometryUtility.TestPlanesAABB(planes, projectionBoundInLightSpace);
                characterShadowInternal.Projector.enabled = flag;

                if (flag) {
                    collector.Add(character);
                }

                num++;
            }
        }

        void CalculateProjectionData(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowNode> characters, Camera camera) {
            Quaternion rotation = settings.virtualLight.rotation;
            Matrix4x4 localToWorldMatrix = settings.virtualLight.localToWorldMatrix;
            Matrix4x4 inverse = Matrix4x4.TRS(Vector3.zero, rotation, new Vector3(1f, 1f, -1f)).inverse;
            Matrix4x4 matrix4x = inverse * camera.cameraToWorldMatrix;
            int num = settings.shadowMap.width / settings.textureSize;
            int num2 = settings.shadowMap.height / settings.textureSize;
            int num3 = 0;

            foreach (CharacterShadowNode character in characters) {
                int num4 = num3 / num;
                int num5 = num3 - num4 * num;
                Rect atlasData = CalculateLocationInAtlas(num, num2, num5, num4);
                CharacterShadowInternalComponent characterShadowInternal = character.characterShadowInternal;
                Bounds projectionBoundInLightSpace = characterShadowInternal.ProjectionBoundInLightSpace;
                SetProjectorData(characterShadowInternal, localToWorldMatrix.MultiplyPoint3x4(projectionBoundInLightSpace.center), rotation, character.characterShadow, atlasData);
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
                characterShadowInternal.CasterMaterial.SetMatrix("ViewToAtlas", GL.GetGPUProjectionMatrix(matrix4x3 * matrix4x2, true));
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

        void SetProjectorData(CharacterShadowInternalComponent characterInternal, Vector3 position, Quaternion rotation, CharacterShadowComponent characterShadow, Rect atlasData) {
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

        void GenerateDrawCommandBuffer(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowNode> characters) {
            RenderTexture shadowMap = settings.shadowMap;
            CommandBuffer commandBuffer = settings.CommandBuffer;
            commandBuffer.Clear();
            commandBuffer.GetTemporaryRT(settings.RawShadowMapNameId, shadowMap.width, shadowMap.height, 16, FilterMode.Point, shadowMap.format, RenderTextureReadWrite.Linear);
            commandBuffer.SetRenderTarget(settings.RawShadowMapId);
            commandBuffer.ClearRenderTarget(true, true, Color.black, 0f);

            foreach (CharacterShadowNode character in characters) {
                GenerateDrawCharacterCommands(commandBuffer, character.characterShadowCasters, character.characterShadowInternal);
            }

            commandBuffer.GetTemporaryRT(settings.BlurredShadowMapNameId, shadowMap.width, shadowMap.height, 0, FilterMode.Point, shadowMap.format, RenderTextureReadWrite.Linear);
            commandBuffer.Blit(settings.RawShadowMapId, settings.BlurredShadowMapId, settings.HorizontalBlurMaterial, settings.blurSize);
            commandBuffer.ReleaseTemporaryRT(settings.RawShadowMapNameId);
            commandBuffer.Blit(settings.BlurredShadowMapId, new RenderTargetIdentifier(shadowMap), settings.VerticalBlurMaterial, settings.blurSize);
            commandBuffer.ReleaseTemporaryRT(settings.BlurredShadowMapNameId);
        }

        void GenerateDrawCharacterCommands(CommandBuffer commandBuffer, CharacterShadowCastersComponent casters, CharacterShadowInternalComponent shadowInternal) {
            List<Renderer> renderers = casters.Renderers;

            for (int i = 0; i < renderers.Count; i++) {
                commandBuffer.DrawRenderer(renderers[i], shadowInternal.CasterMaterial);
            }
        }

        void UpdateShadowMapSize(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowNode> visibleCharacters, ICollection<CharacterShadowNode> allCharacters) {
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

            Object.Destroy(settings.shadowMap);
            settings.shadowMap = null;

            if (num3 > 0) {
                settings.shadowMap = new RenderTexture(num3, num3, 0, settings.ShadowMapTextureFormat, RenderTextureReadWrite.Linear);
                settings.shadowMap.isPowerOfTwo = true;
                settings.shadowMap.filterMode = FilterMode.Bilinear;
                settings.shadowMap.useMipMap = false;
            }

            foreach (CharacterShadowNode allCharacter in allCharacters) {
                allCharacter.characterShadowInternal.Projector.material.mainTexture = settings.shadowMap;
            }
        }

        [Not(typeof(CharacterShadowInternalComponent))]
        public class NotInitedCharacterShadowNode : Node {
            public CharacterShadowComponent characterShadow;

            public CharacterShadowCastersComponent characterShadowCasters;

            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class CharacterShadowNode : Node {
            public CharacterShadowComponent characterShadow;

            public CharacterShadowCastersComponent characterShadowCasters;

            public CharacterShadowInternalComponent characterShadowInternal;
        }

        public class FadeShadowNode : Node {
            public CharacterShadowComponent characterShadow;

            public CharacterShadowInternalComponent characterShadowInternal;

            public TransparencyTransitionComponent transparencyTransition;
        }

        public class BattleCameraNode : Node {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }
    }
}