using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class ResizeDispatcher : MonoBehaviour {
        [SerializeField] List<ResizeListener> listeners;

        void OnEnable() {
            OnRectTransformDimensionsChange();
        }

        void OnRectTransformDimensionsChange() {
            RectTransform component = GetComponent<RectTransform>();

            foreach (ResizeListener listener in listeners) {
                listener.OnResize(component);
            }
        }
    }
}