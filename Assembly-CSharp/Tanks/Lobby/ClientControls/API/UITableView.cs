using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [AddComponentMenu("UI/UI Table view", 38)]
    public class UITableView : ScrollRect {
        [SerializeField] UITableViewCell cellPrefab;

        [SerializeField] float CellsSpacing;

        [SerializeField] float CellHeight;

        readonly List<UITableViewCell> cellsPool = new();

        List<int> currentVisibleIndexes = new();

        public UITableViewCell CellPrefab {
            get => cellPrefab;
            set => cellPrefab = value;
        }

        void Update() {
            if (Application.isPlaying) {
                UpdateCells();
                UpdateContentHeight();
            }
        }

        protected override void OnDisable() {
            base.OnDisable();

            if (!Application.isPlaying) {
                return;
            }

            for (int i = 0; i < cellsPool.Count; i++) {
                if (cellsPool[i] != null) {
                    Destroy(cellsPool[i].gameObject);
                }
            }

            cellsPool.Clear();
        }

        public void UpdateTable() {
            for (int i = 0; i < cellsPool.Count; i++) {
                if (cellsPool[i] != null) {
                    cellsPool[i].gameObject.SetActive(false);
                }
            }

            currentVisibleIndexes.Clear();
            content.anchoredPosition = Vector2.zero;
        }

        public void RemoveCell(int index, bool toRight) {
            if (!currentVisibleIndexes.Contains(index)) {
                return;
            }

            UITableViewCell cellByIndex = GetCellByIndex(index);

            if (!(cellByIndex != null)) {
                return;
            }

            cellsPool.Remove(cellByIndex);
            cellByIndex.removed = true;
            cellByIndex.CellRemoved = (TableViewCellRemoved)Delegate.Combine(cellByIndex.CellRemoved, new TableViewCellRemoved(CellRemoved));

            if (currentVisibleIndexes.Contains(cellByIndex.Index)) {
                currentVisibleIndexes.Remove(cellByIndex.Index);
            }

            for (int i = 0; i < currentVisibleIndexes.Count; i++) {
                UITableViewCell cellByIndex2 = GetCellByIndex(currentVisibleIndexes[i]);

                if (cellByIndex2 != null && cellByIndex2.Index > cellByIndex.Index) {
                    cellByIndex2.Index--;
                    currentVisibleIndexes[i]--;
                }
            }

            cellByIndex.Remove(toRight);
        }

        public void CellRemoved(UITableViewCell cell) {
            cell.CellRemoved = (TableViewCellRemoved)Delegate.Remove(cell.CellRemoved, new TableViewCellRemoved(CellRemoved));
            Destroy(cell.gameObject);
            UITableViewCell[] componentsInChildren = content.GetComponentsInChildren<UITableViewCell>();
            UITableViewCell[] array = componentsInChildren;

            foreach (UITableViewCell uITableViewCell in array) {
                uITableViewCell.UpdatePosition();
            }
        }

        protected virtual int NumberOfRows() => 0;

        protected virtual UITableViewCell CellForRowAtIndex(int index) {
            if (index < 0 || index >= NumberOfRows()) {
                return null;
            }

            UITableViewCell uITableViewCell = ReusableCell();
            uITableViewCell.Index = index;
            uITableViewCell.UpdatePositionImmidiate();
            return uITableViewCell;
        }

        void UpdateCells() {
            if (!Application.isPlaying) {
                return;
            }

            List<int> visibleIndexes = GetVisibleIndexes();

            if (visibleIndexes.Equals(currentVisibleIndexes)) {
                return;
            }

            if (visibleIndexes.Count >= currentVisibleIndexes.Count) {
                for (int i = 0; i < visibleIndexes.Count; i++) {
                    if (!currentVisibleIndexes.Contains(visibleIndexes[i])) {
                        CellForRowAtIndex(visibleIndexes[i]);
                    }
                }
            }

            if (currentVisibleIndexes.Count >= visibleIndexes.Count) {
                for (int j = 0; j < currentVisibleIndexes.Count; j++) {
                    if (!visibleIndexes.Contains(currentVisibleIndexes[j])) {
                        UITableViewCell cellByIndex = GetCellByIndex(currentVisibleIndexes[j]);

                        if (cellByIndex != null) {
                            cellByIndex.gameObject.SetActive(false);
                        }
                    }
                }
            }

            currentVisibleIndexes = visibleIndexes;
        }

        void UpdateContentHeight() {
            float num = NumberOfRows() * CellHeight + NumberOfRows() * CellsSpacing;

            if (!(content == null) && content.rect.height != num) {
                content.sizeDelta = new Vector2(content.sizeDelta.x, num);
            }
        }

        protected UITableViewCell GetCellByIndex(int index) {
            UITableViewCell[] componentsInChildren = content.GetComponentsInChildren<UITableViewCell>();
            UITableViewCell[] array = componentsInChildren;

            foreach (UITableViewCell uITableViewCell in array) {
                if (uITableViewCell.Index == index && !uITableViewCell.removed) {
                    return uITableViewCell;
                }
            }

            return null;
        }

        List<int> GetVisibleIndexes() {
            List<int> list = new();
            float height = viewport.rect.height;
            float y = content.anchoredPosition.y;
            float num = height / (CellHeight + CellsSpacing) + 1f;
            int num2 = (int)(y / (CellHeight + CellsSpacing));

            for (int i = 0; i < num; i++) {
                list.Add(num2 + i);
            }

            return list;
        }

        public Vector2 PositionForRowAtIndex(int index) => new(0f, 0f - (CellHeight * index + CellsSpacing * index));

        UITableViewCell ReusableCell() {
            foreach (UITableViewCell item in cellsPool) {
                if (!item.gameObject.activeSelf) {
                    item.gameObject.SetActive(true);
                    return item;
                }
            }

            UITableViewCell uITableViewCell = Instantiate(cellPrefab, content);
            uITableViewCell.gameObject.SetActive(true);
            cellsPool.Add(uITableViewCell);
            return uITableViewCell;
        }
    }
}