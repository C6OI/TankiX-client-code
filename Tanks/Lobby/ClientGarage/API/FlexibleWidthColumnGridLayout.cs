using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.API {
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class FlexibleWidthColumnGridLayout : UIBehaviour {
        [SerializeField] RectTransform viewport;

        protected override void Awake() {
            if (viewport == null) {
                viewport = (RectTransform)transform.parent;
            }
        }

        protected override void OnEnable() => CalculateWidthCell();

        protected override void OnRectTransformDimensionsChange() => CalculateWidthCell();

        protected override void OnTransformParentChanged() => CalculateWidthCell();

        void CalculateWidthCell() {
            float width = viewport.rect.width;
            GridLayoutGroup component = GetComponent<GridLayoutGroup>();

            if (component.constraint == GridLayoutGroup.Constraint.FixedColumnCount) {
                int constraintCount = component.constraintCount;
                float num = (width - component.spacing.x * (constraintCount - 1)) / constraintCount;
                component.cellSize = new Vector2((int)num, component.cellSize.y);
            }
        }
    }
}