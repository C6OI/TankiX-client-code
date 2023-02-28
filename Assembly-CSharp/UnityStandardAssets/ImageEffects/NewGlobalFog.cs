using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Rendering/Global Fog")]
    internal class NewGlobalFog : PostEffectsBase {
        [Tooltip("Apply distance-based fog?")] public bool distanceFog = true;

        [Tooltip("Distance fog is based on radial distance from camera when checked")]
        public bool useRadialDistance;

        [Tooltip("Apply height-based fog?")] public bool heightFog = true;

        [Tooltip("Fog top Y coordinate")] public float height = 1f;

        [Range(0.001f, 10f)] public float heightDensity = 2f;

        [Tooltip("Push fog away from the camera by this amount")]
        public float startDistance;

        public float ShaftDensity = 1f;

        public Shader fogShader;

        public Color fogColor;

        public float fogFacCoef;

        public bool horizontalFog;

        Camera _camera;

        Material fogMaterial;

        new void Start() {
            base.Start();
            ShaftDensity = 1f;
            _camera = GetComponent<Camera>();
        }

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (!isSupported || !distanceFog && !heightFog) {
                Graphics.Blit(source, destination);
                return;
            }

            Transform transform = _camera.transform;
            float nearClipPlane = _camera.nearClipPlane;
            float farClipPlane = _camera.farClipPlane;
            float fieldOfView = _camera.fieldOfView;
            float aspect = _camera.aspect;
            Matrix4x4 identity = Matrix4x4.identity;
            float num = fieldOfView * 0.5f;
            Vector3 vector = transform.right * nearClipPlane * Mathf.Tan(num * ((float)Math.PI / 180f)) * aspect;
            Vector3 vector2 = transform.up * nearClipPlane * Mathf.Tan(num * ((float)Math.PI / 180f));
            Vector3 vector3 = transform.forward * nearClipPlane - vector + vector2;
            float num2 = vector3.magnitude * farClipPlane / nearClipPlane;
            vector3.Normalize();
            vector3 *= num2;
            Vector3 vector4 = transform.forward * nearClipPlane + vector + vector2;
            vector4.Normalize();
            vector4 *= num2;
            Vector3 vector5 = transform.forward * nearClipPlane + vector - vector2;
            vector5.Normalize();
            vector5 *= num2;
            Vector3 vector6 = transform.forward * nearClipPlane - vector - vector2;
            vector6.Normalize();
            vector6 *= num2;
            identity.SetRow(0, vector3);
            identity.SetRow(1, vector4);
            identity.SetRow(2, vector5);
            identity.SetRow(3, vector6);
            Vector3 position = transform.position;
            float num3 = position.y - height;
            float z = !(num3 <= 0f) ? 0f : 1f;
            fogMaterial.SetMatrix("_FrustumCornersWS", identity);
            fogMaterial.SetVector("_CameraWS", position);
            fogMaterial.SetVector("_HeightParams", new Vector4(height, num3, z, heightDensity * 0.5f * ShaftDensity));
            fogMaterial.SetVector("_DistanceParams", new Vector4(0f - Mathf.Max(startDistance, 0f), 0f, 0f, 0f));
            fogMaterial.SetVector("_FogColor", fogColor);
            fogMaterial.SetFloat("_FogFacCoef", fogFacCoef);
            fogMaterial.SetInt("_horizontalFog", horizontalFog ? 1 : 0);
            FogMode fogMode = RenderSettings.fogMode;
            float fogDensity = RenderSettings.fogDensity;
            float fogStartDistance = RenderSettings.fogStartDistance;
            float fogEndDistance = RenderSettings.fogEndDistance;
            bool flag = fogMode == FogMode.Linear;
            float num4 = !flag ? 0f : fogEndDistance - fogStartDistance;
            float num5 = !(Mathf.Abs(num4) > 0.0001f) ? 0f : 1f / num4;
            Vector4 value = default;
            value.x = fogDensity * 1.2011224f;
            value.y = fogDensity * 1.442695f;
            value.z = !flag ? 0f : 0f - num5;
            value.w = !flag ? 0f : fogEndDistance * num5;
            fogMaterial.SetVector("_SceneFogParams", value);
            fogMaterial.SetVector("_SceneFogMode", new Vector4((float)fogMode, useRadialDistance ? 1 : 0, 0f, 0f));
            int num6 = 0;
            CustomGraphicsBlit(passNr: !distanceFog || !heightFog ? distanceFog ? 1 : 2 : 0, source: source, dest: destination, fxMaterial: fogMaterial);
        }

        public override bool CheckResources() {
            CheckSupport(true);
            fogMaterial = CheckShaderAndCreateMaterial(fogShader, fogMaterial);

            if (!isSupported) {
                ReportAutoDisable();
            }

            return isSupported;
        }

        static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr) {
            RenderTexture.active = dest;
            fxMaterial.SetTexture("_MainTex", source);
            GL.PushMatrix();
            GL.LoadOrtho();
            fxMaterial.SetPass(passNr);
            GL.Begin(7);
            GL.MultiTexCoord2(0, 0f, 0f);
            GL.Vertex3(0f, 0f, 3f);
            GL.MultiTexCoord2(0, 1f, 0f);
            GL.Vertex3(1f, 0f, 2f);
            GL.MultiTexCoord2(0, 1f, 1f);
            GL.Vertex3(1f, 1f, 1f);
            GL.MultiTexCoord2(0, 0f, 1f);
            GL.Vertex3(0f, 1f, 0f);
            GL.End();
            GL.PopMatrix();
        }
    }
}