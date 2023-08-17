using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectFadeInOutShaderColor : MonoBehaviour {
        public string ShaderColorName = "_Color";

        public float StartDelay;

        public float FadeInSpeed;

        public float FadeOutDelay;

        public float FadeOutSpeed;

        public bool UseSharedMaterial;

        public bool FadeOutAfterCollision;

        public bool UseHideStatus;

        float alpha;

        bool canStart;

        bool canStartFadeOut;

        Color currentColor;

        UpdateRankEffectSettings effectSettings;

        bool fadeInComplited;

        bool fadeOutComplited;

        bool isCollisionEnter;

        bool isIn;

        bool isInitialized;

        bool isOut;

        bool isStartDelay;

        Material mat;

        float oldAlpha;

        Color oldColor;

        void Start() {
            GetEffectSettingsComponent(transform);

            if (effectSettings != null) {
                effectSettings.CollisionEnter += prefabSettings_CollisionEnter;
            }

            InitMaterial();
        }

        void Update() {
            if (!canStart) {
                return;
            }

            if (effectSettings != null && UseHideStatus) {
                if (!effectSettings.IsVisible && fadeInComplited) {
                    fadeInComplited = false;
                }

                if (effectSettings.IsVisible && fadeOutComplited) {
                    fadeOutComplited = false;
                }
            }

            if (UseHideStatus) {
                if (isIn && effectSettings != null && effectSettings.IsVisible && !fadeInComplited) {
                    FadeIn();
                }

                if (isOut && effectSettings != null && !effectSettings.IsVisible && !fadeOutComplited) {
                    FadeOut();
                }
            } else if (!FadeOutAfterCollision) {
                if (isIn && !fadeInComplited) {
                    FadeIn();
                }

                if (isOut && canStartFadeOut && !fadeOutComplited) {
                    FadeOut();
                }
            } else {
                if (isIn && !fadeInComplited) {
                    FadeIn();
                }

                if (isOut && isCollisionEnter && canStartFadeOut && !fadeOutComplited) {
                    FadeOut();
                }
            }
        }

        void OnEnable() {
            if (isInitialized) {
                InitDefaultVariables();
            }
        }

        void GetEffectSettingsComponent(Transform tr) {
            Transform parent = tr.parent;

            if (parent != null) {
                effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();

                if (effectSettings == null) {
                    GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        public void UpdateMaterial(Material instanceMaterial) {
            mat = instanceMaterial;
            InitMaterial();
        }

        void InitMaterial() {
            if (isInitialized) {
                return;
            }

            if (GetComponent<Renderer>() != null) {
                mat = GetComponent<Renderer>().material;
            } else {
                LineRenderer component = GetComponent<LineRenderer>();

                if (component != null) {
                    mat = component.material;
                } else {
                    Projector component2 = GetComponent<Projector>();

                    if (component2 != null) {
                        if (!component2.material.name.EndsWith("(Instance)")) {
                            component2.material = new Material(component2.material) {
                                name = component2.material.name + " (Instance)"
                            };
                        }

                        mat = component2.material;
                    }
                }
            }

            if (!(mat == null)) {
                oldColor = mat.GetColor(ShaderColorName);
                isStartDelay = StartDelay > 0.001f;
                isIn = FadeInSpeed > 0.001f;
                isOut = FadeOutSpeed > 0.001f;
                InitDefaultVariables();
                isInitialized = true;
            }
        }

        void InitDefaultVariables() {
            fadeInComplited = false;
            fadeOutComplited = false;
            canStartFadeOut = false;
            isCollisionEnter = false;
            oldAlpha = 0f;
            alpha = 0f;
            canStart = false;
            currentColor = oldColor;

            if (isIn) {
                currentColor.a = 0f;
            }

            mat.SetColor(ShaderColorName, currentColor);

            if (isStartDelay) {
                Invoke("SetupStartDelay", StartDelay);
            } else {
                canStart = true;
            }

            if (!isIn) {
                if (!FadeOutAfterCollision) {
                    Invoke("SetupFadeOutDelay", FadeOutDelay);
                }

                oldAlpha = oldColor.a;
            }
        }

        void prefabSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e) {
            isCollisionEnter = true;

            if (!isIn && FadeOutAfterCollision) {
                Invoke("SetupFadeOutDelay", FadeOutDelay);
            }
        }

        void SetupStartDelay() => canStart = true;

        void SetupFadeOutDelay() => canStartFadeOut = true;

        void FadeIn() {
            alpha = oldAlpha + Time.deltaTime / FadeInSpeed;

            if (alpha >= oldColor.a) {
                fadeInComplited = true;
                alpha = oldColor.a;
                Invoke("SetupFadeOutDelay", FadeOutDelay);
            }

            currentColor.a = alpha;
            mat.SetColor(ShaderColorName, currentColor);
            oldAlpha = alpha;
        }

        void FadeOut() {
            alpha = oldAlpha - Time.deltaTime / FadeOutSpeed;

            if (alpha <= 0f) {
                alpha = 0f;
                fadeOutComplited = true;
            }

            currentColor.a = alpha;
            mat.SetColor(ShaderColorName, currentColor);
            oldAlpha = alpha;
        }
    }
}