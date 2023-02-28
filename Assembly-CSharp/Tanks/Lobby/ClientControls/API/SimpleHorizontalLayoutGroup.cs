using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [AddComponentMenu("Layout/Simple Horizontal Layout Group", 152)]
    public class SimpleHorizontalLayoutGroup : SimpleLayoutGroup {
        protected SimpleHorizontalLayoutGroup() { }

        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, false);
        }

        public override void CalculateLayoutInputVertical() {
            CalcAlongAxis(1, false);
        }

        public override void SetLayoutHorizontal() {
            SetChildrenAlongAxis(0, false);
        }

        public override void SetLayoutVertical() {
            SetChildrenAlongAxis(1, false);
        }
    }
}