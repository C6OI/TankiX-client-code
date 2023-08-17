using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(635824351629065226L)]
    public class ScoreTableComponent : MonoBehaviour, Component {
        [SerializeField] RectTransform headerContainer;

        [SerializeField] ScoreTableRowComponent rowPrefab;

        public float rowHeight = 66f;

        public float rowSpacing = 5f;

        readonly HashSet<RectTransform> excluded = new();

        bool dirty;

        void LateUpdate() {
            Animator component = GetComponent<Animator>();
            bool flag = component == null || component.GetBool("Visible");

            if (dirty && flag) {
                UpdatePositions();
                dirty = false;
                excluded.Clear();
            }
        }

        public ScoreTableRowComponent AddRow() {
            ScoreTableRowComponent scoreTableRowComponent = Instantiate(rowPrefab);
            scoreTableRowComponent.transform.SetParent(transform, false);
            SetDirty();
            return scoreTableRowComponent;
        }

        public void Clear() {
            foreach (Transform item in transform) {
                if (item != headerContainer) {
                    Destroy(item.gameObject);
                }
            }

            foreach (Transform item2 in headerContainer) {
                Destroy(item2.gameObject);
            }

            SetHeight(0f);
        }

        public void AddHeader(ScoreTableRowIndicator headerPrefab) {
            ScoreTableRowIndicator scoreTableRowIndicator = Instantiate(headerPrefab);
            scoreTableRowIndicator.transform.SetParent(headerContainer, false);
        }

        public void SetDirty() => dirty = true;

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
            excluded.Add((RectTransform)row.transform);
            DestroyImmediate(row.gameObject);
            SetDirty();
        }
    }
}