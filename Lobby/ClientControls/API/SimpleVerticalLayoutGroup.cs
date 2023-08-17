using UnityEngine;

namespace Lobby.ClientControls.API {
    [AddComponentMenu("Layout/Simple Vertical Layout Group", 153)]
    public class SimpleVerticalLayoutGroup : SimpleLayoutGroup {
        protected SimpleVerticalLayoutGroup() { }

        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, true);
        }

        public override void CalculateLayoutInputVertical() => CalcAlongAxis(1, true);

        public override void SetLayoutHorizontal() => SetChildrenAlongAxis(0, true);

        public override void SetLayoutVertical() => SetChildrenAlongAxis(1, true);
    }
}