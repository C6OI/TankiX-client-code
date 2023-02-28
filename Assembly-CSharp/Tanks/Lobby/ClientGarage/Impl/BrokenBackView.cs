using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BrokenBackView : MonoBehaviour {
        [SerializeField] float animationTime = 1f;

        [SerializeField] AnimationCurve curve;

        [SerializeField] RectTransform[] parts;

        BrokenBackPartsData[] partsData;

        float timer;

        void Update() {
            timer += Time.deltaTime;
            float t = curve.Evaluate(Mathf.Clamp01(timer / animationTime));

            for (int i = 0; i < parts.Length; i++) {
                parts[i].offsetMin = Vector3.Lerp(partsData[i].OffsetMin, Vector2.zero, t);
                parts[i].offsetMax = Vector3.Lerp(partsData[i].OffsetMax, Vector2.zero, t);
            }

            if (timer >= animationTime) {
                enabled = false;
            }
        }

        void OnEnable() {
            Init();
            timer = 0f;
        }

        void Init() {
            if (partsData == null) {
                partsData = new BrokenBackPartsData[parts.Length];

                for (int i = 0; i < parts.Length; i++) {
                    partsData[i] = new BrokenBackPartsData(parts[i].offsetMin, parts[i].offsetMax);
                }
            }
        }

        public void BreakBack() {
            Init();

            for (int i = 0; i < parts.Length; i++) {
                parts[i].offsetMin = partsData[i].OffsetMin;
                parts[i].offsetMax = partsData[i].OffsetMax;
            }
        }

        class BrokenBackPartsData {
            public BrokenBackPartsData(Vector2 offsetMin, Vector2 offsetMax) {
                OffsetMin = offsetMin;
                OffsetMax = offsetMax;
            }

            public Vector2 OffsetMin { get; }

            public Vector2 OffsetMax { get; }
        }
    }
}