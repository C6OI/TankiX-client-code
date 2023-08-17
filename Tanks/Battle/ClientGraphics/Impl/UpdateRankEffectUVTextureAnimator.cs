using System.Collections;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    class UpdateRankEffectUVTextureAnimator : MonoBehaviour {
        public Material[] AnimatedMaterialsNotInstance;

        public int Rows = 4;

        public int Columns = 4;

        public float Fps = 20f;

        public int OffsetMat;

        public Vector2 SelfTiling;

        public bool IsLoop = true;

        public bool IsReverse;

        public bool IsRandomOffsetForInctance;

        public bool IsBump;

        public bool IsHeight;

        public bool IsCutOut;

        int allCount;

        int count;

        Renderer currentRenderer;

        float deltaFps;

        int index;

        Material instanceMaterial;

        bool isCorutineStarted;

        bool isInizialised;

        bool isVisible;

        void Start() {
            InitMaterial();
            InitDefaultVariables();
            isInizialised = true;
            isVisible = true;
            StartCoroutine(UpdateCorutine());
        }

        void OnEnable() {
            if (isInizialised) {
                InitDefaultVariables();
                isVisible = true;

                if (!isCorutineStarted) {
                    StartCoroutine(UpdateCorutine());
                }
            }
        }

        void OnDisable() {
            isCorutineStarted = false;
            isVisible = false;
            StopAllCoroutines();
        }

        void OnBecameInvisible() => isVisible = false;

        void OnBecameVisible() {
            isVisible = true;

            if (!isCorutineStarted) {
                StartCoroutine(UpdateCorutine());
            }
        }

        public void SetInstanceMaterial(Material mat, Vector2 offsetMat) {
            instanceMaterial = mat;
            InitDefaultVariables();
        }

        void InitDefaultVariables() {
            allCount = 0;
            deltaFps = 1f / Fps;
            count = Rows * Columns;
            index = Columns - 1;
            Vector2 offset = new(index / (float)Columns - index / Columns, 1f - index / Columns / (float)Rows);
            OffsetMat = IsRandomOffsetForInctance ? Random.Range(0, count) : OffsetMat - OffsetMat / count * count;
            Vector2 scale = !(SelfTiling == Vector2.zero) ? SelfTiling : new Vector2(1f / Columns, 1f / Rows);

            if (AnimatedMaterialsNotInstance.Length > 0) {
                Material[] animatedMaterialsNotInstance = AnimatedMaterialsNotInstance;

                foreach (Material material in animatedMaterialsNotInstance) {
                    material.SetTextureScale("_MainTex", scale);
                    material.SetTextureOffset("_MainTex", Vector2.zero);

                    if (IsBump) {
                        material.SetTextureScale("_BumpMap", scale);
                        material.SetTextureOffset("_BumpMap", Vector2.zero);
                    }

                    if (IsHeight) {
                        material.SetTextureScale("_HeightMap", scale);
                        material.SetTextureOffset("_HeightMap", Vector2.zero);
                    }

                    if (IsCutOut) {
                        material.SetTextureScale("_CutOut", scale);
                        material.SetTextureOffset("_CutOut", Vector2.zero);
                    }
                }
            } else if (instanceMaterial != null) {
                instanceMaterial.SetTextureScale("_MainTex", scale);
                instanceMaterial.SetTextureOffset("_MainTex", offset);

                if (IsBump) {
                    instanceMaterial.SetTextureScale("_BumpMap", scale);
                    instanceMaterial.SetTextureOffset("_BumpMap", offset);
                }

                if (IsBump) {
                    instanceMaterial.SetTextureScale("_HeightMap", scale);
                    instanceMaterial.SetTextureOffset("_HeightMap", offset);
                }

                if (IsCutOut) {
                    instanceMaterial.SetTextureScale("_CutOut", scale);
                    instanceMaterial.SetTextureOffset("_CutOut", offset);
                }
            } else if (currentRenderer != null) {
                currentRenderer.material.SetTextureScale("_MainTex", scale);
                currentRenderer.material.SetTextureOffset("_MainTex", offset);

                if (IsBump) {
                    currentRenderer.material.SetTextureScale("_BumpMap", scale);
                    currentRenderer.material.SetTextureOffset("_BumpMap", offset);
                }

                if (IsHeight) {
                    currentRenderer.material.SetTextureScale("_HeightMap", scale);
                    currentRenderer.material.SetTextureOffset("_HeightMap", offset);
                }

                if (IsCutOut) {
                    currentRenderer.material.SetTextureScale("_CutOut", scale);
                    currentRenderer.material.SetTextureOffset("_CutOut", offset);
                }
            }
        }

        void InitMaterial() {
            if (GetComponent<Renderer>() != null) {
                currentRenderer = GetComponent<Renderer>();
                return;
            }

            Projector component = GetComponent<Projector>();

            if (component != null) {
                if (!component.material.name.EndsWith("(Instance)")) {
                    component.material = new Material(component.material) {
                        name = component.material.name + " (Instance)"
                    };
                }

                instanceMaterial = component.material;
            }
        }

        IEnumerator UpdateCorutine() {
            isCorutineStarted = true;

            while (isVisible && (IsLoop || allCount != count)) {
                UpdateCorutineFrame();

                if (!IsLoop && allCount == count) {
                    break;
                }

                yield return new WaitForSeconds(deltaFps);
            }

            isCorutineStarted = false;
        }

        void UpdateCorutineFrame() {
            if (currentRenderer == null && instanceMaterial == null && AnimatedMaterialsNotInstance.Length == 0) {
                return;
            }

            allCount++;

            if (IsReverse) {
                index--;
            } else {
                index++;
            }

            if (index >= count) {
                index = 0;
            }

            if (AnimatedMaterialsNotInstance.Length > 0) {
                for (int i = 0; i < AnimatedMaterialsNotInstance.Length; i++) {
                    int num = i * OffsetMat + index + OffsetMat;
                    num -= num / count * count;
                    Vector2 offset = new(num / (float)Columns - num / Columns, 1f - num / Columns / (float)Rows);
                    AnimatedMaterialsNotInstance[i].SetTextureOffset("_MainTex", offset);

                    if (IsBump) {
                        AnimatedMaterialsNotInstance[i].SetTextureOffset("_BumpMap", offset);
                    }

                    if (IsHeight) {
                        AnimatedMaterialsNotInstance[i].SetTextureOffset("_HeightMap", offset);
                    }

                    if (IsCutOut) {
                        AnimatedMaterialsNotInstance[i].SetTextureOffset("_CutOut", offset);
                    }
                }

                return;
            }

            Vector2 offset2;

            if (IsRandomOffsetForInctance) {
                int num2 = index + OffsetMat;
                offset2 = new Vector2(num2 / (float)Columns - num2 / Columns, 1f - num2 / Columns / (float)Rows);
            } else {
                offset2 = new Vector2(index / (float)Columns - index / Columns, 1f - index / Columns / (float)Rows);
            }

            if (instanceMaterial != null) {
                instanceMaterial.SetTextureOffset("_MainTex", offset2);

                if (IsBump) {
                    instanceMaterial.SetTextureOffset("_BumpMap", offset2);
                }

                if (IsHeight) {
                    instanceMaterial.SetTextureOffset("_HeightMap", offset2);
                }

                if (IsCutOut) {
                    instanceMaterial.SetTextureOffset("_CutOut", offset2);
                }
            } else if (currentRenderer != null) {
                currentRenderer.material.SetTextureOffset("_MainTex", offset2);

                if (IsBump) {
                    currentRenderer.material.SetTextureOffset("_BumpMap", offset2);
                }

                if (IsHeight) {
                    currentRenderer.material.SetTextureOffset("_HeightMap", offset2);
                }

                if (IsCutOut) {
                    currentRenderer.material.SetTextureOffset("_CutOut", offset2);
                }
            }
        }
    }
}