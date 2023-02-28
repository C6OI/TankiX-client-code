using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(RectTransform))]
    public class UITableViewCell : MonoBehaviour {
        [SerializeField] int index;

        public bool removed;

        Animator animator;

        readonly float moveSpeed = 600f;

        bool moveToPosition;

        UITableView tableView;

        Vector2 targetPosition;

        public TableViewCellRemoved CellRemoved { get; set; }

        public int Index {
            get => index;
            set {
                index = value;
                targetPosition = tableView.PositionForRowAtIndex(index);
            }
        }

        public RectTransform CellRect { get; private set; }

        void Awake() {
            CellRect = GetComponent<RectTransform>();
            RectTransform rectTransform = CellRect;
            Vector2 vector = new(0f, 1f);
            CellRect.anchorMax = vector;
            vector = vector;
            CellRect.anchorMin = vector;
            rectTransform.pivot = vector;
            tableView = GetComponentInParent<UITableView>();
            animator = GetComponent<Animator>();
        }

        void Update() {
            if (moveToPosition) {
                float num = Vector2.Distance(CellRect.anchoredPosition, targetPosition);

                if (num > 0.1f) {
                    CellRect.anchoredPosition = Vector2.Lerp(CellRect.anchoredPosition, targetPosition, Time.deltaTime / num * moveSpeed);
                } else {
                    moveToPosition = false;
                }
            }
        }

        protected void OnDisable() {
            animator.SetBool("show", false);
            animator.SetBool("remove", false);

            if (CellRemoved != null) {
                CellRemoved(this);
            }
        }

        protected void OnDestroy() {
            CellRemoved = null;
        }

        public void UpdatePositionImmidiate() {
            CellRect.anchoredPosition = targetPosition;
        }

        public void UpdatePosition() {
            moveToPosition = true;
        }

        public void Remove(bool toRight) {
            if (gameObject.activeSelf) {
                animator.SetBool("toRight", toRight);
                animator.SetBool("remove", true);
            } else {
                Removed();
            }
        }

        void Removed() {
            if (CellRemoved != null) {
                CellRemoved(this);
            }
        }
    }
}