using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class UITintController : MonoBehaviour {
        public Color tint = Color.white;

        readonly List<UITint> elements = new();

        Color lastTint;

        void Update() {
            if (tint != lastTint) {
                for (int i = 0; i < elements.Count; i++) {
                    SetTint(elements[i], tint);
                }

                lastTint = tint;
            }
        }

        public void AddElement(UITint element) {
            elements.Add(element);
        }

        public void RemoveElement(UITint element) {
            elements.Remove(element);
        }

        protected virtual void SetTint(UITint uiTint, Color tint) {
            uiTint.SetTint(tint);
        }
    }
}