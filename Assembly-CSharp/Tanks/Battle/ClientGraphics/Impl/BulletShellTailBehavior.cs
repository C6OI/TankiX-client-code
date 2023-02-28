using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BulletShellTailBehavior : MonoBehaviour {
        [SerializeField] int yFrames = 4;

        [SerializeField] int fps = 4;

        [SerializeField] LineRenderer lineRenderer;

        [SerializeField] float zFrom = 0.5f;

        [SerializeField] float zTo = -3f;

        [SerializeField] float zTime = 0.25f;

        float frameOffset;

        int lastIndex;

        Vector2 size;

        bool tailGrow;

        float timer;

        void Update() {
            timer += Time.deltaTime;
            int num = Mathf.RoundToInt(timer * fps) % yFrames;

            if (num != lastIndex) {
                Vector2 value = new(0f, frameOffset * num);
                lineRenderer.material.SetTextureOffset("_MainTex", value);
                lastIndex = num;
            }

            if (timer <= zTime) {
                lineRenderer.SetPositions(new Vector3[2] {
                    new(0f, 0f, Mathf.Lerp(zFrom, zTo, timer / zTime)),
                    new(0f, 0f, zFrom)
                });
            } else if (!tailGrow) {
                tailGrow = true;

                lineRenderer.SetPositions(new Vector3[2] {
                    new(0f, 0f, zTo),
                    new(0f, 0f, zFrom)
                });
            }
        }

        void OnEnable() {
            timer = 0f;
            frameOffset = 1f / yFrames;
            lineRenderer.material.SetTextureScale("_MainTex", new Vector2(1f, frameOffset));
            lastIndex = -1;
            tailGrow = false;
        }
    }
}