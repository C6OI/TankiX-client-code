using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingLaserBehaviour : MonoBehaviour {
        const string SHADER_COLOR_NAME = "_TintColor";

        [SerializeField] float fadeInTimeSec = 3f;

        [SerializeField] float fadeOutTimeSec = 0.2f;

        [SerializeField] float maxStartAlpha = 0.9f;

        [SerializeField] float texScale = 5f;

        [SerializeField] float laserWidth = 0.05f;

        [SerializeField] float laserSourceOffset = 6f;

        [SerializeField] float laserBeginLength = 1f;

        [SerializeField] float speed1 = 0.05f;

        [SerializeField] float speed2 = 0.075f;

        [SerializeField] float delta = 3f;

        Camera _cachedCamera;

        float angle1;

        float angle2;

        Texture2D baseLaserTex;

        List<LineRenderer> baseLines;

        List<LineRenderer> beginLines;

        float currentAlpha;

        float initialAlpha;

        float segment;

        Sprite3D spot;

        LaserStates state;

        float stateTime;

        public Camera CachedCamera {
            get {
                if (!_cachedCamera) {
                    _cachedCamera = Camera.main;
                }

                return _cachedCamera;
            }
        }

        void Update() {
            angle1 += speed1 * Time.deltaTime;
            angle2 += speed2 * Time.deltaTime;
            float num = Mathf.Sin(angle1) * delta / segment;
            Vector2 mainTextureOffset = new(0f - num, 0f);
            float num2 = Mathf.Sin(angle2) * delta / segment;
            Vector2 mainTextureOffset2 = new(0f - num2, 0f);
            baseLines[0].material.mainTextureOffset = mainTextureOffset;
            baseLines[1].material.mainTextureOffset = mainTextureOffset2;

            switch (state) {
                case LaserStates.FADE_IN:
                    FadeIn();
                    break;

                case LaserStates.FADE_OUT:
                    FadeOut(false);
                    break;

                case LaserStates.DEAD:
                    FadeOut(true);
                    break;
            }
        }

        void InitializeLineRendererList<T>(List<LineRenderer> lineRenderers) where T : Component {
            T[] componentsInChildren = GetComponentsInChildren<T>();
            T[] array = componentsInChildren;

            for (int i = 0; i < array.Length; i++) {
                T val = array[i];
                LineRenderer component = val.gameObject.GetComponent<LineRenderer>();
                component.startWidth = laserWidth;
                component.endWidth = laserWidth;
                lineRenderers.Add(val.gameObject.GetComponent<LineRenderer>());
            }
        }

        public void Init() {
            spot = GetComponentsInChildren<Sprite3D>(true)[0];
            beginLines = new List<LineRenderer>();
            baseLines = new List<LineRenderer>();
            InitializeLineRendererList<ShaftLaserBeginUnityComponent>(beginLines);
            InitializeLineRendererList<ShaftLaserBaseUnityComponent>(baseLines);
            baseLaserTex = (Texture2D)baseLines[0].material.mainTexture;
            segment = texScale * laserWidth * baseLaserTex.height / baseLaserTex.width;
            angle1 = 0f;
            angle2 = 0f;
            initialAlpha = 0f;
            currentAlpha = 0f;
            stateTime = 0f;
            UpdateAlphaForAllParts(initialAlpha);
            state = LaserStates.DEFAULT;
        }

        void FadeIn() {
            if (stateTime >= fadeInTimeSec) {
                state = LaserStates.DEFAULT;
                UpdateAlphaForAllParts(1f);
            } else {
                stateTime += Time.deltaTime;
                UpdateAlphaForAllParts(Mathf.Lerp(initialAlpha, 1f, stateTime / fadeInTimeSec));
            }
        }

        void UpdateAlphaForAllParts(float alpha) {
            currentAlpha = alpha;
            UpdateAlpha(alpha, spot.material, beginLines[0].material, beginLines[1].material, baseLines[0].material, baseLines[1].material);
        }

        void UpdateAlpha(float alpha, params Material[] materials) {
            foreach (Material material in materials) {
                ClientGraphicsUtil.UpdateAlpha(material, "_TintColor", alpha);
            }
        }

        void FadeOut(bool killAfterFade) {
            if (stateTime >= fadeOutTimeSec) {
                if (killAfterFade) {
                    Destroy(gameObject);
                    return;
                }

                state = LaserStates.DEFAULT;
                UpdateAlphaForAllParts(0f);
                spot.enabled = false;
                beginLines[0].enabled = false;
                beginLines[1].enabled = false;
                baseLines[0].enabled = false;
                baseLines[1].enabled = false;
                enabled = false;
            } else {
                stateTime += Time.deltaTime;
                UpdateAlphaForAllParts(Mathf.Lerp(initialAlpha, 0f, stateTime / fadeOutTimeSec));
            }
        }

        public void UpdateTargetPosition(Vector3 startPosition, Vector3 targetPosition, bool showLaser, bool showSpot) {
            spot.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(startPosition - targetPosition));
            spot.gameObject.transform.position = targetPosition;
            Vector3 vector = Vector3.Normalize(CachedCamera.transform.position - targetPosition);
            Vector3 vector2 = targetPosition + vector * spot.offsetToCamera;
            Vector3 vector3 = vector2 - startPosition;
            Vector3 vector4 = startPosition + vector3.normalized * laserSourceOffset;
            Vector3 position = vector4 + vector3.normalized * laserBeginLength;
            beginLines[0].SetPosition(0, vector4);
            beginLines[0].SetPosition(1, position);
            beginLines[1].SetPosition(0, vector4);
            beginLines[1].SetPosition(1, position);
            baseLines[0].SetPosition(0, position);
            baseLines[0].SetPosition(1, vector2);
            baseLines[1].SetPosition(0, position);
            baseLines[1].SetPosition(1, vector2);
            spot.enabled = showSpot;
            beginLines[0].enabled = showLaser;
            beginLines[1].enabled = showLaser;
            baseLines[0].enabled = showLaser;
            baseLines[1].enabled = showLaser;
        }

        public void SetColor(Color color) {
            Color value = new(color.r, color.g, color.b, currentAlpha);
            spot.material.SetColor("_TintColor", value);
            beginLines[0].material.SetColor("_TintColor", value);
            beginLines[1].material.SetColor("_TintColor", value);
            baseLines[0].material.SetColor("_TintColor", value);
            baseLines[1].material.SetColor("_TintColor", value);
        }

        public void Show() {
            stateTime = 0f;
            initialAlpha = currentAlpha;
            state = LaserStates.FADE_IN;
            spot.enabled = true;
            beginLines[0].enabled = true;
            beginLines[1].enabled = true;
            baseLines[0].enabled = true;
            baseLines[1].enabled = true;
            enabled = true;
        }

        public void Hide() {
            stateTime = 0f;
            initialAlpha = currentAlpha;
            state = LaserStates.FADE_OUT;
        }

        public void Kill() {
            enabled = true;

            if (state != LaserStates.FADE_OUT) {
                stateTime = 0f;
            }

            state = LaserStates.DEAD;
        }

        enum LaserStates {
            FADE_IN = 0,
            FADE_OUT = 1,
            DEAD = 2,
            DEFAULT = 3
        }
    }
}