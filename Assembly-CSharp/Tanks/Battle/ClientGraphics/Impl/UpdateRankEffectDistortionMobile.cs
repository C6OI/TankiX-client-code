using System.Collections;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectDistortionMobile : MonoBehaviour {
        const int dropedFrames = 50;
        public float TextureScale = 1f;

        public RenderTextureFormat RenderTextureFormat;

        public FilterMode FilterMode;

        public LayerMask CullingMask = -17;

        public RenderingPath RenderingPath;

        public int FPSWhenMoveCamera = 40;

        public int FPSWhenStaticCamera = 20;

        Camera cameraInstance;

        bool canUpdateCamera;

        WaitForSeconds fpsMove;

        WaitForSeconds fpsStatic;

        int frameCountWhenCameraIsStatic;

        GameObject goCamera;

        Transform instanceCameraTransform;

        bool isInitialized;

        bool isStaticUpdate;

        Vector3 oldPosition;

        Quaternion oldRotation;

        RenderTexture renderTexture;

        void Update() {
            if (!isInitialized) {
                Initialize();
                StartCoroutine(RepeatCameraMove());
                StartCoroutine(RepeatCameraStatic());
            }

            if (Vector3.SqrMagnitude(instanceCameraTransform.position - oldPosition) <= 1E-05f && instanceCameraTransform.rotation == oldRotation) {
                frameCountWhenCameraIsStatic++;

                if (frameCountWhenCameraIsStatic >= 50) {
                    isStaticUpdate = true;
                }
            } else {
                frameCountWhenCameraIsStatic = 0;
                isStaticUpdate = false;
            }

            oldPosition = instanceCameraTransform.position;
            oldRotation = instanceCameraTransform.rotation;

            if (cameraInstance == null) {
                canUpdateCamera = false;
            } else if (canUpdateCamera) {
                cameraInstance.enabled = true;
                canUpdateCamera = false;
            } else if (cameraInstance.enabled) {
                cameraInstance.enabled = false;
            }
        }

        void OnEnable() {
            fpsMove = new WaitForSeconds(1f / FPSWhenMoveCamera);
            fpsStatic = new WaitForSeconds(1f / FPSWhenStaticCamera);
        }

        void OnDisable() {
            if ((bool)goCamera) {
                DestroyImmediate(goCamera);
                goCamera = null;
            }

            if ((bool)renderTexture) {
                DestroyImmediate(renderTexture);
                renderTexture = null;
            }

            isInitialized = false;
        }

        void OnBecameInvisible() {
            if (goCamera != null) {
                goCamera.SetActive(false);
            }
        }

        void OnBecameVisible() {
            if (goCamera != null) {
                goCamera.SetActive(true);
            }
        }

        IEnumerator RepeatCameraMove() {
            while (true) {
                if (!isStaticUpdate) {
                    canUpdateCamera = true;
                }

                yield return fpsMove;
            }
        }

        IEnumerator RepeatCameraStatic() {
            while (true) {
                if (isStaticUpdate) {
                    canUpdateCamera = true;
                }

                yield return fpsStatic;
            }
        }

        void Initialize() {
            goCamera = new GameObject("RenderTextureCamera");
            cameraInstance = goCamera.AddComponent<Camera>();
            Camera main = Camera.main;
            cameraInstance.CopyFrom(main);
            cameraInstance.depth += 1f;
            cameraInstance.cullingMask = CullingMask;
            cameraInstance.renderingPath = RenderingPath;
            goCamera.transform.parent = main.transform;
            renderTexture = new RenderTexture(Mathf.RoundToInt(Screen.width * TextureScale), Mathf.RoundToInt(Screen.height * TextureScale), 16, RenderTextureFormat);
            renderTexture.DiscardContents();
            renderTexture.filterMode = FilterMode;
            cameraInstance.targetTexture = renderTexture;
            instanceCameraTransform = cameraInstance.transform;
            oldPosition = instanceCameraTransform.position;
            Shader.SetGlobalTexture("_GrabTextureMobile", renderTexture);
            isInitialized = true;
        }
    }
}