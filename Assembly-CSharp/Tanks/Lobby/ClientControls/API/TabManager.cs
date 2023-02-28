using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class TabManager : MonoBehaviour {
        [SerializeField] protected RectTransform tabsContainer;

        public int index;

        protected virtual void OnEnable() {
            Show(index);
        }

        protected virtual void OnDisable() {
            Tab[] componentsInChildren = GetComponentsInChildren<Tab>(true);

            for (int i = 0; i < componentsInChildren.Length; i++) {
                componentsInChildren[i].Hide();
            }
        }

        public virtual void Show(int newIndex) {
            index = newIndex;
            Tab[] componentsInChildren = GetComponentsInChildren<Tab>(true);
            componentsInChildren[newIndex].Show();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                if (i != newIndex) {
                    componentsInChildren[i].Hide();
                }
            }
        }
    }
}