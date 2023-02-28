using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(635824351629065226L)]
    public class ScoreTableComponent : MonoBehaviour, Component {
        [SerializeField] RectTransform headerContainer;

        [SerializeField] protected ScoreTableRowComponent rowPrefab;

        public float rowHeight = 66f;

        public float rowSpacing = 5f;

        bool dirty;

        readonly HashSet<RectTransform> excluded = new();

        bool headerDirty;

        List<ScoreTableRowIndicator> rowIndicators;

        readonly Queue<ScoreTableRowComponent> rowsCache = new();

        bool useRowsCache;

        void LateUpdate() {
            Animator component = GetComponent<Animator>();
            bool flag = component == null || component.GetBool("Visible");

            if (dirty && flag) {
                UpdatePositions();
                dirty = false;
                excluded.Clear();
            }

            if (headerDirty) {
                headerDirty = false;
                ScoreTableHeaderComponent component2 = GetComponent<ScoreTableHeaderComponent>();

                if (component2 != null) {
                    component2.SetDirty();
                }
            }
        }

        public void InitRowsCache(int cacheSize, List<ScoreTableRowIndicator> rowIndicators) {
            for (int i = 0; i < cacheSize; i++) {
                ScoreTableRowComponent scoreTableRowComponent = Instantiate(rowPrefab);
                scoreTableRowComponent.AddIndicators(rowIndicators);
                scoreTableRowComponent.gameObject.SetActive(false);
                rowsCache.Enqueue(scoreTableRowComponent);
            }

            this.rowIndicators = rowIndicators;
            useRowsCache = true;
        }

        public ScoreTableRowComponent AddRow() {
            if (useRowsCache && rowsCache.Count != 0) {
                ScoreTableRowComponent scoreTableRowComponent = rowsCache.Dequeue();
                scoreTableRowComponent.gameObject.SetActive(true);
                scoreTableRowComponent.transform.SetParent(transform, false);
                scoreTableRowComponent.transform.localScale = Vector3.one;
                SetDirty();
                return scoreTableRowComponent;
            }

            ScoreTableRowComponent scoreTableRowComponent2 = Instantiate(rowPrefab);
            scoreTableRowComponent2.AddIndicators(rowIndicators);
            scoreTableRowComponent2.transform.SetParent(transform, false);
            SetDirty();
            return scoreTableRowComponent2;
        }

        public void Clear() {
            IEnumerator enumerator = this.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;

                    if (transform != headerContainer) {
                        Destroy(transform.gameObject);
                    }
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }

            SetHeight(0f);
            ClearCache();
        }

        public void SetDirty() {
            dirty = true;
            headerDirty = true;
        }

        public void SetHeaderDirty() {
            headerDirty = true;
        }

        public void UpdatePositions() {
            int num = 0;
            int i = 0;

            for (int childCount = transform.childCount; i < childCount; i++) {
                RectTransform rectTransform = (RectTransform)transform.GetChild(i);

                if (!(rectTransform == null) && !excluded.Contains(rectTransform)) {
                    ScoreTableRowComponent component = rectTransform.GetComponent<ScoreTableRowComponent>();

                    if (component == null) {
                        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
                    } else if (component.Position != 0) {
                        num++;
                        float y = 0f - component.Position * (rowHeight + rowSpacing);
                        Vector2 anchoredPosition = rectTransform.anchoredPosition;
                        anchoredPosition.y = y;
                        rectTransform.anchoredPosition = anchoredPosition;
                    }
                }
            }

            int num2 = 0;
            int j = 0;

            for (int childCount2 = transform.childCount; j < childCount2; j++) {
                RectTransform rectTransform2 = (RectTransform)transform.GetChild(j);
                ScoreTableRowComponent component2 = rectTransform2.GetComponent<ScoreTableRowComponent>();

                if (!(component2 == null) && component2.Position == 0) {
                    int num3 = num + num2 + 1;
                    num2++;
                    float y2 = 0f - num3 * (rowHeight + rowSpacing);
                    Vector2 anchoredPosition2 = rectTransform2.anchoredPosition;
                    anchoredPosition2.y = y2;
                    rectTransform2.anchoredPosition = anchoredPosition2;
                }
            }

            float height = (num2 + num) * (rowHeight + rowSpacing) + rowHeight;
            SetHeight(height);
        }

        void SetHeight(float height) {
            Vector2 sizeDelta = ((RectTransform)transform).sizeDelta;
            sizeDelta.y = height;
            ((RectTransform)transform).sizeDelta = sizeDelta;
            LayoutElement component = GetComponent<LayoutElement>();

            if (component != null) {
                component.preferredHeight = height;
            }
        }

        public void RemoveRow(ScoreTableRowComponent row) {
            if (useRowsCache) {
                rowsCache.Enqueue(row);

                foreach (ScoreTableRowIndicator value in row.indicators.Values) {
                    EntityBehaviour component = value.GetComponent<EntityBehaviour>();

                    if (!(component == null)) {
                        component.DetachFromEntity();
                    }
                }

                row.gameObject.SetActive(false);
                row.transform.SetParent(null, false);
                SetDirty();
            } else {
                excluded.Add((RectTransform)row.transform);
                DestroyImmediate(row.gameObject);
                SetDirty();
            }
        }

        public void ClearCache() {
            if (useRowsCache) {
                while (rowsCache.Count > 0) {
                    ScoreTableRowComponent scoreTableRowComponent = rowsCache.Dequeue();
                    Destroy(scoreTableRowComponent.gameObject);
                }
            }
        }
    }
}