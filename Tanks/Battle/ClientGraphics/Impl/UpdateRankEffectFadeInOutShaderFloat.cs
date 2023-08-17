using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectFadeInOutShaderFloat : MonoBehaviour {
        public string PropertyName = "_CutOut";

        public float MaxFloat = 1f;

        public float StartDelay;

        public float FadeInSpeed;

        public float FadeOutDelay;

        public float FadeOutSpeed;

        public bool FadeOutAfterCollision;

        public bool UseHideStatus;

        bool canStart;

        bool canStartFadeOut;

        float currentFloat;

        UpdateRankEffectSettings effectSettings;

        bool fadeInComplited;

        bool fadeOutComplited;

        bool isCollisionEnter;

        bool isIn;

        bool isInitialized;

        bool isOut;

        bool isStartDelay;

        Material mat;

        float oldFloat;

        Material OwnMaterial;

        bool previousFrameVisibleStatus;

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
            canStart = false;
            isCollisionEnter = false;
            oldFloat = 0f;
            currentFloat = MaxFloat;

            if (isIn) {
                currentFloat = 0f;
            }

            mat.SetFloat(PropertyName, currentFloat);

            if (isStartDelay) {
                Invoke("SetupStartDelay", StartDelay);
            } else {
                canStart = true;
            }

            if (!isIn) {
                if (!FadeOutAfterCollision) {
                    Invoke("SetupFadeOutDelay", FadeOutDelay);
                }

                oldFloat = MaxFloat;
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
            currentFloat = oldFloat + Time.deltaTime / FadeInSpeed * MaxFloat;

            if (currentFloat >= MaxFloat) {
                fadeInComplited = true;
                currentFloat = MaxFloat;
                Invoke("SetupFadeOutDelay", FadeOutDelay);
            }

            mat.SetFloat(PropertyName, currentFloat);
            oldFloat = currentFloat;
        }

        void FadeOut() {
            currentFloat = oldFloat - Time.deltaTime / FadeOutSpeed * MaxFloat;

            if (currentFloat <= 0f) {
                currentFloat = 0f;
                fadeOutComplited = true;
            }

            mat.SetFloat(PropertyName, currentFloat);
            oldFloat = currentFloat;
        }
    }
}