using System.Collections.Generic;
using UnityEngine;

namespace Lobby.ClientControls.Impl {
    public class GlassBreaker : MonoBehaviour {
        public GameObject[] prefabs;

        [SerializeField] List<RectTransform> glassTransforms = new();

        readonly Dictionary<RectTransform, float> normalizedPositions = new();

        RectTransform buttonTransform;

        int previousCrack;

        void Start() {
            if (glassTransforms.Count == 0) {
                BreakGlass();
            }
        }

        void OnRectTransformDimensionsChange() => AdjustPostion();

        void AdjustPostion() {
            foreach (RectTransform glassTransform in glassTransforms) {
                glassTransform.anchoredPosition = RandomPosition(glassTransform);
            }
        }

        void CreateBottomGlassCrack() {
            RectTransform rectTransform = CreateGlassCrack();
            rectTransform.localScale = new Vector3(1f, 1f);
            Vector2 anchorMax = rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = default;
            rectTransform.anchoredPosition = RandomPosition(rectTransform);
        }

        void CreateTopGlassCrack() {
            RectTransform rectTransform = CreateGlassCrack();
            rectTransform.localScale = new Vector3(1f, -1f);
            Vector2 anchorMax = rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = default;
            rectTransform.anchoredPosition = RandomPosition(rectTransform);

            rectTransform.anchoredPosition =
                new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 5f);
        }

        RectTransform CreateGlassCrack() {
            GameObject original = RandomCrack();
            GameObject gameObject = Instantiate(original);
            RectTransform component = gameObject.GetComponent<RectTransform>();
            glassTransforms.Add(component);
            normalizedPositions.Add(component, Random.Range(0f, 1f));
            gameObject.transform.SetParent(transform, false);
            return component;
        }

        Vector2 RandomPosition(RectTransform rectTransform) {
            float x = normalizedPositions[rectTransform] * buttonTransform.rect.width;
            return new Vector2(x, 0f);
        }

        GameObject RandomCrack() {
            int num;

            for (num = previousCrack; num == previousCrack; num = Random.Range(0, prefabs.Length)) { }

            previousCrack = num;
            return prefabs[num];
        }

        public void BreakGlass() {
            buttonTransform = GetComponent<RectTransform>();
            ClearInstances();
            previousCrack = -1;

            if (Random.Range(0, 2) == 0) {
                CreateTopGlassCrack();
            }

            if (Random.Range(0, 2) == 0) {
                CreateBottomGlassCrack();
            }
        }

        void ClearInstances() {
            foreach (RectTransform glassTransform in glassTransforms) {
                DestroyImmediate(glassTransform.gameObject);
            }

            glassTransforms.Clear();
        }
    }
}