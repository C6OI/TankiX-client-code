using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class LayoutRebuildMarker : UIBehaviour {
        public RectTransform target;

        protected override void OnRectTransformDimensionsChange() {
            LayoutRebuilder.MarkLayoutForRebuild(target);
        }

        public void Init(RectTransform target) {
            this.target = target;
        }
    }
}