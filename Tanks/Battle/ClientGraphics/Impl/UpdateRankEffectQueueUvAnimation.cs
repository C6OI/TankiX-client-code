using System.Collections;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectQueueUvAnimation : MonoBehaviour {
        public int RowsFadeIn = 4;

        public int ColumnsFadeIn = 4;

        public int RowsLoop = 4;

        public int ColumnsLoop = 4;

        public float Fps = 20f;

        public bool IsBump;

        public Material NextMaterial;

        int allCount;

        int count;

        float deltaTime;

        int index;

        bool isFadeHandle;

        bool isVisible;

        void Start() {
            deltaTime = 1f / Fps;
            InitDefaultTex(RowsFadeIn, ColumnsFadeIn);
        }

        void OnBecameInvisible() => isVisible = false;

        void OnBecameVisible() {
            isVisible = true;
            StartCoroutine(UpdateTiling());
        }

        void InitDefaultTex(int rows, int colums) {
            count = rows * colums;
            index += colums - 1;
            Vector2 scale = new(1f / colums, 1f / rows);
            GetComponent<Renderer>().material.SetTextureScale("_MainTex", scale);

            if (IsBump) {
                GetComponent<Renderer>().material.SetTextureScale("_BumpMap", scale);
            }
        }

        IEnumerator UpdateTiling() {
            while (isVisible && allCount != count) {
                allCount++;
                index++;

                if (index >= count) {
                    index = 0;
                }

                Vector2 offset =
                    isFadeHandle
                        ? new Vector2(index / (float)ColumnsLoop - index / ColumnsLoop,
                            1f - index / ColumnsLoop / (float)RowsLoop) : new Vector2(
                            index / (float)ColumnsFadeIn - index / ColumnsFadeIn,
                            1f - index / ColumnsFadeIn / (float)RowsFadeIn);

                if (!isFadeHandle) {
                    GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);

                    if (IsBump) {
                        GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", offset);
                    }
                } else {
                    GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);

                    if (IsBump) {
                        GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", offset);
                    }
                }

                if (allCount == count) {
                    isFadeHandle = true;
                    GetComponent<Renderer>().material = NextMaterial;
                    InitDefaultTex(RowsLoop, ColumnsLoop);
                }

                yield return new WaitForSeconds(deltaTime);
            }
        }
    }
}