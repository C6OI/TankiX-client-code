using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableHeaderComponent : MonoBehaviour, Component {
        public List<ScoreTableRowIndicator> headers = new();

        [SerializeField] RectTransform headerTitle;

        [SerializeField] RectTransform scoreHeaderContainer;

        public void Clear() {
            IEnumerator enumerator = scoreHeaderContainer.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;

                    if (transform != headerTitle) {
                        Destroy(transform.gameObject);
                    }
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        public void AddHeader(ScoreTableRowIndicator headerPrefab) {
            ScoreTableRowIndicator scoreTableRowIndicator = Instantiate(headerPrefab);
            scoreTableRowIndicator.transform.SetParent(scoreHeaderContainer, false);
        }

        public void SetDirty() {
            LayoutRebuilder.MarkLayoutForRebuild(scoreHeaderContainer);
        }
    }
}